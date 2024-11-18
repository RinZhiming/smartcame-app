using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForgotPasswordView : MonoBehaviour
{
    [SerializeField] private Button sendEmailButton, resendEmailButton;
    [SerializeField] private InputField emailInputField;
    [SerializeField] private CanvaUiController resetCanvaUi, resendCanvaUi;
    [SerializeField] private CanvasGroup headerCanvas;
    [SerializeField] private Text emailErrorText;

    public Text EmailErrorText
    {
        get => emailErrorText;
        set => emailErrorText = value;
    }

    public CanvasGroup HeaderCanvas
    {
        get => headerCanvas;
        set => headerCanvas = value;
    }

    public Button SendEmailButton
    {
        get => sendEmailButton;
        set => sendEmailButton = value;
    }

    public Button ResendEmailButton
    {
        get => resendEmailButton;
        set => resendEmailButton = value;
    }

    public InputField EmailInputField
    {
        get => emailInputField;
        set => emailInputField = value;
    }

    public CanvaUiController ResetCanvaUi
    {
        get => resetCanvaUi;
        set => resetCanvaUi = value;
    }

    public CanvaUiController ResendCanvaUi
    {
        get => resendCanvaUi;
        set => resendCanvaUi = value;
    }

}
