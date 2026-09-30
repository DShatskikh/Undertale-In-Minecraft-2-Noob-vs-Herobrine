using System;
using TMPro;
using UnityEngine;

public class ActLabel : MonoBehaviour
{
    private int _type;
    private int _index;
    
    public TextMeshPro Label;
    public TextMeshPro Star;

    public void Init(int type, int index)
    {
        _type = type;
        _index = index;
    }
    
    private void OnMouseDown()
    {
        if (_type == 0) // ACT
        {
            BattleManager.Instance.ActIndex = _index;
            BattleManager.Instance.EndTurn();
        }
        else if (_type == 1) // Item
        {
            BattleManager.Instance.ItemsIndex = _index;
            BattleManager.Instance.EndTurn();
        }
        else if (_type == 2) // Mercy
        {
            BattleManager.Instance.MercyIndex = _index;
            BattleManager.Instance.EndTurn();
        }
    }
}
