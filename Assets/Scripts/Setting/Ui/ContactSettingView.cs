using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContactSettingView : MonoBehaviour
{
    [SerializeField] private Text nameText, phoneText;

    public Text NameText
    {
        get => nameText;
        set => nameText = value;
    }

    public Text PhoneText
    {
        get => phoneText;
        set => phoneText = value;
    }
}
