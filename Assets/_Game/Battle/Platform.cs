using UnityEngine;

public sealed class Platform : MonoBehaviour
{
    private Collider2D _collider;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
    }
    
    private void Update()
    {
        _collider.enabled = Heart.Instance.transform.position.y - 0.15f >= transform.position.y;
    }
}
