using System;
using UnityEngine;
using UnityEngine.UIElements;

public sealed class HideScreen : MonoBehaviour
{
    private VisualElement _container;
    
    private void Awake()
    {
        var uiDocument = GetComponent<UIDocument>();
        _container = uiDocument.rootVisualElement.Q<VisualElement>("container");
        DontDestroyOnLoad(gameObject);
    }
    
    private void Update()
    {
        float targetAspect = 640f / 480f; // 1.333333333333333
        _container.style.width = new Length(Screen.height * targetAspect, LengthUnit.Pixel);
    }
}
