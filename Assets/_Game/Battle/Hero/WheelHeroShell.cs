using System.Collections;
using UnityEngine;

public sealed class WheelHeroShell : ShellBase
{
    [SerializeField]
    private float _speed = 0.5f;

    private SpriteRenderer _view;
    private bool _isShow;

    

    public override bool CanDamage { get; set; }

    private void Start()
    {
        _view = GetComponent<SpriteRenderer>();
        _view.color = _view.color.SetA(0);
    }

    private void Update()
    {
        if (!_isShow)
        {
            if (_view.color.a < 1)
            {
                _view.color = _view.color.AddA(Time.deltaTime);
            }
            else
            {
                CanDamage = true;
                _isShow = true;
            }
        }
        
        if (!CanDamage)
            return;
        
        transform.eulerAngles = transform.rotation.eulerAngles.AddZ(Time.deltaTime * _speed);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<Heart>())
            return;
        
        Heart.Instance.Damage(5);
    }

    public IEnumerator AwaitHide()
    {
        CanDamage = false;
        
        while (_view.color.a > 0)
        {
            _view.color = _view.color.AddA(-Time.deltaTime);
            yield return null;
        }
    }
}
