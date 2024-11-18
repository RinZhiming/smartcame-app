using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorController : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public static void SetError(Text errortext, bool show, string msg = "")
    {
        errortext.text = msg;
        errortext.gameObject.SetActive(show);
    }
}