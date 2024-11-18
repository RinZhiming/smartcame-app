using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InternalSceneAppManager : MonoBehaviour
{
    [SerializeField] private InternalButtonEvent[] eventsList;

    private void Awake()
    {
        if(eventsList.Length > 0)
        {
            foreach (var e in eventsList)
            {
                e.Button.onClick.AddListener(() =>
                {
                    e.Button.transform.root.GetComponent<CanvaUiController>().FadeOut(() => e.TargetGroup.FadeIn());
                });
            }
        }
    }

    private void OnDestroy()
    {
        if (eventsList.Length > 0)
        {
            foreach (var e in eventsList)
            {
                e.Button.onClick.RemoveAllListeners();
            }
        }
    }
}

[System.Serializable]
public class InternalButtonEvent
{
    [SerializeField] private Button button;
    [SerializeField] private CanvaUiController targetGroup;
    public CanvaUiController TargetGroup { get => targetGroup; set { targetGroup = value;} }
    public Button Button { get => button; set {  button = value;} }
}
