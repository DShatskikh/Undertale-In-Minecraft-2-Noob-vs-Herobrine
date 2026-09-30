using System.Collections;
using UnityEngine;

public sealed class UsableDialogue : Usable
{
    [SerializeField]
    [TextArea]
    private string[] _dialogues;
    
    [SerializeField]
    [TextArea]
    private string[] _alternativeDialogues;
    
    [SerializeField]
    private bool _isDown;
    
    private bool _isUsable;
    
    public void SetDialogues(string[] dialogues)
    {
        _dialogues = dialogues;
    }

    public override IEnumerator AwaitUse()
    {
        if (DialogueWindow.Instance)
            yield break;

        Player.Instance.enabled = false;
        
        var dialogues = _isUsable && _alternativeDialogues.Length != 0 ? _alternativeDialogues : _dialogues;
        yield return DialogueWindow.StartDialogue(dialogues, _isDown);
        Player.Instance.enabled = true;
        _isUsable = true;
    }
}
