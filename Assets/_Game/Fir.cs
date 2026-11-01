using System.Collections;
using UnityEngine;

public sealed class Fir : BannedUsable
{
    protected override int _id => 14;
    protected override string _message => "/f0Привет.";

    private bool _isSpeak;
    
    public override IEnumerator AwaitUse()
    {
        if (Stats.Instance.Data[6] == 1)
        {
            yield return AwaitBan();
        }
        else
        {
            var flip = GetComponent<SpriteRenderer>().flipX;
            GetComponent<SpriteRenderer>().flipX = transform.position.x > Player.Instance.transform.position.x;
            Player.Instance.enabled = false;

            if (Stats.Instance.Data[5] == 0)
            {
                if (!_isSpeak)
                {
                    yield return DialogueWindow.StartDialogue(new string[]
                    {
                        "/n0Чувак, ты не видел Херобрина?",
                        "/f0Он пошел вниз, чувак.",
                        "/n0Спасибо, чувак.",
                        "/f0Пожалуйста, чувак.",
                    }, false);

                    _isSpeak = true;
                }
                else
                {
                    yield return DialogueWindow.StartDialogue(new string[]
                    {
                        "/n0ЧУВАААК.",
                        "/f0ЧУВАААК.",
                    }, false);
                }
            }
            else
            {
                yield return DialogueWindow.StartDialogue(new string[] { "/f0Удачи тебе с Херобрином." }, false);
            }

            Player.Instance.enabled = true;
            GetComponent<SpriteRenderer>().flipX = flip;
        }
    }
}
