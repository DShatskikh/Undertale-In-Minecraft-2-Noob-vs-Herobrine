using System;
using UnityEngine;

public sealed class SetProgressMultiply : MonoBehaviour
{
    [SerializeField]
    private float _progressMultiply;
    
    private void Start()
    {
        BattleManager.Instance.ProgressMultiply = _progressMultiply;
    }
}
