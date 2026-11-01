using System;
using UnityEngine;

public sealed class SpikeHeroShell : ShellBase
{
    private float _timerHide2 = 1f;
    private ShellTrigger _trigger;
    private bool _isHide;
    
    public float Timer = 0.5f;
    public float TimerHide = 0.5f;
    public override bool CanDamage { get; set; }

    private void Start()
    {
        _trigger = GetComponentInChildren<ShellTrigger>();
    }

    private void Update()
    {
        if (_isHide)
        {
            GetComponentInChildren<SpriteRenderer>().color = GetComponentInChildren<SpriteRenderer>().color.AddA(-Time.deltaTime);
            CanDamage = false;
        }
        
        if (Timer <= 0)
        {
            if (!_isHide)
                _trigger.CanDamage = true;
        }
        else
        {
            Timer -= Time.deltaTime;
            return;
        }
        
        if (TimerHide <= 0)
        {
            GetComponent<Animator>().Play("Spike Hide");
            _trigger.CanDamage = false;
        }
        else
        {
            TimerHide -= Time.deltaTime;
            return;
        }
        
        if (_timerHide2 <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            _timerHide2 -= Time.deltaTime;
        }
    }

    public void Hide()
    {
        _isHide = true;
        CanDamage = false;
    }
}
