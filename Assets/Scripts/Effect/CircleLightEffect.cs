using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CircleLightEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler 
{
    [SerializeField] private GameObject objEffector;
    [SerializeField] private HomeAppManager homeAppManager;
    [SerializeField] private Ease effectorEase;
    private bool canPlay = true;

    private void Awake()
    {
        canPlay = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        objEffector.transform.DOScale(0.85f, 0.3f);
    }

    private IEnumerator PlayEffect()
    {
        canPlay = false;
        yield return new WaitForSeconds(1);
        canPlay = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (canPlay) StartCoroutine(PlayEffect());
        homeAppManager.HomeUi();
        objEffector.transform.DOScale(1, 0.5f);
    }
}
