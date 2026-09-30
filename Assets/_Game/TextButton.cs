using TMPro;
using UnityEngine;
using UnityEngine.Events;

public sealed class TextButton : MonoBehaviour
{
    public UnityAction Select;
    public UnityAction Click;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            int wordIndex = TMP_TextUtilities.FindIntersectingWord(GetComponent<TextMeshPro>(), mousePosition, Camera.main);
            
            if (wordIndex != -1)
            {
                Select.Invoke();
            }
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            int wordIndex = TMP_TextUtilities.FindIntersectingWord(GetComponent<TextMeshPro>(), mousePosition, Camera.main);
            
            if (wordIndex != -1)
            {
                Click.Invoke();
            }
        }
    }
}
