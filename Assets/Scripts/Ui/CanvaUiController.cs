using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvaUiController : MonoBehaviour
{
    [SerializeField] private bool autoStart;
    [SerializeField] private float duration;
    [SerializeField] private Ease easeType;
    private CanvasGroup group;

    public CanvasGroup Group
    {
        get => group;
        set => group = value;
    }

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        group.alpha = 0;
        group.blocksRaycasts = false;
        if (autoStart)
        {
            FadeIn();
        }
    }

    public void FadeIn(params Action[] onComplete)
    {
        group.alpha = 0;

        DOVirtual.Float(group.alpha, 1, duration, x =>
        {
            group.blocksRaycasts = false;
            group.alpha = x;
        }).OnComplete(() =>
        {
            group.blocksRaycasts = true;
            if(onComplete.Length > 0) foreach (Action action in onComplete) action();
        }).SetEase(easeType);
    }

    public void FadeOut(params Action[] onComplete)
    {
        DOVirtual.Float(group.alpha, 0, duration, x =>
        {
            group.blocksRaycasts = false;
            group.alpha = x;
        }).OnComplete(() =>
        {
            group.blocksRaycasts = false;
            if (onComplete.Length > 0) foreach (Action action in onComplete) action();
        }).SetEase(easeType);
    }
}
