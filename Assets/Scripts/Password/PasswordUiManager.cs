using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasswordUiManager : MonoBehaviour
{
    [SerializeField] private InputField inputTarget;
    [SerializeField] private Button passwordButton;
    [SerializeField] private Sprite close, open;

    private void Awake()
    {
        passwordButton.onClick.AddListener(() =>
        {
            passwordButton.image.sprite = inputTarget.contentType == InputField.ContentType.Password ? open : close;
            inputTarget.contentType = inputTarget.contentType == InputField.ContentType.Password ? InputField.ContentType.Standard : InputField.ContentType.Password;
            inputTarget.ForceLabelUpdate();
        });
    }

    private void Start()
    {
        passwordButton.image.sprite = close;
    }
}