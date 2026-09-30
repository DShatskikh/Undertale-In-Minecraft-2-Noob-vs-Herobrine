using System;
using TMPro;
using UnityEngine;

public sealed class FightLabel : MonoBehaviour
{
    public TextMeshPro Label;
    public TextMeshPro Star;
    public Transform HealthSlide;

    private int _index;
    
    public void SetHealth(int index, int health, int maxHealth, bool isYellow)
    {
        _index =  index;
        
        Label.color = isYellow ? Color.yellow : Color.white;
        Star.color = isYellow ? Color.yellow : Color.white;
        HealthSlide.localScale = new Vector3(Mathf.Lerp(0, 1, health / ((float)maxHealth)), 1, 1);
        HealthSlide.localPosition = new Vector2((1 - HealthSlide.localScale.x) / -2, 0);
    }

    private void OnMouseDown()
    {
        BattleManager.Instance.FightSelectIndex = _index;
        BattleManager.Instance.SelectScreen(4);
    }
}
