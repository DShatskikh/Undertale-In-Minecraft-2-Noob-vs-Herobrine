using UnityEngine;
using UnityEngine.UIElements;

public sealed class InputHint : MonoBehaviour
{
    private UIDocument _uIDocument;
    
    private void Start()
    {
        _uIDocument = GetComponent<UIDocument>();
        
        if (Meta.Instance.IsCompleteTutorial)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Meta.Instance.IsCompleteTutorial = true;
        }
    }

    private void Update()
    {
        _uIDocument.enabled = !(SaveWindow.Instance || DialogueWindow.Instance || SelectWindow.Instance);
    }
}
