using System;
using DG.Tweening;
using DTT.UI.ProceduralUI;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;
using User;

public class HomeAppManager : MonoBehaviour
{
    [SerializeField] private HomeView view;
    private DatabaseReference databaseReference;
    private Tweener tween;
    private DateTime currentDate, firstDate;
    private CaneHistoryData currentHistory;
    private string[] dayOfWeekThai = { "อาทิตย์", "จันทร์", "อังคาร", "พุธ", "พฤหัสบดี", "ศุกร์", "เสาร์" };
    private string[] monthNameThai =
    {
        "มกราคม", "กุมภาพันธ์", "มีนาคม", "เมษายน", "พฤษภาคม", "มิถุนายน", "กรกฎาคม", "สิงหาคม", "กันยายน", "ตุลาคม",
        "พฤศจิกายน", "ธันวาคม"
    };

    private void Awake()
    {
        view.CircleImage.raycastTarget = false;
        CircleUi();
        CleanUp();
    }

    private void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        
        databaseReference.Child(UserDataApp.CaneId).Child("history").ValueChanged += UpdateCurrentData;

    }

    private void CleanUp()
    {
        UserDataApp.History.Clear();

        view.HourText.text = "-";
        view.MinuteText.text = "-";
        view.SecondText.text = "-";
        view.DateTimeText.text = "-";
        view.CurrentFallCountText.text = "-";
        view.SumFallCountText.text = "-";
        view.StepCountText.text = "-";
        view.RangeDateText.text = "-";
        view.DistanceText.text = "-";
    }

    private void OnDisable()
    {
        databaseReference.Child(UserDataApp.CaneId).Child("history").ValueChanged -= UpdateCurrentData;
    }
    
    

    private void UpdateCurrentData(object sender, ValueChangedEventArgs e)
    {
        if (view != null
            &&view.HourText != null 
            && view.MinuteText != null 
            && view.SecondText != null 
            && view.DateTimeText != null
            && view.CurrentFallCountText != null
            && view.SumFallCountText != null 
            && view.StepCountText != null
            && view.RangeDateText != null
            && view.DistanceText != null)
        {
            CleanUp();
            
            
            if (e.DatabaseError != null)
            {
                Debug.Log("Error Get History : " + e.DatabaseError.Message + e.DatabaseError.Details);
                return;
            }

            if (e.Snapshot.Exists)
            {
                foreach (var childdata in e.Snapshot.Children)
                {
                    var key = childdata.Key;
                    var val = JsonUtility.FromJson<CaneHistoryData>(childdata.GetRawJsonValue());
                    var day = key.Substring(0, 2);
                    var month = key.Substring(2, 2);
                    var year = key.Substring(4, 4);
                    var datetime = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
                    UserDataApp.History.TryAdd(datetime, val);
                }
                
                foreach (var history in UserDataApp.History)
                {

                    if (history.Key.Date == DateTime.Now.Date)
                    {
                        currentDate = history.Key.Date;
                        currentHistory = history.Value;
                        break;
                    }
                }
                
                HomeUi();
            }
        }
        
    }

    public void HomeUi()
    {
        Date();
        Time();
        Distance();
        RangeDate();
    }

    private void Date()
    {
        var france = new CultureInfo("fr-FR");
        var franceInfo = france.DateTimeFormat;
        var dateinfo = new DateTimeFormatInfo();
        
        var dayofweek = 
            LocalizationManager.CurrentLanguage == Localized.Thai ? dayOfWeekThai[(int)currentDate.DayOfWeek] :
            LocalizationManager.CurrentLanguage == Localized.English ? currentDate.DayOfWeek.ToString().Substring(0,3) :
            LocalizationManager.CurrentLanguage == Localized.France ? franceInfo.GetDayName(currentDate.DayOfWeek).Substring(0,3) : string.Empty;
        var day = currentDate.Day.ToString();
        var month =             
            LocalizationManager.CurrentLanguage == Localized.Thai ? monthNameThai[currentDate.Month] :
            LocalizationManager.CurrentLanguage == Localized.English ? dateinfo.GetMonthName(currentDate.Month) :
            LocalizationManager.CurrentLanguage == Localized.France ? franceInfo.GetMonthName(currentDate.Month) : string.Empty;
        var year =             
            LocalizationManager.CurrentLanguage == Localized.Thai ? (currentDate.Year + 543).ToString() :
            LocalizationManager.CurrentLanguage == Localized.English ? currentDate.Year.ToString() :
            LocalizationManager.CurrentLanguage == Localized.France ? currentDate.Year.ToString() : string.Empty;

        view.DateTimeText.text = $"{dayofweek} {day} {month} {year}";
    }

    private void RangeDate()
    {
        firstDate = UserDataApp.History.Keys.Min();
        view.RangeDateText.text = $"{firstDate.Day}/{firstDate.Month}/{firstDate.Year} - {currentDate.Day}/{currentDate.Month}/{currentDate.Year}";
    }

    private void Distance()
    {
        view.CurrentFallCountText.text = currentHistory.fallCount.ToString();
        view.SumFallCountText.text = currentHistory.sumfallCount.ToString();
        view.StepCountText.text = currentHistory.footStep.ToString();
        view.DistanceText.text = currentHistory.distance;
    }

    private void Time()
    {
        long milsec = long.Parse(currentHistory.timeStamp);
        var timestamp = TimeSpan.FromSeconds(milsec);
        
        view.HourText.text = $"{timestamp.Hours:00}";
        view.MinuteText.text = $"{timestamp.Minutes:00}";
        view.SecondText.text = $"{timestamp.Seconds:00}";
    }

    private void CircleUi()
    {
        view.CircleSlider.SliderValue = 0;

        tween = DOVirtual.Float(0, 1, 2f, x =>
        {
            view.CircleSlider.SliderValue = x;
        }).OnComplete(() =>
        {
            view.CircleImage.raycastTarget = true;
        }).SetEase(Ease.InOutCubic);
    }
    
    private void OnDestroy()
    {
        databaseReference.Child(UserDataApp.CaneId).Child("history").ValueChanged -= UpdateCurrentData;
        tween.Kill();
    }
}