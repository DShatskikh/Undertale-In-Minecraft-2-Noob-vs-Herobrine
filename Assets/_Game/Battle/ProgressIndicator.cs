using TMPro;
using UnityEngine;

public sealed class ProgressIndicator : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _label;

    private void Update()
    {
        _label.text = $"Прогресс хода {(int)BattleManager.Instance.Progress}%";
    }
}
