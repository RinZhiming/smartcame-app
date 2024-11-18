using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularSlider : MonoBehaviour
{
    [SerializeField, Range(0,1)] private float sliderValue;
    [SerializeField] private Image sliderImage, effectImage, innerEffectImage;

    private void Update()
    {
        sliderImage.fillAmount = sliderValue;
        effectImage.fillAmount = sliderValue;
    }

    public float SliderValue { get => sliderValue; set => sliderValue = value; }
    public Image SliderImage { get => sliderImage; set => sliderImage = value; }
    public Image EffectImage { get => effectImage; set => effectImage = value; }
}
