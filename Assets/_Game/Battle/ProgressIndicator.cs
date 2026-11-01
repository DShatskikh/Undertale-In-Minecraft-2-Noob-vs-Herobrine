using TMPro;
using UnityEngine;

public sealed class ProgressIndicator : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _label;

    [SerializeField]
    private Transform _field;

    [SerializeField]
    private GameObject _container;
    
    private void Update()
    {
        var progress = Mathf.Min(BattleManager.Instance.Progress, 100);
        var coef = progress / 100;
        _field.position = _field.position.SetX(Mathf.Lerp(-5.6395f, -0.13988f, coef));
        _field.GetComponent<SpriteRenderer>().size = _field.GetComponent<SpriteRenderer>().size.SetX(Mathf.Lerp(0f, 11.00024f, coef));
        _label.text = $"Прогресс хода {(int)progress}%";
        
        _container.SetActive(BattleManager.Instance.IsEnemyTurn);
    }
}
