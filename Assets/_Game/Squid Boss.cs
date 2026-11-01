using System.Collections;
using UnityEngine;

public sealed class SquidBoss : MonoBehaviour
{
    private IEnumerator Start()
    {
        if (Stats.Instance.Data[40] == 1)
        {
            gameObject.SetActive(false);
            yield break;
        }
        
        if (Stats.Instance.Data[37] == 0)
        {
            yield break;
        }
        
        var view = GetComponent<SpriteRenderer>();
        
        Player.Instance.enabled = false;

        yield return null;
        
        yield return DialogueWindow.StartDialogue(new []
        {
            "/n0Теперь твоя очередь сквидвард.",
            "/s0Нееет!",
            "/s0Я не буду с тобой сражаться.",
            "/n0Почему?",
            "/s0Ты страшный.",
        }, false);
        
        view.flipX = false;
        yield return new WaitForSeconds(1);
        
        while (transform.localPosition.x < 8.68f)
        {
            transform.localPosition = transform.localPosition.AddX(Time.deltaTime * 5);
            yield return null;
        }
        
        gameObject.SetActive(false);
        Stats.Instance.Data[40] = 1;
        Player.Instance.enabled = true;
    }
}
