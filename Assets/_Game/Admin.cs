using System.Collections;

public sealed class Admin : Usable
{
    public override IEnumerator AwaitUse()
    {
        Player.Instance.enabled = false;
        yield return DialogueWindow.StartDialogue(new string[] {"Не веди себя плохо."}, false);
        Player.Instance.enabled = true;
    }
}
