using System.Collections;
using UnityEngine;

public sealed class TeleportShell : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
