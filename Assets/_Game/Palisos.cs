using System.Collections;

public sealed class Palisos : BannedUsable
{
    protected override int _id => 20;
    protected override string _message => "/p0Чувак, че как?";
    
    public override IEnumerator AwaitUse()
    {
        if (Stats.Instance.Data[6] == 1)
        {
            yield return AwaitBan();
        }
        else
        {
            Player.Instance.enabled = false;

            yield return DialogueWindow.StartDialogue(new[]
            {
                "/p0Чувак, че как?",
                "/p0Я заблудился.*Не обращай внимание.",
                "/p0Я хочу сам найти выход.",
            }, false);
            
            Player.Instance.enabled = true;
        }
    }
}
