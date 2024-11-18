using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using User;

public class HistoryManager : MonoBehaviour
{
    [SerializeField] private HistoryView view;
    private DatabaseReference databaseReference;
    private string[] dayOfWeekThai = { "อาทิตย์", "จันทร์", "อังคาร", "พุธ", "พฤหัสบดี", "ศุกร์", "เสาร์" };
    private string[] monthNameThai =
    {
        "มกราคม", "กุมภาพันธ์", "มีนาคม", "เมษายน", "พฤษภาคม", "มิถุนายน", "กรกฎาคม", "สิงหาคม", "กันยายน", "ตุลาคม",
        "พฤศจิกายน", "ธันวาคม"
    };

    private void Awake()
    {
    }

    private void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        if(UserDataApp.History.Count > 0)
        {
            AddHistory();
        }
        else
        {
            GetHistoryData(databaseReference, UserDataApp.CaneId, CreateHistory);
        }
    }

    private void AddHistory()
    {
        CreateHistory();
    }

    private void CreateHistory()
    {
        foreach (var history in UserDataApp.History)
        {
            var h = Instantiate(view.HistoryPrefab, view.HistoryContain);
            var hobj = h.GetComponent<HistoryObject>();
            
            var france = new CultureInfo("fr-FR");
            var franceInfo = france.DateTimeFormat;
            var dateinfo = new DateTimeFormatInfo();

            var dayofweek =
                LocalizationManager.CurrentLanguage == Localized.Thai ? dayOfWeekThai[(int)history.Key.DayOfWeek] :
                LocalizationManager.CurrentLanguage == Localized.English ? history.Key.DayOfWeek.ToString().Substring(0, 3) :
                LocalizationManager.CurrentLanguage == Localized.France ? franceInfo.GetDayName(history.Key.DayOfWeek).Substring(0, 3) : string.Empty;
            var day = history.Key.Day.ToString();
            var month =
                LocalizationManager.CurrentLanguage == Localized.Thai ? monthNameThai[history.Key.Month] :
                LocalizationManager.CurrentLanguage == Localized.English ? dateinfo.GetMonthName(history.Key.Month) :
                LocalizationManager.CurrentLanguage == Localized.France ? franceInfo.GetMonthName(history.Key.Month) : string.Empty;
            var year =
                LocalizationManager.CurrentLanguage == Localized.Thai ? (history.Key.Year + 543).ToString() :
                LocalizationManager.CurrentLanguage == Localized.English ? history.Key.Year.ToString() :
                LocalizationManager.CurrentLanguage == Localized.France ? history.Key.Year.ToString() : string.Empty;

            hobj.DateTime.text = $"{dayofweek} {day} {month} {year}";

            long milsec = long.Parse(history.Value.timeStamp);
            var timestamp = TimeSpan.FromSeconds(milsec);
            hobj.TimeText.text = (LocalizationManager.CurrentLanguage == Localized.Thai ? "ระยะเวลาที่ใช้ทั้งหมด " :
                                     LocalizationManager.CurrentLanguage == Localized.English ? "Total time " :
                                     LocalizationManager.CurrentLanguage == Localized.France ? "Temps total " : string.Empty) + 
                                 $"{timestamp.Hours:00}:{timestamp.Minutes:00}:{timestamp.Seconds:00}";
            
            hobj.FallCountText.text = (LocalizationManager.CurrentLanguage == Localized.Thai ? "จำนวนการล้ม " :
                LocalizationManager.CurrentLanguage == Localized.English ? "Number of falls " :
                LocalizationManager.CurrentLanguage == Localized.France ? "Nombre de chutes " : string.Empty)
                                      + history.Value.fallCount + 
                                      (LocalizationManager.CurrentLanguage == Localized.Thai ? " ครั้ง" :
                                          LocalizationManager.CurrentLanguage == Localized.English ? " Times" :
                                          LocalizationManager.CurrentLanguage == Localized.France ? " Fois" : string.Empty);
            
            hobj.WalkCountText.text = (LocalizationManager.CurrentLanguage == Localized.Thai ? "จำนวนก้าว " :
                LocalizationManager.CurrentLanguage == Localized.English ? "Steps " :
                LocalizationManager.CurrentLanguage == Localized.France ? "Pas " : string.Empty)
                                      + history.Value.footStep + 
                                      (LocalizationManager.CurrentLanguage == Localized.Thai ? " ก้าว" :
                                          LocalizationManager.CurrentLanguage == Localized.English ? " steps" :
                                          LocalizationManager.CurrentLanguage == Localized.France ? " Pas" : string.Empty);
            
            hobj.MeterCountText.text = (LocalizationManager.CurrentLanguage == Localized.Thai ? "ระยะทาง " :
                LocalizationManager.CurrentLanguage == Localized.English ? "Distance " :
                LocalizationManager.CurrentLanguage == Localized.France ? "Distance " : string.Empty)
                                       + history.Value.distance + 
                                       (LocalizationManager.CurrentLanguage == Localized.Thai ? " ม" :
                                           LocalizationManager.CurrentLanguage == Localized.English ? " m" :
                                           LocalizationManager.CurrentLanguage == Localized.France ? " m" : string.Empty);
        }
    }

    public static void GetHistoryData(DatabaseReference dbref,string id, Action onComplete = null, Action onFail = null)
    {
        UserDataApp.History.Clear();
        
        dbref.Child(id).Child("history").GetValueAsync().ContinueWithOnMainThread(result =>
        {
            if(result.IsFaulted)
            {
                onFail?.Invoke();
                return;
            }

            if (result.IsCompleted)
            {
                foreach (var childdata in result.Result.Children)
                {
                    var key = childdata.Key;
                    var day = key.Substring(0, 2);
                    var month = key.Substring(2, 2);
                    var year = key.Substring(4, 4);
                    var datetime = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
                    var val = JsonUtility.FromJson<CaneHistoryData>(childdata.GetRawJsonValue());
                    UserDataApp.History.TryAdd(datetime, val);
                }
                onComplete?.Invoke();
            }
        });
    }
}