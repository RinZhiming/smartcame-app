using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContactEditView : MonoBehaviour
{
    [SerializeField] private InputField nameInput, phoneInput;
    [SerializeField] private Button saveButton,confirmButton;
    [SerializeField] private CanvaUiController informationUiCanva, successUiCanva;
    [SerializeField] private string settingScene;
    [SerializeField] private Text nameErrorText, phoneNumberErrorText;

    public Text NameErrorText
    {
        get => nameErrorText;
        set => nameErrorText = value;
    }

    public Text PhoneNumberErrorText
    {
        get => phoneNumberErrorText;
        set => phoneNumberErrorText = value;
    }

    public Button ConfirmButton
    {
        get => confirmButton;
        set => confirmButton = value;
    }

    public string SettingScene
    {
        get => settingScene;
        set => settingScene = value;
    }

    public InputField NameInput
    {
        get => nameInput;
        set => nameInput = value;
    }

    public InputField PhoneInput
    {
        get => phoneInput;
        set => phoneInput = value;
    }

    public Button SaveButton
    {
        get => saveButton;
        set => saveButton = value;
    }

    public CanvaUiController InformationUiCanva
    {
        get => informationUiCanva;
        set => informationUiCanva = value;
    }

    public CanvaUiController SuccessUiCanva
    {
        get => successUiCanva;
        set => successUiCanva = value;
    }

}
