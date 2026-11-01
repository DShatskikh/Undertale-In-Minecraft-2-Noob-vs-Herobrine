using System;
using UnityEngine;

public sealed class ArrowSideTrigger : MonoBehaviour
{
    [SerializeField]
    private bool _isOtherSide;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            Arrow.IsOtherSide = _isOtherSide;
        }
    }
}
