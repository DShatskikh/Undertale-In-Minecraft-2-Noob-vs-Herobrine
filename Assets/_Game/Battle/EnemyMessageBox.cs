using System.Collections;
using Febucci.TextAnimatorCore.Text;
using Febucci.TextAnimatorForUnity;
using UnityEngine;

public sealed class EnemyMessageBox : MonoBehaviour
{
    public static EnemyMessageBox Instance;
    
    [SerializeField]
    private TypewriterComponent _label;

    private AudioSource _sfx;
    
    public static IEnumerator AwaitWriting(Vector3 position, string message, bool isMirrored = false)
    {
        var messageBox = Instantiate(Resources.Load<EnemyMessageBox>(isMirrored 
                ? "Enemy Message Box Mirrored" : "Enemy Message Box"), position, Quaternion.identity);
     
        Instance = messageBox;
        messageBox._label.ShowText(message);
        messageBox._label.StartShowingText();

        while (messageBox._label.IsShowingText && !InputManager.Instance.IsCancelDown)
        {
            yield return null;
        }

        messageBox._sfx = messageBox.GetComponent<AudioSource>();
        messageBox._label.onCharacterVisible.AddListener(messageBox.Call);
        messageBox._label.SkipTypewriter();
        yield return InputManager.Instance.AwaitSubmitDownOrClick();
        Destroy(messageBox.gameObject);
    }

    private void Call(CharacterData arg0)
    {
        if (!_sfx.isPlaying)
            _sfx.Play();
    }
}
