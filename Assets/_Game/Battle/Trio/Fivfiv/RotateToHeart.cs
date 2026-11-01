using System;
using UnityEngine;

public sealed class RotateToHeart : MonoBehaviour
{
    private void Start()
    {
        var v = Heart.Instance.transform.position - transform.position;
        var angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
