using UnityEngine;

public sealed class ShowShell : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = _spriteRenderer.color.SetA(0);
    }

    private void Update()
    {
        _spriteRenderer.color = _spriteRenderer.color.AddA(Time.deltaTime);
    }
}
