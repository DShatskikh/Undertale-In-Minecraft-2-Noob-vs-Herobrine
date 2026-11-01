using System;
using System.Collections;
using UnityEngine;

public sealed class PVPArenaTrigger : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.GetComponent<Player>())
            return;

        StartCoroutine(AwaitTrigger());
    }

    private IEnumerator AwaitTrigger()
    {
        Player.Instance.enabled = false;

        yield return DialogueWindow.StartDialogue(new string[]
        {
            "(Невидимый барьер не даёт вам пройти.)"
        }, true);
        
        Player.Instance.enabled = true;
    }
}
