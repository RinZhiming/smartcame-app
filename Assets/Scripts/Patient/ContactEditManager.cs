using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using User;

public class ContactEditManager : MonoBehaviour
{
    [SerializeField] private ContactEditView view;
    private DatabaseReference databaseReference;
    private FirebaseAuth auth;
    private FirebaseUser user;
    private UserData userData;
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
            {view.NameInput, view.NameErrorText},
            {view.PhoneInput, view.PhoneNumberErrorText},
        };
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
        if (view.NameInput.text != userData.contactName 
            || view.PhoneInput.text != userData.contactPhoneNumber
            && view.NameInput.text.Length > 0
            && view.PhoneInput.text.Length == 10)
        {
            var update = new Dictionary<string, object>
            {
                {"contactName" , view.NameInput.text},
                {"contactPhoneNumber" , view.PhoneInput.text}
            };
            UpdateInformation(update);
        }
        else
        {
            foreach (var err in errorDatas)
            {
                if(String.IsNullOrEmpty(err.Key.text)) 
                    ErrorController.SetError(err.Value, true, 
                        LocalizationManager.CurrentLanguage == Localized.Thai ? "กรุณากรอกข้อมูลให้ครบถ้วน" :
                        LocalizationManager.CurrentLanguage == Localized.English ? "Please fill out this field" : 
                        LocalizationManager.CurrentLanguage == Localized.France ? "Merci de remplir ce champ" : string.Empty);
                if(err.Key == view.PhoneInput && err.Key.text.Length != 10) 
                    ErrorController.SetError(err.Value, true, 
                        LocalizationManager.CurrentLanguage == Localized.Thai ? "กรุณาใส่เบอร์โทรศัพท์ให้ถูกต้อง":
                        LocalizationManager.CurrentLanguage == Localized.English ? "Invalid phone number format" : 
                        LocalizationManager.CurrentLanguage == Localized.France ? "Format de numéro de téléphone invalide" : string.Empty);
            }
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
    
    private void SetInput()
    {
        if (userData != null)
        {
            view.NameInput.text = userData.contactName;
            view.PhoneInput.text = userData.contactPhoneNumber;
        }
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
}
