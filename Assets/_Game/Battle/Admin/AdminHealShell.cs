using UnityEngine;

public sealed class AdminHealShell : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponentInParent<Heart>() == null)
            return;
        
        ((AdminEnemy)BattleManager.Instance.Enemies[0]).IsJumpedPlatform = true;
        BattleManager.Instance.Health(0);
        Destroy(gameObject);
    }
}
