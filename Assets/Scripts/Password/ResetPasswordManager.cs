using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using User;

public class ResetPasswordManager : MonoBehaviour
{
    [SerializeField] private ResetPasswordView view;
    private FirebaseAuth auth;
    private FirebaseUser user;
    private string currentPassword;
    private Dictionary<InputField, Text> errorDatas = new();
    
    private void Awake()
    {
        view.SaveButton.onClick.AddListener(SaveButton);
        view.ConfirmButton.onClick.AddListener(() =>
        {
            view.ConfirmCanvasui.FadeOut(() =>
            {
                view.HeaderCanva.blocksRaycasts = true;
                SceneManager.LoadScene(view.SettingScene);
            });
        });

        errorDatas = new()
        {
            {view.CurrentPasswordInput, view.CurrentPasswordErrorText},
            {view.NewPasswordInput, view.NewPasswordErrorText},
            {view.CNewPasswordInput, view.ConfirmNewPasswordErrorText},
        };
    }

    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser;
        
        foreach (var err in errorDatas)
        {
            err.Key.onValueChanged.AddListener(delegate { ValueChange(err.Value); });
        }
        
        var rawdata = File.ReadAllText(Application.persistentDataPath + "/AppUserData.json");
        var userdata = JsonUtility.FromJson<AppUserData>(rawdata);
        currentPassword = userdata.password;
    }
    
    private void ValueChange(Text errtext)
    {
        ErrorController.SetError(errtext, false);
    }

    private void SaveButton()
    {
        if (view.CurrentPasswordInput.text.Length > 0
            && view.NewPasswordInput.text.Length > 7
            && view.CNewPasswordInput.text.Length > 7
            && view.NewPasswordInput.text == view.CNewPasswordInput.text
            && currentPassword == view.CurrentPasswordInput.text)
        {
            UpdatePassword();
        }
        else
        {
            foreach (var err in errorDatas)
            {
                if(String.IsNullOrEmpty(err.Key.text)) ErrorController.SetError(err.Value, true, 
                    LocalizationManager.CurrentLanguage == Localized.Thai ? "กรุณากรอกข้อมูลให้ครบถ้วน" :
                    LocalizationManager.CurrentLanguage == Localized.English ? "Please fill out this field" :
                    LocalizationManager.CurrentLanguage == Localized.France ? "Merci de remplir ce champ" : string.Empty);
                if((err.Key == view.NewPasswordInput || err.Key == view.CNewPasswordInput || err.Key == view.CurrentPasswordInput) && err.Key.text.Length < 8)
                    ErrorController.SetError(err.Value, true, 
                        LocalizationManager.CurrentLanguage == Localized.Thai ? "คุณกรอกรหัสผ่านไม่ตรงตามเงื่อนไข" :
                        LocalizationManager.CurrentLanguage == Localized.English ? "Invalid password format" :
                        LocalizationManager.CurrentLanguage == Localized.France ? "Format de mot de passe incorrect" : string.Empty);
            }
            if (currentPassword != view.CurrentPasswordInput.text)
            {
                ErrorController.SetError(errorDatas[view.CurrentPasswordInput], true, 
                    LocalizationManager.CurrentLanguage == Localized.Thai ? "รหัสผ่านเดิมไม่ถูกต้อง" :
                    LocalizationManager.CurrentLanguage == Localized.English ? "Current Password is not correct" :
                    LocalizationManager.CurrentLanguage == Localized.France ? "Le mot de passe n'est pas correct" : string.Empty);
            }

            if (view.NewPasswordInput.text != view.CNewPasswordInput.text)
            {
                ErrorController.SetError(errorDatas[view.CNewPasswordInput], true,
                    LocalizationManager.CurrentLanguage == Localized.Thai ?"รหัสผ่านไม่ตรงกัน" :
                    LocalizationManager.CurrentLanguage == Localized.English ? "Password do not match" :
                    LocalizationManager.CurrentLanguage == Localized.France ? "Le mot de passe ne correspond pas" : string.Empty);
            }
        }
    }

    private void UpdatePassword()
    {
        LoadingController.Load(true);
        view.HeaderCanva.blocksRaycasts = false;
        Debug.Log("OnUpdate");
        user.UpdatePasswordAsync(view.CNewPasswordInput.text).ContinueWithOnMainThread(result =>
        {
            if (result.IsFaulted)
            {
                LoadingController.Load(false);
                Debug.Log("Fail to reset");
                return;
            }

            if (result.IsCompleted)
            {
                LoadingController.Load(false);
                view.ResetCanvasUi.FadeOut(() => view.ConfirmCanvasui.FadeIn());
                var rawdata = File.ReadAllText(Application.persistentDataPath + "/AppUserData.json");
                var userdata = JsonUtility.FromJson<AppUserData>(rawdata);
                var newuser = new AppUserData{email = userdata.email, password = view.CNewPasswordInput.text, caneId = userdata.caneId};
                var newdatauser = JsonUtility.ToJson(newuser);
                
                File.WriteAllText(Application.persistentDataPath + "/AppUserData.json", newdatauser);
            }
        });
    }
}
