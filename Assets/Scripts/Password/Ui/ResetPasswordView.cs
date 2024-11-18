using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetPasswordView : MonoBehaviour
{
    [SerializeField] private InputField currentPasswordInput, newPasswordInput, cNewPasswordInput;
    [SerializeField] private Button saveButton, confirmButton;
    [SerializeField] private CanvaUiController resetCanvasUi, confirmCanvasui;
    [SerializeField] private CanvasGroup headerCanva;
    [SerializeField] private string settingScene;
    [SerializeField] private Text currentPasswordErrorText, newPasswordErrorText, confirmNewPasswordErrorText;

    public Text CurrentPasswordErrorText
    {
        get => currentPasswordErrorText;
        set => currentPasswordErrorText = value;
    }

    public Text NewPasswordErrorText
    {
        get => newPasswordErrorText;
        set => newPasswordErrorText = value;
    }

    public Text ConfirmNewPasswordErrorText
    {
        get => confirmNewPasswordErrorText;
        set => confirmNewPasswordErrorText = value;
    }

    public CanvasGroup HeaderCanva
    {
        get => headerCanva;
        set => headerCanva = value;
    }
    
    public CanvaUiController ResetCanvasUi
    {
        get => resetCanvasUi;
        set => resetCanvasUi = value;
    }

    public CanvaUiController ConfirmCanvasui
    {
        get => confirmCanvasui;
        set => confirmCanvasui = value;
    }

    public string SettingScene
    {
        get => settingScene;
        set => settingScene = value;
    }
    
    public InputField CurrentPasswordInput
    {
        get => currentPasswordInput;
        set => currentPasswordInput = value;
    }

    public InputField NewPasswordInput
    {
        get => newPasswordInput;
        set => newPasswordInput = value;
    }

    public InputField CNewPasswordInput
    {
        get => cNewPasswordInput;
        set => cNewPasswordInput = value;
    }

    public Button SaveButton
    {
        get => saveButton;
        set => saveButton = value;
    }

    public Button ConfirmButton
    {
        get => confirmButton;
        set => confirmButton = value;
    }
}
