using UnityEngine;

public sealed class Shield : MonoBehaviour
{
    [SerializeField]
    private AudioSource _sfx;
    
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _spriteRenderer.color = _spriteRenderer.color.AddA(-Time.deltaTime);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log(other.name);
        
        if (!other.GetComponent<Shell>()) //  || !other.GetComponent<Shell>().CanDamage
            return;
        
        if (_spriteRenderer.color.a < 0.5f)
        {
            _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            BattleManager.Instance.Progress += 5;
            _sfx.Play();
        }
    }
}
