using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterView : MonoBehaviour
{
    [SerializeField] private Button registerButton;
    [SerializeField] private InputField emailInput, passwordInput, passwordConfirmInput, nameInput, birthdayInput, heightInput, weightInput, phoneNumberInput, nameContactText, phoneNumberContactText;
    [SerializeField] private Dropdown sexDropdown;
    [SerializeField] private GameObject notificationContain, notificationGroup;
    [SerializeField] private CanvaUiController firstPage, secondPage;
    [SerializeField] private Text emailErrorText,
        passwordErrorText,
        confirmPasswordErrorText,
        nameErrorText,
        birthdayErrorText,
        heightErrorText,
        weightErrorText,
        phoneNumberErrorText,
        nameContactErrorText,
        phoneNumberContactErrorText;

    public CanvaUiController FirstPage
    {
        get => firstPage;
        set => firstPage = value;
    }

    public CanvaUiController SecondPage
    {
        get => secondPage;
        set => secondPage = value;
    }
    
    public Text EmailErrorText
    {
        get => emailErrorText;
        set => emailErrorText = value;
    }

    public Text PasswordErrorText
    {
        get => passwordErrorText;
        set => passwordErrorText = value;
    }

    public Text ConfirmPasswordErrorText
    {
        get => confirmPasswordErrorText;
        set => confirmPasswordErrorText = value;
    }

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

    public Text NameContactErrorText
    {
        get => nameContactErrorText;
        set => nameContactErrorText = value;
    }

    public Text PhoneNumberContactErrorText
    {
        get => phoneNumberContactErrorText;
        set => phoneNumberContactErrorText = value;
    }

    public GameObject NotificationContain { get =>  notificationContain; set => notificationContain = value; }
    public GameObject NotificationGroup { get => notificationGroup; set => notificationGroup = value; }
    public InputField EmailInput { get => emailInput; set => emailInput = value; }
    public InputField PasswordInput { get => passwordInput; set => passwordInput = value; }
    public InputField PasswordConfirmInput { get => passwordConfirmInput; set => passwordConfirmInput = value; }
    public InputField NameInput { get => nameInput; set => nameInput = value; }
    public InputField BirthdayInput { get => birthdayInput; set => birthdayInput = value; }
    public Dropdown SexDropdown { get => sexDropdown; set => sexDropdown = value; }
    public InputField HeightInput { get => heightInput; set => heightInput = value; }
    public InputField WeightInput { get => weightInput; set => weightInput = value; }
    public InputField PhoneNumberInput { get => phoneNumberInput; set => phoneNumberInput = value; }
    public InputField NameContactText { get => nameContactText; set => nameContactText = value; }
    public InputField PhoneNumberContactText { get => phoneNumberContactText; set => phoneNumberContactText = value; }
    public Button RegisterButton { get => registerButton; set => registerButton = value; }
}
