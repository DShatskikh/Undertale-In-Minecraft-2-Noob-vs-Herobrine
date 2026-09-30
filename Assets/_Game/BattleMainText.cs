using Febucci.TextAnimatorForUnity;
using UnityEngine;

public sealed class BattleMainText : MonoBehaviour
{
    private TypewriterComponent _label;

    public bool IsWriting => _label.IsShowingText;
    
    private void Awake()
    {
        _label = GetComponent<TypewriterComponent>();
    }

    public void Write(string message)
    {
        gameObject.SetActive(true);
        _label.ShowText(message);
        _label.StartShowingText();
    }
}
