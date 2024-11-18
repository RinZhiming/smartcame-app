using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CircularSlider))]
public class CircularSliderEditor : Editor
{
    private SerializedProperty sliderValue, sliderImage, effectImage;
    private CircularSlider slider;

    private void OnEnable()
    {
        sliderValue = serializedObject.FindProperty("sliderValue");
        sliderImage = serializedObject.FindProperty("sliderImage");
        effectImage = serializedObject.FindProperty("effectImage");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        slider = target as CircularSlider;

        EditorGUILayout.PropertyField(sliderValue);
        EditorGUILayout.PropertyField(sliderImage);
        EditorGUILayout.PropertyField(effectImage);

        if(slider.SliderImage != null && slider.EffectImage != null)
        {
            slider.SliderImage.fillAmount = slider.SliderValue;
            slider.EffectImage.fillAmount = slider.SliderValue;
        }
        else
        {
            EditorGUILayout.LabelField("You don't have slider graphic.");
        }

        serializedObject.ApplyModifiedProperties();
    }
}
