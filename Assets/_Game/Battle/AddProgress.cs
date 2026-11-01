using System;
using UnityEngine;

public sealed class AddProgress : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2;
    
    private void Update()
    {
        BattleManager.Instance.Progress += Time.deltaTime * _speed;
    }
}
