using Firebase.Auth;
using Firebase.Extensions;
using System.IO;
using Firebase.Database;
using UnityEngine;
using UnityEngine.SceneManagement;
using User;

public class LoginAppManager : MonoBehaviour
{
    [SerializeField] private LoginView view;
    [SerializeField] private string sceneName;
    private FirebaseAuth authentication;
    private DatabaseReference databaseReference;

    private void Awake()
    {
        view.LoginButton.onClick.AddListener(Login);
        view.EmailInput.onValueChanged.AddListener(FocusInput);
        view.PasswordInput.onValueChanged.AddListener(FocusInput);
    }

    private void Start()
    {
        authentication = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void OnDestroy()
    {
        view.LoginButton.onClick.RemoveAllListeners();
        view.EmailInput.onValueChanged.RemoveAllListeners();
        view.PasswordInput.onValueChanged.RemoveAllListeners();
    }

    private void Login()
    {
        if(view.EmailInput.text.Length > 0 && view.PasswordInput.text.Length > 0)
        {
            LoadingController.Load(true);

            authentication.SignInWithEmailAndPasswordAsync(view.EmailInput.text, view.PasswordInput.text).ContinueWithOnMainThread(result =>
            {
                if(result.IsFaulted)
                {
                    view.ErrorText.SetActive(true);
                    LoadingController.Load(false);
                    return;
                }
                if (result.IsCanceled)
                {
                    LoadingController.Load(false);
                    return;
                }
                if (result.IsCompleted)
                {
                    GetCaneId();
                }
            });
        }
    }
    
    private void GetCaneId()
    {
        databaseReference.Child(authentication.CurrentUser.UserId).Child("userdata").GetValueAsync()
            .ContinueWithOnMainThread(
                result =>
                {
                    if (result.IsFaulted)
                    {
                        LoadingController.Load(false);
                        return;
                    }
                    if (result.IsCompleted)
                    {
                        foreach (var data in result.Result.Children)
                        {
                            if (data.Key == "userCaneId")
                            {
                                HistoryManager.GetHistoryData(databaseReference, data.Value.ToString(), () =>
                                {
                                    SetUser(view.EmailInput.text, view.PasswordInput.text, data.Value.ToString());
                                    UserDataApp.CaneId = data.Value.ToString();
                                    LoadingController.Load(false);
                                    SceneManager.LoadScene(sceneName);
                                });
                                Debug.Log(data.Value.ToString());
                            }
                        }
                    }
                });
    }
    
    private void SetUser(string email, string password, string canId)
    {
        var user = new AppUserData { email = email, password = password, caneId = string.Empty};
        var userjson = JsonUtility.ToJson(user);
        File.WriteAllText(Application.persistentDataPath + "/AppUserData.json", userjson);
    }

    private void FocusInput(string i)
    {
        view.ErrorText.SetActive(false);
    }
}
