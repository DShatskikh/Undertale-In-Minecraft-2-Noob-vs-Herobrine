using System.Collections;
using UnityEngine;

public abstract class Usable : MonoBehaviour
{
    public virtual void Use()
    {
        StartCoroutine(AwaitUse());
    }

    public abstract IEnumerator AwaitUse();
}
