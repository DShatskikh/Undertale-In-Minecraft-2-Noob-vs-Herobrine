using System;
using System.Collections;
using System.Collections.Generic;
using Febucci.TextAnimatorForUnity;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public sealed class BattleMainText : MonoBehaviour
{
    private TypewriterComponent _label;
    private bool _isSkip;
    
    public bool IsWriting => _label.IsShowingText;
    
    private void Awake()
    {
        _label = GetComponent<TypewriterComponent>();
    }

    private void Update()
    {
        if (InputManager.Instance.IsCancelDown)
        {
            _isSkip = true;
        }
    }

    public void Write(string message)
    {
        gameObject.SetActive(true);
        _label.StopShowingText();
        _label.ShowText("");
        gameObject.SetActive(true);
        _label.ShowText(message);
        _label.StartShowingText();
    }
    
    public IEnumerator AwaitWrite(string message)
    {
        gameObject.SetActive(true);
        var createdLabels = new List<TypewriterComponent>();
        string[] parts = message.Split('*', StringSplitOptions.RemoveEmptyEntries);
        var currentLabel = _label;
        var previousLabel = _label;
        
        _isSkip = false;
        
        for (var i = 0; i < parts.Length; i++)
        {
            var part = parts[i];

            if (i != 0)
            {
                currentLabel = Instantiate(previousLabel, transform);
                currentLabel.StopShowingText();
                currentLabel.GetComponent<TextMeshPro>().text = null;
                
                if (!currentLabel.gameObject.GetComponent<SortingGroup>())
                    currentLabel.gameObject.AddComponent<SortingGroup>().sortingOrder = 5;
                
                createdLabels.Add(currentLabel);
                var textMesh = previousLabel.GetComponentInChildren<TextMeshPro>();

                currentLabel.transform.position = new Vector3(previousLabel.transform.position.x,
                    previousLabel.transform.position.y - textMesh.renderedHeight,
                    previousLabel.transform.position.z); // -1

                currentLabel.GetComponentInChildren<TextMeshPro>().text = string.Empty;
            }

            previousLabel = currentLabel;
            currentLabel.ShowText("");
            currentLabel.ShowText(part);
            currentLabel.StartShowingText();
            
            while (currentLabel.IsShowingText)
            {
                if (_isSkip)
                {
                    currentLabel.SkipTypewriter();
                }
                
                yield return null;
            }
        }
        
        yield return InputManager.Instance.AwaitSubmitDownOrClick();
        
        for (int i = 0; i < createdLabels.Count; i++)
        {
            Destroy(createdLabels[i].gameObject);
        }
    }

    public void Reset()
    {
        _label.StopShowingText();
    }
}
