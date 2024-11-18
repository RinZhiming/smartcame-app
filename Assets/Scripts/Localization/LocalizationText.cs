using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationText : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private string thai, english, france;

    private void Update()
    {
        TextLanguage();
    }

    private void TextLanguage()
    {
        switch (LocalizationManager.CurrentLanguage)
        {
            case Localized.Thai:
                text.text = thai;
                break;
            case Localized.English:
                text.text = english;
                break;
            case Localized.France:
                text.text = france;
                break;
        }
    }
}