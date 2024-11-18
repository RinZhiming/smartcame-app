using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationButton : MonoBehaviour
{
    [SerializeField] private Button button;
    private int index;

    private void Awake()
    {
        button.onClick.AddListener(OnCLick);
    }

    private void Start()
    {
        switch (LocalizationManager.CurrentLanguage)
        {
            case Localized.Thai:
                LocalizationManager.SetLanguage(Localized.Thai, button.image);
                index = 0;
                break;
            case Localized.English:
                LocalizationManager.SetLanguage(Localized.English, button.image);
                index = 1;
                break;
            case Localized.France:
                LocalizationManager.SetLanguage(Localized.France, button.image);
                index = 2;
                break;
        }
    }

    private void OnCLick()
    {
        if (index > 1) index = 0;
        else index++;
        
        switch (index)
        {
            case 0:
                LocalizationManager.SetLanguage(Localized.Thai, button.image);
                break;
            case 1:
                LocalizationManager.SetLanguage(Localized.English, button.image);
                break;
            case 2:
                LocalizationManager.SetLanguage(Localized.France, button.image);
                break;
        }
    }
}
