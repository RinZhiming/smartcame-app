using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatientSettingView : MonoBehaviour
{
    [SerializeField] private Text nameText, birthdayText, sexText, heightText, weightText, emailText, phoneText;

    public Text NameText
    {
        get => nameText;
        set => nameText = value;
    }

    public Text BirthdayText
    {
        get => birthdayText;
        set => birthdayText = value;
    }

    public Text SexText
    {
        get => sexText;
        set => sexText = value;
    }

    public Text HeightText
    {
        get => heightText;
        set => heightText = value;
    }

    public Text WeightText
    {
        get => weightText;
        set => weightText = value;
    }

    public Text EmailText
    {
        get => emailText;
        set => emailText = value;
    }

    public Text PhoneText
    {
        get => phoneText;
        set => phoneText = value;
    }
}
