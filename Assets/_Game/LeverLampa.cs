using System.Collections;
using UnityEngine;

public sealed class LeverLampa : Usable
{
    [SerializeField]
    private int _save;

    [SerializeField]
    private AudioSource _sfx;

    [SerializeField]
    private Sprite _activate;

    [SerializeField]
    private SpriteRenderer _lamp;

    [SerializeField]
    private GameObject _spawner;
    
    private void Start()
    {
        if (Stats.Instance.Data[_save] == 1)
        {
            Activate();
        }
        else
        {
            _lamp.color = new Color(0.5f, 0.5f, 0.5f, 1);
        }
    }
    
    public override IEnumerator AwaitUse()
    {
        if (Stats.Instance.Data[_save] == 1)
            yield break;
        
        Player.Instance.enabled = false;

        Stats.Instance.Data[_save] = 1;
        _sfx.Play();
        Activate();
        yield return new WaitForSeconds(0.5f);
        
        Player.Instance.enabled = true;
    }

    private void Activate()
    {
        if (_spawner)
            _spawner.SetActive(false);
            
        GetComponent<SpriteRenderer>().sprite = _activate;
        _lamp.color = new Color(1f, 1f, 1f, 1);
    }
}
