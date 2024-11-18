using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using User;

public class PatientSettingManager : MonoBehaviour
{
    [SerializeField] private PatientSettingView view;
    [SerializeField] private ContactSettingManager contact;
    private FirebaseAuth auth;
    private FirebaseUser user;
    private DatabaseReference databaseReference;
    private UserData userData;
    private AppUserData userDataLocal;

    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        user = auth.CurrentUser;
        var rawdata = File.ReadAllText(Application.persistentDataPath + "/AppUserData.json");
        userDataLocal = JsonUtility.FromJson<AppUserData>(rawdata);
        
        if(user != null) GetUser();
    }

    private void ShowUser()
    {
        view.EmailText.text = (
            LocalizationManager.CurrentLanguage == Localized.Thai ? "อีเมล : " :
            LocalizationManager.CurrentLanguage == Localized.English ? "Email : " :
            LocalizationManager.CurrentLanguage == Localized.France ? "Email : " : string.Empty) + userDataLocal.email;
        view.NameText.text = (
            LocalizationManager.CurrentLanguage == Localized.Thai ? "ชื่อ-นามสกุล : ":
            LocalizationManager.CurrentLanguage == Localized.English ? "Name-Surname : " :
            LocalizationManager.CurrentLanguage == Localized.France ? "Prénom / nom de famille : " : string.Empty) + userData.name;
        view.BirthdayText.text = (
            LocalizationManager.CurrentLanguage == Localized.Thai ? "วันเกิด : ":
            LocalizationManager.CurrentLanguage == Localized.English ? "Date of birth : " :
            LocalizationManager.CurrentLanguage == Localized.France ? "Date de naissance : " : string.Empty) + userData.birthday;
        view.HeightText.text = (
            LocalizationManager.CurrentLanguage == Localized.Thai ? "ส่วนสูง : " :
            LocalizationManager.CurrentLanguage == Localized.English ? "Height : " :
            LocalizationManager.CurrentLanguage == Localized.France ? "Hauteur : " : string.Empty) + userData.height.ToString();
        view.WeightText.text = (
            LocalizationManager.CurrentLanguage == Localized.Thai ? "น้ำหนัก : " :
            LocalizationManager.CurrentLanguage == Localized.English ? "Weight : " :
            LocalizationManager.CurrentLanguage == Localized.France ? "Poids : " : string.Empty) + userData.width.ToString();
        view.SexText.text = (
            LocalizationManager.CurrentLanguage == Localized.Thai ? "เพศ : ":
            LocalizationManager.CurrentLanguage == Localized.English ? "Gender : ":
            LocalizationManager.CurrentLanguage == Localized.France ? "Genre : " :string.Empty) + userData.sex;
        view.PhoneText.text = (
            LocalizationManager.CurrentLanguage == Localized.Thai ? "เบอร์โทรศัพท์ : ":
            LocalizationManager.CurrentLanguage == Localized.English ? "Phone number : " :
            LocalizationManager.CurrentLanguage == Localized.France ? "Numéro de téléphone : " : string.Empty) + userData.phoneNumber;
    }

    private void GetUser()
    {
        LoadingController.Load(true);
        databaseReference.Child(user.UserId).Child("userdata").GetValueAsync().ContinueWithOnMainThread(result =>
        {
            if (result.IsFaulted)
            {
                Debug.Log("Error Patient Setting.");
                LoadingController.Load(false);
                return;
            }
            if (result.IsCompleted)
            {
                LoadingController.Load(false);
                userData = JsonUtility.FromJson<UserData>(result.Result.GetRawJsonValue());
                ShowUser();
                contact.GetUser();
            }
        });
    }
}
