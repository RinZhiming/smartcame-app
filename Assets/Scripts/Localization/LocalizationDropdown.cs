using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationDropdown : MonoBehaviour
{
    [SerializeField] private Dropdown dropdown;
    [SerializeField] private string[] thai, english, france;
    
    private void Start()
    {
        TextLanguage();
    }

    private void TextLanguage()
    {
        switch (LocalizationManager.CurrentLanguage)
        {
            case Localized.Thai:
                var thaidata = new List<Dropdown.OptionData>();
                
                foreach (var l in thai)
                {
                    thaidata.Add(new Dropdown.OptionData(l));
                }

                dropdown.options = thaidata;
                break;
            case Localized.English:
                var englishdata = new List<Dropdown.OptionData>();
                
                foreach (var l in english)
                {
                    englishdata.Add(new Dropdown.OptionData(l));
                }

                dropdown.options = englishdata;
                break;
            case Localized.France:
                var francedata = new List<Dropdown.OptionData>();
                
                foreach (var l in france)
                {
                    francedata.Add(new Dropdown.OptionData(l));
                }

                dropdown.options = francedata;
                break;
        }
    }
}
