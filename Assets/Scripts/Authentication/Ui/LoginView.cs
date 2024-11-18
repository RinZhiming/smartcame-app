using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginView : MonoBehaviour
{
    [SerializeField] private InputField emailInput, passwordInput;
    [SerializeField] private Button loginButton;
    [SerializeField] private GameObject errorText;

    public InputField EmailInput { get => emailInput; set => emailInput = value; }
    public InputField PasswordInput { get => passwordInput; set => passwordInput = value; }
    public Button LoginButton { get => loginButton; set => loginButton = value; }
    public GameObject ErrorText { get => errorText; set => errorText = value; }
}
