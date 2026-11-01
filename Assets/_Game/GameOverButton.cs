using System;
using System.Collections;
using TMPro;
using UnityEngine;

public sealed class GameOverButton : MonoBehaviour
{
    [SerializeField]
    private int _index;
    
    [SerializeField]
    private SpriteRenderer _icon;
    
    [SerializeField]
    private TextMeshPro _label;

    [SerializeField]
    private GameOver _gameOver;
    
    private SpriteRenderer _frame;
    private readonly Color _selectedColor = new Color32(0, 255, 255, 255);
    private readonly Color _unselectedColor = new Color32(0, 86, 255, 255);

    public bool IsSelected;
    
    private void Awake()
    {
        _frame = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _icon.color = _unselectedColor.SetA(0);
        _label.color = _unselectedColor.SetA(0);
        _frame.color = _unselectedColor.SetA(0);

        Heart.Instance.enabled = false;
        Heart.Instance.gameObject.SetActive(false);
    }

    // private void Update()
    // {
    //     if (IsSelected)
    //     {
    //         _frame.color = _selectedColor;
    //         _label.color = _selectedColor;
    //     
    //         _icon.gameObject.SetActive(false);
    //     }
    //     else
    //     {
    //         _frame.color = _unselectedColor;
    //         _label.color = _unselectedColor;
    //     
    //         _icon.gameObject.SetActive(true);
    //     }
    // }
    
    private void OnMouseDown()
    {
        if (IsSelected)
            _gameOver.Next();
    }

    public IEnumerator AwaitShow()
    {
        var alpha = 0f;
        
        while (alpha < 1)
        {
            alpha += Time.deltaTime * 2;
            _icon.color = _icon.color.SetA(alpha);
            _label.color = _label.color.SetA(alpha);
            _frame.color = _frame.color.SetA(alpha);
            yield return null;
        }

        _icon.gameObject.SetActive(false);
        _label.color = _selectedColor;
        _frame.color = _selectedColor;
        
        IsSelected = true;
        Heart.Instance.gameObject.SetActive(true);
    }
}
