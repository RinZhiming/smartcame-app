using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoryView : MonoBehaviour
{
    [SerializeField] private GameObject historyPrefab;
    [SerializeField] private Transform historyContain;
    [SerializeField] private CanvaUiController historyCanvaUiController;
    public GameObject HistoryPrefab
    {
        get => historyPrefab;
        set => historyPrefab = value;
    }

    public Transform HistoryContain
    {
        get => historyContain;
        set => historyContain = value;
    }

    public CanvaUiController HistoryCanvaUiController
    {
        get => historyCanvaUiController;
        set => historyCanvaUiController = value;
    }
}
