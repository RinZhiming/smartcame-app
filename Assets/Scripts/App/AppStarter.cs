using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using User;

public class AppStarter : MonoBehaviour
{
    [SerializeField] private string loginScene, homeScene;
    private FirebaseAuth authentication;
    private DatabaseReference databaseReference;

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        UserDataApp.History.Clear();
        UserDataApp.Email = string.Empty;
        UserDataApp.CaneId = string.Empty;
    }

    private void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        authentication = FirebaseAuth.DefaultInstance;
        
        authentication.SignOut();
        InitAppData();
    }

    private void InitAppData()
    {
        if(!CheckAppData())
        {
            var newuser = new AppUserData { email = string.Empty, password = string.Empty, caneId = string.Empty };
            var newuserjson = JsonUtility.ToJson(newuser);
            File.WriteAllText(Application.persistentDataPath + "/AppUserData.json", newuserjson);
            SceneManager.LoadScene(loginScene);
        }
        else
        {
            if (GetUser().email != string.Empty && GetUser().password != string.Empty) AutoLogin();
            else SceneManager.LoadScene(loginScene);
        }
    }

    private void AutoLogin()
    {
        authentication.SignInWithEmailAndPasswordAsync(GetUser().email, GetUser().password).ContinueWithOnMainThread(
            result =>
            {
                if (result.IsFaulted)
                {
                    SceneManager.LoadScene(loginScene);
                    return;
                }
                if (result.IsCompleted)
                {
                    GetCaneId();
                }
            });
    }

    private void GetCaneId()
    {
        databaseReference.Child(authentication.CurrentUser.UserId).Child("userdata").GetValueAsync()
            .ContinueWithOnMainThread(
                result =>
                {
                    if (result.IsCompleted)
                    {
                        foreach (var data in result.Result.Children)
                        {
                            if (data.Key == "userCaneId")
                            {
                                HistoryManager.GetHistoryData(databaseReference, data.Value.ToString(), () =>
                                {
                                    UserDataApp.CaneId = data.Value.ToString();
                                    SceneManager.LoadScene(homeScene);
                                });
                                Debug.Log(data.Value.ToString());
                            }
                        }
                    }
                });
    }

    private bool CheckAppData()
    {
        return File.Exists(Application.persistentDataPath + "/AppUserData.json");
    }

    private AppUserData GetUser()
    {
        var user = File.ReadAllText(Application.persistentDataPath + "/AppUserData.json");
        return JsonUtility.FromJson<AppUserData>(user);
    }
}