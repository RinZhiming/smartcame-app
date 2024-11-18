using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeaderInputfieldManager : MonoBehaviour
{
    [SerializeField] private InputField inputfield;
    [SerializeField] private GameObject textinfo;

    private void Awake()
    {
        textinfo.SetActive(false);
        
        inputfield.onValueChanged.AddListener(SetInfo);
    }

    private void SetInfo(string s)
    {
        textinfo.SetActive(!String.IsNullOrEmpty(s));
    }
}