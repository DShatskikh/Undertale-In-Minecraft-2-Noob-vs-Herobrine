using System.Collections;

public sealed class Admin : BannedUsable
{
    protected override int _id => 18;
    protected override string _message => "/a0Не веди себя плохо.";
    
    public override IEnumerator AwaitUse()
    {
        if (Stats.Instance.Data[6] == 1)
        {
            yield return AwaitBan();
        }
        else
        {
            Player.Instance.enabled = false;
            yield return DialogueWindow.StartDialogue(new string[] {"/a0Не веди себя плохо."}, false);
            Player.Instance.enabled = true;
        }
    }
}
