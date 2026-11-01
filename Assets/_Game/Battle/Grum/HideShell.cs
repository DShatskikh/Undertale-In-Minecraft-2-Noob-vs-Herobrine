using UnityEngine;

public class HideShell : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = _spriteRenderer.color.SetA(1);
    }

    private void Update()
    {
        _spriteRenderer.color = _spriteRenderer.color.AddA(-Time.deltaTime);
        
        if (_spriteRenderer.color.a <= 0)
            Destroy(gameObject);
    }
}
