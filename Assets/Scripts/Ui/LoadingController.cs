using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour
{
    [SerializeField] private CanvaUiController canvasController;
    [SerializeField] private Image loadingImage;
    [SerializeField] private bool isLoading = false;
    private static LoadingController instance;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(canvasController.gameObject);
        loadingImage.fillAmount = 0;
    }

    public static void Load(bool load)
    {
        instance.isLoading = load;
        
        if (load)
        {
            instance.canvasController.FadeIn();
            instance.StartCoroutine(instance.Loading());
        }
        else
        {
            instance.canvasController.FadeOut();
            instance.StopCoroutine(nameof(instance.Loading));
        }
    }

    private IEnumerator Loading()
    {
        while (isLoading)
        {
            loadingImage.fillClockwise = true;

            yield return new WaitForEndOfFrame();

            DOVirtual.Float(0, 1, 1, v => loadingImage.fillAmount = v);

            yield return new WaitForSeconds(1);

            loadingImage.fillClockwise = false;

            yield return new WaitForEndOfFrame();

            DOVirtual.Float(1, 0, 1, v => loadingImage.fillAmount = v);

            yield return new WaitForSeconds(1);
        }

        yield return null;
        
        
    }

}