using System.Collections;
using Febucci.TextAnimatorForUnity;
using UnityEngine;

public sealed class EnemyMessageBox : MonoBehaviour
{
    [SerializeField]
    private TypewriterComponent _label;

    public static IEnumerator AwaitWriting(Vector3 position, string message)
    {
        var messageBox = Instantiate(Resources.Load<EnemyMessageBox>("Enemy Message Box"), position,
            Quaternion.identity);
        
        messageBox._label.ShowText(message);
        messageBox._label.StartShowingText();

        while (messageBox._label.IsShowingText && !InputManager.Instance.IsCancelDown)
        {
            yield return null;
        }
        
        messageBox._label.SkipTypewriter();
        yield return InputManager.Instance.AwaitSubmitDownOrClick();
        Destroy(messageBox.gameObject);
    }
}
