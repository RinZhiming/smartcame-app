using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class BackgroundTouch : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private UnityEvent onClickEvent;
    public void OnPointerDown(PointerEventData eventData)
    {
        onClickEvent?.Invoke();
    }
}
