using UnityEngine;

public abstract class ShellBase : MonoBehaviour
{
    public abstract bool CanDamage { get; set; }
    public virtual int Damage { get; } = 1;
    public float Progress = 2f;
}
