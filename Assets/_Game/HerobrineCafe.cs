using System;
using UnityEngine;

public sealed class HerobrineCafe : MonoBehaviour
{
    private SpriteRenderer _view;

    private void Start()
    {
        _view = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (transform.position.y - 3 < Player.Instance.transform.position.y)
        {
            _view.flipX = transform.position.x > Player.Instance.transform.position.x;
        }
        else
        {
            _view.flipX = false;
        }
    }
}
