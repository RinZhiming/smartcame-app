using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using User;

public class ContactSettingManager : MonoBehaviour
{
    [SerializeField] private ContactSettingView view;
    private FirebaseAuth auth;
    private FirebaseUser user;
    private DatabaseReference databaseReference;
    private UserData userData;
    
    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        user = auth.CurrentUser;
    }
    
    private void ShowUser()
    {
        view.NameText.text =
            (LocalizationManager.CurrentLanguage == Localized.Thai ? "ชื่อ-นามสกุล : " :
                LocalizationManager.CurrentLanguage == Localized.English ? "Name-Surname : " :
                LocalizationManager.CurrentLanguage == Localized.France ? "Prénom / nom de famille : " : string.Empty) + userData.contactName;
        view.PhoneText.text = 
            (LocalizationManager.CurrentLanguage == Localized.Thai ? "เบอร์โทรศัพท์ : " : 
                LocalizationManager.CurrentLanguage == Localized.English ? "Phone number : " :
                LocalizationManager.CurrentLanguage == Localized.France ? "Numéro de téléphone : " : string.Empty) + userData.contactPhoneNumber;
    }
    
    public void GetUser()
    {
        LoadingController.Load(true);
        databaseReference.Child(user.UserId).Child("userdata").GetValueAsync().ContinueWithOnMainThread(result =>
        {
            if (result.IsFaulted)
            {
                Debug.Log("Error Contact Setting.");
                LoadingController.Load(false);
                return;
            }
            if (result.IsCompleted)
            {
                LoadingController.Load(false);
                userData = JsonUtility.FromJson<UserData>(result.Result.GetRawJsonValue());
                ShowUser();
            }
        });
    }
}
