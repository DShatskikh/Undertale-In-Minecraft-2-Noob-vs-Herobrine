using System.Collections;

public sealed class HerobrineSpawn : BannedUsable
{
    protected override int _id => 11;
    protected override string _message => "/y0Ха-ха, научись приватить.";

    public override IEnumerator AwaitUse()
    {
        if (Stats.Instance.Data[6] == 1)
        {
            yield return AwaitBan();
        }
        else
        {
            Player.Instance.enabled = false;
            yield return DialogueWindow.StartDialogue(new[] { "ФФФФ" }, false);
            Player.Instance.enabled = true;
        }
    }
}
