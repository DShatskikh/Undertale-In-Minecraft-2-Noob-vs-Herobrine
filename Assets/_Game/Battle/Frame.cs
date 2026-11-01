using UnityEngine;

public sealed class Frame : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D _up, _down, _left, _right;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    private void Update()
    {
        SetFrame(_spriteRenderer.size);
    }

    public void SetFrame(Vector2 size)
    {
        _spriteRenderer.size = size;
        transform.position = transform.position.SetY(_spriteRenderer.size.y - 3.75f);
        
        _right.transform.localPosition = new Vector3(_spriteRenderer.size.x / 2, 0);
        _right.transform.localScale = new Vector3(0.15f, _spriteRenderer.size.y);
        
        _left.transform.localPosition = new Vector3(-_spriteRenderer.size.x / 2, 0);
        _left.transform.localScale = new Vector3(0.15f, _spriteRenderer.size.y);
        
        _up.transform.localPosition = new Vector3(0, _spriteRenderer.size.y / 2);
        _up.transform.localScale = new Vector3(_spriteRenderer.size.x, 0.1f); // 1.75f
        
        _down.transform.localPosition = new Vector3(0, -_spriteRenderer.size.y / 2);
        _down.transform.localScale = new Vector3(_spriteRenderer.size.x, 0.175f);
    }

    [ContextMenu("ToDown")]
    public void ToDown()
    {
        SetFrame(_spriteRenderer.size);
    }

    [ContextMenu("ResetFrame")]
    public void ResetFrame()
    {
        SetFrame(new Vector2(7.177707f, 1.75f));
    }
}
