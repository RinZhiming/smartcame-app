using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Localized
{
    Thai,
    English,
    France
}

public class LocalizationManager : MonoBehaviour
{
    private static LocalizationManager instance;
    [SerializeField] private Sprite thaiSprite, englishSprite, franceSprite;
    private Dictionary<Localized, Sprite> localizedDatas;
    public static Localized CurrentLanguage { get; set; }

    private void Awake()
    {
        instance = this;
        localizedDatas = new()
        {
            {Localized.Thai, thaiSprite},
            {Localized.English, englishSprite},
            {Localized.France, franceSprite},
        };
        
        if(PlayerPrefs.HasKey("Language")) CurrentLanguage = (Localized) PlayerPrefs.GetInt("Language");
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public static void SetLanguage(Localized language, Image image)
    {
        CurrentLanguage = language;
        foreach (var localized in instance.localizedDatas)
        {
            if (localized.Key == CurrentLanguage)
            {
                image.sprite = localized.Value;
                PlayerPrefs.SetInt("Language", (int)language);
                PlayerPrefs.Save();
                break;
            }
        }
    }
}