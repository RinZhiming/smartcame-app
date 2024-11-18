using System;
using DG.Tweening;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using UnityEngine;
using UnityEngine.UI;
using User;
using System.Text.RegularExpressions;

public class RegisterAppManager : MonoBehaviour
{
    [SerializeField] private RegisterView view;
    [SerializeField] private float duration;
    [SerializeField] private Ease notificationAnimationType;
    private FirebaseAuth authentication;
    private DatabaseReference databaseReference;
    private FirebaseUser user;
    private Dictionary<InputField, Text> errorDatas = new();

    private void Awake()
    {
        view.RegisterButton.onClick.AddListener(OnRegister);

        errorDatas = new()
        {
            {view.EmailInput, view.EmailErrorText},
            {view.PasswordInput, view.PasswordErrorText},
            {view.PasswordConfirmInput, view.ConfirmPasswordErrorText},
            {view.NameInput, view.NameErrorText},
            {view.BirthdayInput, view.BirthdayErrorText},
            {view.HeightInput, view.HeightErrorText},
            {view.WeightInput, view.WeightErrorText},
            {view.PhoneNumberInput, view.PhoneNumberErrorText},
            {view.NameContactText, view.NameContactErrorText},
            {view.PhoneNumberContactText, view.PhoneNumberContactErrorText},
        };

        view.BirthdayInput.onValueChanged.AddListener(BirthDayInputFieldFormatRegex);
    }

    private void OnDestroy()
    {
        view.RegisterButton.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        authentication = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        foreach (var err in errorDatas)
        {
            err.Key.onValueChanged.AddListener(delegate { ValueChange(err.Value); });
        }

        view.NotificationGroup.transform.DOMoveY(-300, 0.1f);
    }

    private void ValueChange(Text errtext)
    {
        ErrorController.SetError(errtext, false);
    }

    private void OnRegister()
    {
        if (view.EmailInput.text.Length > 0
            && view.PasswordInput.text.Length > 7
            && view.PasswordConfirmInput.text.Length > 7
            && view.PasswordInput.text == view.PasswordConfirmInput.text
            && view.NameInput.text.Length > 0
            && view.BirthdayInput.text.Length > 0
            && view.HeightInput.text.Length > 1
            && view.WeightInput.text.Length > 1
            && view.PhoneNumberInput.text.Length == 10
            && view.NameContactText.text.Length > 0
            && view.PhoneNumberContactText.text.Length == 10
            && Regex.IsMatch(view.BirthdayInput.text, @"^\d{2}/\d{2}/\d{4}$"))
        {
            LoadingController.Load(true);
            view.RegisterButton.transform.root.GetComponent<CanvasGroup>().blocksRaycasts = false;

            authentication.CreateUserWithEmailAndPasswordAsync(view.EmailInput.text, view.PasswordInput.text).ContinueWithOnMainThread(result =>
            {
                if (result.IsFaulted)
                {
                    FirebaseException exception = null;

                    foreach (var ex in result.Exception.Flatten().InnerExceptions)
                    {
                        if (ex is FirebaseException)
                        {
                            exception = ex as FirebaseException;
                            break;
                        }
                    }
                    if (exception != null)
                    {
                        var errcode = (AuthError)exception.ErrorCode;
                        if (errcode == AuthError.InvalidEmail)
                        {
                            // Handle invalid email error
                            ErrorController.SetError(view.EmailErrorText, true,
                                LocalizationManager.CurrentLanguage == Localized.Thai ? "อีเมลไม่ถูกต้อง" :
                                LocalizationManager.CurrentLanguage == Localized.English ? "Invalid email" :
                                LocalizationManager.CurrentLanguage == Localized.France ? "Email invalide" : string.Empty);
                        }
                        if (errcode == AuthError.EmailAlreadyInUse)
                        {
                            // Handle email already in use error
                            ErrorController.SetError(view.EmailErrorText, true,
                                LocalizationManager.CurrentLanguage == Localized.Thai ? "อีเมลซ้ำ กรุณาลองอีกครั้ง" :
                                LocalizationManager.CurrentLanguage == Localized.English ? "Email Already In Use" :
                                LocalizationManager.CurrentLanguage == Localized.France ? "Email déjà utilisé" : string.Empty);
                            view.SecondPage.FadeOut(() =>
                            {
                                view.FirstPage.FadeIn();
                            });
                        }
                    }
                    view.RegisterButton.transform.root.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    LoadingController.Load(false);
                    return;
                }

                if (result.IsCanceled)
                {
                    view.RegisterButton.transform.root.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    LoadingController.Load(false);
                    return;
                }

                if (result.IsCompleted)
                {
                    user = authentication.CurrentUser;
                    Debug.Log(user.UserId);
                    Debug.Log("Register Success");
                    SendUserData();
                }
            });
        }
        else
        {
            LoadingController.Load(false);

            foreach (var err in errorDatas)
            {
                if (String.IsNullOrEmpty(err.Key.text)) ErrorController.SetError(err.Value, true,
                    LocalizationManager.CurrentLanguage == Localized.Thai ? "กรุณากรอกข้อมูลให้ครบถ้วน" :
                    LocalizationManager.CurrentLanguage == Localized.English ? "Please fill out this field" :
                    LocalizationManager.CurrentLanguage == Localized.France ? "Merci de remplir ce champ" : string.Empty);
                if ((err.Key == view.PhoneNumberInput || err.Key == view.PhoneNumberContactText) && err.Key.text.Length != 10)
                    ErrorController.SetError(err.Value, true,
                        LocalizationManager.CurrentLanguage == Localized.Thai ? "กรุณาใส่เบอร์โทรศัพท์ให้ถูกต้อง" :
                        LocalizationManager.CurrentLanguage == Localized.English ? "Invalid phone number format" :
                        LocalizationManager.CurrentLanguage == Localized.France ? "Format de numéro de téléphone invalide" : string.Empty);
                if ((err.Key == view.PasswordInput || err.Key == view.PasswordConfirmInput) && err.Key.text.Length < 8)
                {
                    ErrorController.SetError(err.Value, true,
                        LocalizationManager.CurrentLanguage == Localized.Thai ? "คุณกรอกรหัสผ่านไม่ตรงตามเงื่อนไข" :
                        LocalizationManager.CurrentLanguage == Localized.English ? "Invalid password format" :
                        LocalizationManager.CurrentLanguage == Localized.France ? "Format de mot de passe incorrect" : string.Empty);
                    view.SecondPage.FadeOut(() =>
                    {
                        view.FirstPage.FadeIn();
                    });
                }

                if (err.Key == view.BirthdayInput && !Regex.IsMatch(view.BirthdayInput.text, @"^\d{2}/\d{2}/\d{4}$"))
                    ErrorController.SetError(err.Value, true,
                                                LocalizationManager.CurrentLanguage == Localized.Thai ? "กรุณาใส่วันเกิดให้ถูกต้อง" :
                                                LocalizationManager.CurrentLanguage == Localized.English ? "Invalid birthday format" :
                                                LocalizationManager.CurrentLanguage == Localized.France ? "Format de date de naissance invalide" : string.Empty);
            }

            if (view.PasswordInput.text != view.PasswordConfirmInput.text)
            {
                ErrorController.SetError(errorDatas[view.PasswordConfirmInput], true,
                    LocalizationManager.CurrentLanguage == Localized.Thai ? "รหัสผ่านไม่ตรงกัน" :
                        LocalizationManager.CurrentLanguage == Localized.English ? "Password do not match" :
                    LocalizationManager.CurrentLanguage == Localized.France ? "Le mot de passe ne correspond pas" : string.Empty);
                view.SecondPage.FadeOut(() =>
                {
                    view.FirstPage.FadeIn();
                });
            }
        }
    }

    private void SendUserData()
    {
        var newuser = new UserData
        {
            name = view.NameInput.text,
            birthday = view.BirthdayInput.text,
            sex = view.SexDropdown.options[view.SexDropdown.value].text,
            height = int.Parse(view.HeightInput.text),
            width = int.Parse(view.WeightInput.text),
            phoneNumber = view.PhoneNumberInput.text,
            contactName = view.NameContactText.text,
            contactPhoneNumber = view.PhoneNumberContactText.text,
            userCaneId = String.Empty,
        };

        var userjson = JsonUtility.ToJson(newuser);

        databaseReference.Child(user.UserId).Child("userdata").SetRawJsonValueAsync(userjson).ContinueWithOnMainThread(result =>
        {
            Debug.Log(user.UserId);
            if (result.IsCanceled)
            {
                view.RegisterButton.transform.root.GetComponent<CanvasGroup>().blocksRaycasts = true;
                LoadingController.Load(false);
                return;
            }

            if (result.IsFaulted)
            {
                view.RegisterButton.transform.root.GetComponent<CanvasGroup>().blocksRaycasts = true;
                Debug.Log("Database faile");
                LoadingController.Load(false);
                return;
            }

            if (result.IsCompleted)
            {
                CompleteRegister();
                Debug.Log("Database Success");
                LoadingController.Load(false);
            }
        });
    }

    private void CompleteRegister()
    {
        var uiController = view.NotificationContain.GetComponent<CanvaUiController>();
        var canvasGroup = view.RegisterButton.transform.root.GetComponent<CanvasGroup>();
        if (uiController != null)
        {
            uiController.FadeIn(() =>
            {
                view.NotificationGroup.transform.DOMoveY(285, duration).SetEase(notificationAnimationType).OnStart(() =>
                {
                    canvasGroup.blocksRaycasts = false;
                    view.NotificationContain.GetComponent<CanvasGroup>().blocksRaycasts = false;
                }).OnComplete(() =>
                {
                    view.NotificationContain.GetComponent<CanvasGroup>().blocksRaycasts = true;
                });
            });
        }
    }

    private void BirthDayInputFieldFormatRegex(string s)
    {
        string limitstring = Regex.Replace(s, @"[^\d]", "");

        string formatdate = string.Empty;
        for (int i = 0; i < limitstring.Length; i++)
        {
            if(i == 2 || i == 4)
            {
                formatdate += "/";
            }
            if(i < 8)
            {
                formatdate += limitstring[i];
            }
        }

        view.BirthdayInput.text = formatdate;
        view.BirthdayInput.caretPosition = view.BirthdayInput.text.Length;
    }
}