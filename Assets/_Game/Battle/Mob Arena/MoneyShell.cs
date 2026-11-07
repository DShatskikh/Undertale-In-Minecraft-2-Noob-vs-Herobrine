using UnityEngine;

public sealed class MoneyShell : MonoBehaviour
{
    [SerializeField]
    private int _money = 1;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Heart>() == null)
            return;
        
        ((MobArena)BattleManager.Instance.Enemies[0]).AddMoney(_money);
        Destroy(gameObject);
    }
}
