using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SnowArgsUiToolEditor : Editor
{
    private static void SceneDirty(GameObject obj, string name)
    {
        if (!Application.isPlaying)
        {
            Undo.RegisterCreatedObjectUndo(obj, name);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }

    private static void CreateObject(string itemname)
    {
        try
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("SnowUi/" + itemname), Vector3.zero, Quaternion.identity) as GameObject;

            try
            {
                if(Selection.activeGameObject != null)
                {
                    obj.transform.SetParent(Selection.activeGameObject.transform, false);
                }
                obj.name = obj.name.Replace("(Clone)", "").Trim();
                SceneDirty(obj, obj.name);
            }
            catch
            {
                Debug.LogError("Faile to Add gameobject.");
            }

            Selection.activeGameObject = obj;
        }
        catch
        {
            Debug.LogError("Faile to Create gameobject.");
        }
        

    }

    [MenuItem("GameObject/Snow Ui/Icon Image", false, 30)]
    private static void CreateImage()
    {
        CreateObject("ImageCircle");
    }

    [MenuItem("GameObject/Snow Ui/Button", false, 15)]
    private static void CreateButton()
    {
        CreateObject("Button");
    }

    [MenuItem("GameObject/Snow Ui/Icon Button", false, 15)]
    private static void CreateIconButton()
    {
        CreateObject("IconButton");
    }

    [MenuItem("GameObject/Snow Ui/InputField", false, 33)]
    private static void CreateInputField()
    {
        CreateObject("InputField");
    }

    [MenuItem("GameObject/Snow Ui/Text", false, 32)]
    private static void CreateText()
    {
        CreateObject("Text");
    }

    [MenuItem("GameObject/Snow Ui/Dropdown", false, 33)]
    private static void CreateDropdown()
    {
        CreateObject("Dropdown");
    }

    [MenuItem("GameObject/Snow Ui/Toggle", false, 33)]
    private static void CreateToggle()
    {
        CreateObject("Toggle");
    }

    [MenuItem("GameObject/Snow Ui/Canvas",false, 1)]
    private static void CreateCanvas()
    {
        CreateObject("Canvas");
    }

    [MenuItem("GameObject/Snow Ui/Image", false, 31)]
    private static void CreateBorderImage()
    {
        CreateObject("BorderImage");
    }

    [MenuItem("GameObject/Snow Ui/Circular Slider", false, 34)]
    private static void CreateCircularSlider()
    {
        CreateObject("CircularSlider");
    }
}
