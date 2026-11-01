using System;
using System.Collections;
using UnityEngine;

public sealed class Lever : Usable
{
    [SerializeField]
    private PlateManager _plateManager;

    [SerializeField]
    private AudioSource _sfx;
    
    [SerializeField]
    private Sprite[] _sprites;

    private void Start()
    {
        if (Stats.Instance.Data[43] == 1)
        {
            GetComponent<SpriteRenderer>().sprite = _sprites[1];
        }
    }

    public override IEnumerator AwaitUse()
    {
        if (Stats.Instance.Data[43] == 1)
            yield break;
        
        Player.Instance.enabled = false;
        
        _sfx.Play();
        GetComponent<SpriteRenderer>().sprite = _sprites[1];
        yield return new WaitForSeconds(1);
        
        _plateManager.OnResetActivate(default);

        if (_plateManager.GetActivated)
        {
            yield return DialogueWindow.StartDialogue(new []
            {
                "/n0Ура! Получилось!."
            }, true);

            Stats.Instance.Data[43] = 1;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = _sprites[0];
            
            yield return new WaitForSeconds(0.5f);
            
            yield return DialogueWindow.StartDialogue(new []
            {
                "/n0Ну блин."
            }, true);
        }
        
        Player.Instance.enabled = true;
    }
}
