using UnityEngine;

public sealed class Reflection : MonoBehaviour
{
    [SerializeField]
    private Vector2 _offset;
    
    private void LateUpdate()
    {
        transform.position = Player.Instance.transform.position + (Vector3)_offset;
        transform.localScale = Player.Instance.transform.localScale;
        GetComponent<SpriteRenderer>().sprite = Player.Instance.View.sprite;
    }
}
