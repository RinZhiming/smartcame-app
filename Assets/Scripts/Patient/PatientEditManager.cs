using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using User;

public class PatientEditManager : MonoBehaviour
{
    [SerializeField] private PatientEditView view;
    private DatabaseReference databaseReference;
    private FirebaseAuth auth;
    private FirebaseUser user;
    private UserData userData;

    private Dictionary<string, int> sexConstraint = new()
    {
        {"หญิง", 0},
        {"ชาย", 1},
    };
    private Dictionary<InputField, Text> errorDatas = new();

    private void Awake()
    {
        view.SaveButton.onClick.AddListener(OnSave);
        view.ConfirmButton.onClick.AddListener(() =>
        {
            view.SuccessUiCanva.FadeOut(() => SceneManager.LoadScene(view.SettingScene));
        });

        errorDatas = new()
        {
            {view.FirstNameInput, view.NameErrorText},
            {view.BirthdayInput, view.BirthdayErrorText},
            {view.HeightInput, view.HeightErrorText},
            {view.WeightInput, view.WeightErrorText},
            {view.PhoneNumberInput, view.PhoneNumberErrorText},
        };

        view.BirthdayInput.onValueChanged.AddListener(BirthDayInputFieldFormatRegex);
    }

    private void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser;
        
        foreach (var err in errorDatas)
        {
            err.Key.onValueChanged.AddListener(delegate { ValueChange(err.Value); });
        }
        
        if(user != null) GetInformation();
    }
    
    private void ValueChange(Text errtext)
    {
        ErrorController.SetError(errtext, false);
    }

    private void OnSave()
    {
        if (view.FirstNameInput.text != userData.contactName 
            || view.HeightInput.text != userData.contactPhoneNumber
            || view.WeightInput.text != userData.width.ToString()
            || view.BirthdayInput.text != userData.birthday
            || view.PhoneNumberInput.text != userData.phoneNumber
            || view.SexDropdown.itemText.text != userData.sex
            && view.FirstNameInput.text.Length > 0
            && view.BirthdayInput.text.Length > 0
            && view.HeightInput.text.Length > 1
            && view.WeightInput.text.Length > 1
            && view.PhoneNumberInput.text.Length == 10 
            && Regex.IsMatch(view.BirthdayInput.text, @"^\d{2}/\d{2}/\d{4}$"))
        {
            var update = new Dictionary<string, object>
            {
                {"name" , view.FirstNameInput.text},
                {"birthday" , view.BirthdayInput.text},
                {"sex" , view.SexDropdown.options[view.SexDropdown.value].text},
                {"height" , int.Parse(view.HeightInput.text)},
                {"width" , int.Parse(view.WeightInput.text)},
                {"phoneNumber" , view.PhoneNumberInput.text},
            };
            
            UpdateInformation(update);
        }
        else
        {
            foreach (var err in errorDatas)
            {
                if (String.IsNullOrEmpty(err.Key.text))
                    ErrorController.SetError(err.Value, true, 
                        LocalizationManager.CurrentLanguage == Localized.Thai ? "กรุณากรอกข้อมูลให้ครบถ้วน" :
                        LocalizationManager.CurrentLanguage == Localized.English ? "Please fill out this field" : 
                        LocalizationManager.CurrentLanguage == Localized.France ? "Merci de remplir ce champ" : string.Empty);
                if (err.Key == view.PhoneNumberInput && err.Key.text.Length != 10) 
                    ErrorController.SetError(err.Value, true, 
                        LocalizationManager.CurrentLanguage == Localized.Thai ? "กรุณาใส่เบอร์โทรศัพท์ให้ถูกต้อง":
                        LocalizationManager.CurrentLanguage == Localized.English ? "Invalid phone number format" : 
                        LocalizationManager.CurrentLanguage == Localized.France ? "Format de numéro de téléphone invalide" : string.Empty);
                if (err.Key == view.BirthdayInput && !Regex.IsMatch(view.BirthdayInput.text, @"^\d{2}/\d{2}/\d{4}$"))
                    ErrorController.SetError(err.Value, true,
                                            LocalizationManager.CurrentLanguage == Localized.Thai ? "กรุณาใส่วันเกิดให้ถูกต้อง" :
                                            LocalizationManager.CurrentLanguage == Localized.English ? "Invalid birthday format" :
                                            LocalizationManager.CurrentLanguage == Localized.France ? "Format de date de naissance invalide" : string.Empty);
            }
        }
    }

    private void SetInput()
    {
        if (userData != null)
        {
            view.FirstNameInput.text = userData.name;
            view.BirthdayInput.text = userData.birthday;
            if (sexConstraint.TryGetValue(userData.sex, out int i))
            {
                view.SexDropdown.value = i;
            }
            view.HeightInput.text = userData.height.ToString();
            view.WeightInput.text = userData.width.ToString();
            view.PhoneNumberInput.text = userData.phoneNumber;
        }
    }
    
    private void UpdateInformation(Dictionary<string, object> data)
    {
        LoadingController.Load(true);
        databaseReference.Child(user.UserId).Child("userdata").UpdateChildrenAsync(data).ContinueWithOnMainThread(
            result =>
            {
                if (result.IsFaulted)
                {
                    LoadingController.Load(false);
                    return;
                }
                if (result.IsCompleted)
                {
                    LoadingController.Load(false);
                    view.InformationUiCanva.FadeOut(() =>
                    {
                        view.SuccessUiCanva.FadeIn();
                    });
                }
            });
    }

    private void GetInformation()
    {
        LoadingController.Load(true);
        databaseReference.Child(user.UserId).Child("userdata").GetValueAsync().ContinueWithOnMainThread(
            result =>
            {
                if (result.IsFaulted)
                {
                    LoadingController.Load(false);
                    return;
                }
                if (result.IsCompleted)
                {
                    LoadingController.Load(false);
                    userData = JsonUtility.FromJson<UserData>(result.Result.GetRawJsonValue());
                    SetInput();
                }
            });
    }


    private void BirthDayInputFieldFormatRegex(string s)
    {
        string limitstring = Regex.Replace(s, @"[^\d]", "");

        string formatdate = string.Empty;
        for (int i = 0; i < limitstring.Length; i++)
        {
            if (i == 2 || i == 4)
            {
                formatdate += "/";
            }
            if (i < 8)
            {
                formatdate += limitstring[i];
            }
        }

        view.BirthdayInput.text = formatdate;
        view.BirthdayInput.caretPosition = view.BirthdayInput.text.Length;
    }
}
