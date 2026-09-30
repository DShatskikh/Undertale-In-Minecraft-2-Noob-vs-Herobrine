using TMPro;
using UnityEngine;

public class BattleButton : MonoBehaviour
{
    [SerializeField]
    private int _index;
    
    [SerializeField]
    private SpriteRenderer _icon;
    
    [SerializeField]
    private TextMeshPro _label;
    
    private SpriteRenderer _frame;
    private readonly Color _selectedColor = new Color32(0, 255, 255, 255);
    private readonly Color _unselectedColor = new Color32(0, 86, 255, 255);

    private void Awake()
    {
        _frame = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        var index = _index;

        if (_index == 1 && BattleManager.Instance.MainIndex == 5)
            index = 5;
        
        if (BattleManager.Instance.MainIndex == index)
        {
            _frame.color = _selectedColor;
            _label.color = _selectedColor;
        
            _icon.gameObject.SetActive(false);
        }
        else
        {
            _frame.color = _unselectedColor;
            _label.color = _unselectedColor;
        
            _icon.gameObject.SetActive(true);
        }
    }
    
    private void OnMouseDown()
    {
        var index = _index;

        if (_index == 1 && BattleManager.Instance.Enemies.Count == 1)
        {
            index = 5;
        }
        
        BattleManager.Instance.MainIndex = index;
        BattleManager.Instance.SelectScreen(index);
    }
}
