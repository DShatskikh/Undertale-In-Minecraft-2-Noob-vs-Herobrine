using System;
using TMPro;
using UnityEngine;

public sealed class Plate : MonoBehaviour
{
    [SerializeField]
    private char _symbol;

    [SerializeField]
    private AudioSource _sfx;
    
    [SerializeField]
    private Sprite _activate, _deactivate;

    [SerializeField]
    private TextMeshPro _label;
    
    private SpriteRenderer _view;
    
    private bool _isActivate;
    public event Action<char> OnActivate;
    public event Action OnExit;

    private void Start()
    {
        _view = GetComponent<SpriteRenderer>();
        _label.text = _symbol.ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponentInParent<Player>())
            return;
        
        if (_isActivate)
            return;
        
        _sfx.Play();
        OnActivate?.Invoke(_symbol);
        Activate();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.GetComponentInParent<Player>())
            return;
        
        OnExit?.Invoke();
    }

    public void Activate()
    {
        _isActivate = true;
        _view.sprite = _activate;
        _label.color = new Color32(131, 131, 131, 255);
        _label.transform.localPosition = new Vector3(0, -0.03f);
    }

    public void Reset()
    {
        _isActivate = false;
        _view.sprite = _deactivate;
        _label.color = new Color32(47, 50, 59, 255);
        _label.transform.localPosition = new Vector3(0, 0.034f);
    }
}
