using System;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using User;

public class CaneConnectManager : MonoBehaviour
{
    [SerializeField] private CaneConnectView view;
    private DatabaseReference databaseReference;
    private FirebaseAuth auth;
    private FirebaseUser user;
    
    private void Awake()
    {
        view.ConnectButton.onClick.AddListener(OnConnect);
        view.DisconnectButton.onClick.AddListener(DisConnect);
        view.CaneIdButton.onClick.AddListener(OnSetId);
        view.InputId.onValueChanged.AddListener(v =>
        {
            view.TextError.gameObject.SetActive(false);
        });
        
        if (UserDataApp.CaneId != String.Empty)
        {
            view.InputId.text = UserDataApp.CaneId;
            view.ConnectStatusUi.sprite = view.ConnectSprite;
            view.ConnectStatusText.text =                    
                LocalizationManager.CurrentLanguage == Localized.Thai ? "เชื่อมต่อแล้ว" : 
                LocalizationManager.CurrentLanguage == Localized.English ? "Connected" :
                LocalizationManager.CurrentLanguage == Localized.France ? "Connecté" : string.Empty;
            view.Disconnect.gameObject.SetActive(true);
            view.Connect.gameObject.SetActive(false);
            view.InputId.interactable = false;
        }
        else
        {
            view.ConnectStatusUi.sprite = view.DisconnectSprite;
            view.ConnectStatusText.text =             
                LocalizationManager.CurrentLanguage == Localized.Thai ? "ยังไม่เชื่อมต่อ" : 
                LocalizationManager.CurrentLanguage == Localized.English ? "Not Connect" :
                LocalizationManager.CurrentLanguage == Localized.France ? "Pas connecte" : string.Empty;
            view.Disconnect.gameObject.SetActive(false);
            view.Connect.gameObject.SetActive(true);
            view.InputId.interactable = true;
        }
    }

    private void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser;
    }
    
    private void DisConnect()
    {
        UserDataApp.History.Clear();
        view.Disconnect.gameObject.SetActive(false);
        view.Connect.gameObject.SetActive(true);
        view.ConnectStatusUi.sprite = view.DisconnectSprite;
        view.ConnectStatusText.text = 
            LocalizationManager.CurrentLanguage == Localized.Thai ? "ยังไม่เชื่อมต่อ" : 
            LocalizationManager.CurrentLanguage == Localized.English ? "Not Connect" :
            LocalizationManager.CurrentLanguage == Localized.France ? "Pas connecte" : string.Empty;
        //UpdateCaneConnected(view.InputId.text, false);
        view.InputId.interactable = true;
        view.InputId.text = string.Empty;
        SetCaneId(string.Empty);
    }

    private void OnSetId()
    {
        view.SettingCanvas.blocksRaycasts = false;
        view.CaneIdCanvas.FadeIn();
    }
    
    private void OnConnect()
    {
        if (view.InputId.text.Length > 0)
        {
            LoadingController.Load(true);
            DOVirtual.DelayedCall(0.5f, Connected);
        }
        else
        {
            view.CaneIdCanvas.FadeOut(() =>
            {
                view.SettingCanvas.blocksRaycasts = true;
            });
        }
    }

    private void SetCaneId(string id)
    {
        var userjson = File.ReadAllText(Application.persistentDataPath + "/AppUserData.json");
        var user = JsonUtility.FromJson<AppUserData>(userjson);
        
        var newuser = new AppUserData{email = user.email, password = user.password, caneId = id};
        var newdatauser = JsonUtility.ToJson(newuser);
        
        File.WriteAllText(Application.persistentDataPath + "/AppUserData.json", newdatauser);
        
        UpdateUserCaneId(id);
        Debug.Log(UserDataApp.CaneId);
    }

    private void Connected()
    {
        databaseReference.Child(view.InputId.text).Child("information").GetValueAsync().ContinueWithOnMainThread(
            result =>
            {
                if (result.IsFaulted)
                {
                    Debug.Log("Fail to Connect");
                    LoadingController.Load(false);
                    Error(LocalizationManager.CurrentLanguage == Localized.Thai ? "เกิดข้อผิดพลาดขณะเชื่อมต่อ กรุณาลองใหม่อีกครั้ง" : 
                        LocalizationManager.CurrentLanguage == Localized.English ? "Error while connecting. Please try again." :
                        LocalizationManager.CurrentLanguage == Localized.France ? "erreur lors de la connexion. Veuillez réessayer." : string.Empty,true);
                    return;
                }
                    
                if (result.IsCompleted)
                {
                    Debug.Log("Complete Connect");
                    var rawdata = result.Result;
                    var datainformation = JsonUtility.FromJson<CaneInformationData>(rawdata.GetRawJsonValue());
                    
                    if (rawdata.Exists)
                    {
                        if (datainformation.caneID == view.InputId.text)
                        {
                            UserDataApp.CaneId = datainformation.caneID;
                            GetHistoryData();
                            SetCaneId(datainformation.caneID);
                            view.Disconnect.gameObject.SetActive(true);
                            view.Connect.gameObject.SetActive(false);
                            //UpdateCaneConnected(view.InputId.text,true);
                            //if (!datainformation.hasConnected)
                            //{
                            //}
                            //else
                            //{
                            //    SetCaneId(string.Empty);
                            //    LoadingController.Load(false);
                            //    Error(LocalizationManager.CurrentLanguage == Localized.Thai ? "ID นี้ถูกเชื่อมต่อแล้ว" : 
                            //        LocalizationManager.CurrentLanguage == Localized.English ? "This ID is already connected." :
                            //        LocalizationManager.CurrentLanguage == Localized.France ? "Cet identifiant est déjà connecté." : string.Empty, true);
                            //}
                        }
                        else
                        {
                            LoadingController.Load(false);
                            Error(LocalizationManager.CurrentLanguage == Localized.Thai ? "เกิดข้อผิดพลาดขณะเชื่อมต่อ กรุณาลองใหม่อีกครั้ง" : 
                                LocalizationManager.CurrentLanguage == Localized.English ? "Error while connecting. Please try again." :
                                LocalizationManager.CurrentLanguage == Localized.France ? "erreur lors de la connexion. Veuillez réessayer." : string.Empty,true);
                        }
                    }
                    else
                    {
                        Debug.Log("Fail to Connect");
                        LoadingController.Load(false);
                        Error(LocalizationManager.CurrentLanguage == Localized.Thai ? "เกิดข้อผิดพลาดขณะเชื่อมต่อ กรุณาลองใหม่อีกครั้ง" : 
                            LocalizationManager.CurrentLanguage == Localized.English ? "Error while connecting. Please try again." :
                            LocalizationManager.CurrentLanguage == Localized.France ? "erreur lors de la connexion. Veuillez réessayer." : string.Empty, true);
                    }
                }
            });
    }

    private void GetHistoryData()
    {
        HistoryManager.GetHistoryData(databaseReference, view.InputId.text, () =>
        {
            LoadingController.Load(false);
            view.CaneIdCanvas.FadeOut();
            view.InputId.interactable = false;
            view.CaneCompleteCanvas.FadeIn(() =>
            {
                DOVirtual.DelayedCall(1, () => view.CaneCompleteCanvas.FadeOut());
                view.SettingCanvas.blocksRaycasts = true;
                    
                view.ConnectStatusUi.sprite = view.ConnectSprite;
                view.ConnectStatusText.text =             
                    LocalizationManager.CurrentLanguage == Localized.Thai ? "เชื่อมต่อแล้ว" : 
                    LocalizationManager.CurrentLanguage == Localized.English ? "Connected" :
                    LocalizationManager.CurrentLanguage == Localized.France ? "Connecté" : string.Empty;
                Error(LocalizationManager.CurrentLanguage == Localized.Thai ? "เกิดข้อผิดพลาดขณะเชื่อมต่อ กรุณาลองใหม่อีกครั้ง" : 
                    LocalizationManager.CurrentLanguage == Localized.English ? "Error while connecting. Please try again." :
                    LocalizationManager.CurrentLanguage == Localized.France ? "erreur lors de la connexion. Veuillez réessayer." : string.Empty,false);
            });
        }, () =>
        {
            SetCaneId(string.Empty);
            LoadingController.Load(false);
            Error(LocalizationManager.CurrentLanguage == Localized.Thai ? "เกิดข้อผิดพลาดขณะเชื่อมต่อ กรุณาลองใหม่อีกครั้ง" : 
                LocalizationManager.CurrentLanguage == Localized.English ? "Error while connecting. Please try again." :
                LocalizationManager.CurrentLanguage == Localized.France ? "erreur lors de la connexion. Veuillez réessayer." : string.Empty,true);
        });
    }

    private void UpdateCaneConnected(string caneId,bool hasConnected)
    {
        databaseReference.Child(caneId).Child("information").UpdateChildrenAsync(
            new Dictionary<string, object>
            {
                { "hasConnected", hasConnected}
            });
    }

    private void UpdateUserCaneId(string caneid)
    {
        Debug.Log(UserDataApp.CaneId);
        databaseReference.Child(user.UserId).Child("userdata").UpdateChildrenAsync(new Dictionary<string, object>()
        {
            {"userCaneId" , caneid}
        });

        UserDataApp.CaneId = caneid;
    }

    private void Error(string text, bool showError)
    {
        view.TextError.gameObject.SetActive(showError);
        view.TextError.text = text;
    }
    
    public void DeactiveCanva()
    {
        view.CaneIdCanvas.FadeOut(() => view.SettingCanvas.blocksRaycasts = true);
    }
}