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
        if (!other.GetComponent<ShellBase>() || !other.GetComponent<ShellBase>().CanDamage || other.GetComponent<ShellBase>().Damage < 0) 
            return;
        
        if (_spriteRenderer.color.a < 0.5f)
        {
            _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            BattleManager.Instance.Progress += other.GetComponent<ShellBase>().Progress;
            _sfx.Play();
        }
    }
}
