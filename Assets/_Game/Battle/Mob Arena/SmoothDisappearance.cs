using System;
using UnityEngine;

public sealed class SmoothDisappearance : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _spriteRenderer.color = _spriteRenderer.color.AddA(-Time.deltaTime);
    }
}
