using UnityEngine;

public sealed class AliveOrRelationshipSwitcher : MonoBehaviour
{
    [SerializeField]
    private int _index;

    [SerializeField]
    private GameObject _defaultAttack, _relationShipAttack;

    private void Start()
    {
        var enemy = BattleManager.Instance.Enemies[_index];
        _defaultAttack.SetActive(false);
        
        if (_relationShipAttack)
            _relationShipAttack.SetActive(false);

        if (enemy.IsActive)
        {
            if (enemy.Relationship > 0)
            {
                if (_relationShipAttack)
                    _relationShipAttack.SetActive(true);
            }
            else
            {
                _defaultAttack.SetActive(true);
            }
        }
    }
}
