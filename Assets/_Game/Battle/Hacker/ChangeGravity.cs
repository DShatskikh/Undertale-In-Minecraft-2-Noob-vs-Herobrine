using System;
using UnityEngine;

public sealed class ChangeGravity : MonoBehaviour
{
    [SerializeField]
    private Vector2 _gravity = Vector2.down;

    [SerializeField]
    private float _timer = 5;

    [SerializeField]
    private bool _isMoveToCenter = true;
    
    private AudioSource _sfx;

    private void Awake()
    {
        _sfx = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_timer < 0)
        {
            if (_isMoveToCenter)
            {
                Heart.Instance.enabled = false;
                Heart.Instance.transform.position = Vector2.MoveTowards(Heart.Instance.transform.position, 
                    BattleManager.Instance.GetFrame.transform.position, Time.deltaTime * 3);

                if (Heart.Instance.transform.position == BattleManager.Instance.GetFrame.transform.position)
                {
                    _isMoveToCenter = false;
                }
            }
            else
            {
                Heart.Instance.enabled = true;
                Heart.Instance.GravityDirection = _gravity;
                enabled = false;
                _sfx.Play();
            }
        }
        else
        {
            _timer -= Time.deltaTime;
        }
    }
}
