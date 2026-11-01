using System.Collections;
using UnityEngine;

public abstract class BaseAttack : MonoBehaviour
{
    public abstract Vector2 SizeField { get; }
    public abstract IEnumerator AwaitProcess();
}
