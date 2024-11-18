using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ForgotPasswordManager : MonoBehaviour
{
    [SerializeField] private ForgotPasswordView view;
    private FirebaseAuth auth;
    private string email;
    private Dictionary<InputField, Text> errorDatas = new();

    private void Awake()
    {
        view.SendEmailButton.onClick.AddListener(() =>
        {
            email = view.EmailInputField.text;
            OnResetPassword(() =>
            {
                view.ResetCanvaUi.FadeOut(() =>
                {
                    view.ResendCanvaUi.FadeIn((() => view.HeaderCanvas.blocksRaycasts = true));
                });
            });
        });
        view.ResendEmailButton.onClick.AddListener(() =>
        {
            OnResetPassword(() => view.HeaderCanvas.blocksRaycasts = true);
        });

        errorDatas = new()
        {
            {view.EmailInputField, view.EmailErrorText}
        };
    }

    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        email = string.Empty;
        
        foreach (var err in errorDatas)
        {
            err.Key.onValueChanged.AddListener(delegate { ValueChange(err.Value); });
        }
    }
    
    private void ValueChange(Text errtext)
    {
        ErrorController.SetError(errtext, false);
    }

    private void OnResetPassword(Action onComplete = null)
    {
        foreach (var err in errorDatas)
        {
            if (String.IsNullOrEmpty(err.Key.text))
            {
                ErrorController.SetError(err.Value, true, "กรุณากรอกข้อมูลให้ครบถ้วน");
                return;
            }
        }
        
        LoadingController.Load(true);
        view.HeaderCanvas.blocksRaycasts = false;
        auth.SendPasswordResetEmailAsync(email).ContinueWithOnMainThread(result =>
        {
            if (result.IsFaulted)
            {
                LoadingController.Load(false);
                return;
            }

            if (result.IsCompleted)
            {
                LoadingController.Load(false);
                onComplete?.Invoke();
            }
        });
        
    }
}
