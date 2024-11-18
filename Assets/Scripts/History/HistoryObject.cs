using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoryObject : MonoBehaviour
{
    [SerializeField] private Text dateTime, timeText, fallCountText, walkCountText, meterCountText;

    public Text DateTime { get =>  dateTime; set => dateTime = value;  }
    public Text TimeText { get => timeText; set => timeText = value; }
    public Text FallCountText { get => fallCountText; set => fallCountText = value; }
    public Text WalkCountText { get => walkCountText; set => walkCountText = value; }
    public Text MeterCountText { get => meterCountText; set => meterCountText = value; }
}
