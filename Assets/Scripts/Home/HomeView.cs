using DTT.UI.ProceduralUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeView : MonoBehaviour
{
    [SerializeField] private CircularSlider circleSlider;
    [SerializeField] private RoundedImage circleImage;
    [SerializeField] private Text 
        hourText, 
        minuteText, 
        secondText,
        dateTimeText,
        currentFallCountText,
        stepCountText,
        distanceText,
        sumFallCountText,
        rangeDateText;

    public Text HourText
    {
        get => hourText;
        set => hourText = value;
    }

    public Text MinuteText
    {
        get => minuteText;
        set => minuteText = value;
    }

    public Text SecondText
    {
        get => secondText;
        set => secondText = value;
    }

    public Text DateTimeText
    {
        get => dateTimeText;
        set => dateTimeText = value;
    }

    public Text CurrentFallCountText
    {
        get => currentFallCountText;
        set => currentFallCountText = value;
    }

    public Text StepCountText
    {
        get => stepCountText;
        set => stepCountText = value;
    }

    public Text DistanceText
    {
        get => distanceText;
        set => distanceText = value;
    }

    public Text SumFallCountText
    {
        get => sumFallCountText;
        set => sumFallCountText = value;
    }

    public Text RangeDateText
    {
        get => rangeDateText;
        set => rangeDateText = value;
    }

    public CircularSlider CircleSlider { get => circleSlider; set => circleSlider = value; }
    public RoundedImage CircleImage { get => circleImage; set => circleImage = value; }
}
