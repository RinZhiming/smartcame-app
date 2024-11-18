using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneAppManager : MonoBehaviour
{
    [SerializeField] private ExternalButtonEvent[] eventsList;

    private void Awake()
    {
        if (eventsList.Length > 0)
        {
            foreach (ExternalButtonEvent e in eventsList)
            {
                e.Button.onClick.AddListener(() =>
                {
                    if(e.CanvasControllerAltLists.Count > 0)
                    {
                        foreach(CanvaUiController c in e.CanvasControllerAltLists)
                        {
                            c.FadeOut();
                        }
                    }
                    e.Button.transform.root.GetComponent<CanvaUiController>().FadeOut(() =>
                    {
                        SceneManager.LoadScene(e.SceneName);
                    });
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
public class ExternalButtonEvent
{
    [SerializeField] private Button button;
    [SerializeField] private string sceneName;
    [SerializeField] private List<CanvaUiController> canvasControllerAltLists = new List<CanvaUiController>();
    public string SceneName { get => sceneName; set { sceneName = value; } }
    public Button Button { get => button; set { button = value; } }
    public List<CanvaUiController> CanvasControllerAltLists { get => canvasControllerAltLists; set => canvasControllerAltLists = value; }
}
