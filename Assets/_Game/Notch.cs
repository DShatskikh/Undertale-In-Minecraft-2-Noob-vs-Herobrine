using System.Collections;
using UnityEngine;

public sealed class Notch : BannedUsable
{
    protected override int _id => 13;
    protected override string _message => "/t0Привет.";

    private bool _isSpeak;
    
    public override IEnumerator AwaitUse()
    {
        var flip = GetComponent<SpriteRenderer>().flipX;
        GetComponent<SpriteRenderer>().flipX = transform.position.x > Player.Instance.transform.position.x;
        
        if (Stats.Instance.Data[6] == 1)
        {
            yield return AwaitBan();
        }
        else
        {
            Player.Instance.enabled = false;
            
            if (Stats.Instance.Data[5] == 0)
            {
                if (_isSpeak)
                {
                    yield return DialogueWindow.StartDialogue(new string[]
                    {
                        "/t0Он пошел к магазину вниз и на лево.",
                    }, false);
                }
                else
                {
                    yield return DialogueWindow.StartDialogue(new string[]
                    {
                        "/t0Жаль что Херобрин сломал твой дом.",
                        "/n0Как ты узнал?",
                        "/t0Он мне только что расказал.",
                        "/t0Он пошел к магазину вниз и на лево.",
                    }, false);

                    _isSpeak = true;
                }
            }
            else
            {
                yield return DialogueWindow.StartDialogue(new string[]
                {
                    "/t0Я верю в тебя."
                }, false);
            }
            
            GetComponent<SpriteRenderer>().flipX = flip;
            Player.Instance.enabled = true;
        }
    }
}
