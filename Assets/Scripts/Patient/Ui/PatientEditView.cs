using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatientEditView : MonoBehaviour
{
    [SerializeField] private InputField firstNameInput, birthdayInput, heightInput, weightInput, phoneNumberInput;
    [SerializeField] private Dropdown sexDropdown;
    [SerializeField] private Button saveButton, confirmButton;
    [SerializeField] private CanvaUiController informationUiCanva, successUiCanva;
    [SerializeField] private string settingScene;

    [SerializeField]
    private Text nameErrorText, birthdayErrorText, heightErrorText, weightErrorText, phoneNumberErrorText;

    public Text NameErrorText
    {
        get => nameErrorText;
        set => nameErrorText = value;
    }

    public Text BirthdayErrorText
    {
        get => birthdayErrorText;
        set => birthdayErrorText = value;
    }

    public Text HeightErrorText
    {
        get => heightErrorText;
        set => heightErrorText = value;
    }

    public Text WeightErrorText
    {
        get => weightErrorText;
        set => weightErrorText = value;
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

    public Button SaveButton
    {
        get => saveButton;
        set => saveButton = value;
    }

    public InputField FirstNameInput
    {
        get => firstNameInput;
        set => firstNameInput = value;
    }

    public InputField BirthdayInput
    {
        get => birthdayInput;
        set => birthdayInput = value;
    }

    public InputField HeightInput
    {
        get => heightInput;
        set => heightInput = value;
    }

    public InputField WeightInput
    {
        get => weightInput;
        set => weightInput = value;
    }

    public InputField PhoneNumberInput
    {
        get => phoneNumberInput;
        set => phoneNumberInput = value;
    }

    public Dropdown SexDropdown
    {
        get => sexDropdown;
        set => sexDropdown = value;
    }
}
