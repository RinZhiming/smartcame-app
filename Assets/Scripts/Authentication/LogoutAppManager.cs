using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using User;

public class LogoutAppManager : MonoBehaviour
{
    [SerializeField] private Button logoutButton;
    [SerializeField] private string sceneName;
    private FirebaseAuth authentication;

    private void Awake()
    {
        logoutButton.onClick.AddListener(LogOut);
    }

    private void OnDestroy()
    {
        logoutButton.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        authentication = FirebaseAuth.DefaultInstance;
    }

    private void LogOut()
    {
        authentication.SignOut();
        logoutButton.transform.root.GetComponent<CanvaUiController>().FadeOut(() =>
        {
            SetUser(string.Empty, string.Empty, string.Empty);
            SceneManager.LoadScene(sceneName);
        });
    }
    
    private void SetUser(string email, string password, string id)
    {
        var user = new AppUserData { email = email, password = password };
        var userjson = JsonUtility.ToJson(user);
        File.WriteAllText(Application.persistentDataPath + "/AppUserData.json", userjson);
    }
}
