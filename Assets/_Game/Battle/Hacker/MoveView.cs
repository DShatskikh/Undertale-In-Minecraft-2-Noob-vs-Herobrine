using System;
using UnityEngine;

public sealed class MoveView : MonoBehaviour
{
    [SerializeField]
    private Vector3 _point;

    [SerializeField]
    private Transform _view;
    
    private void Update()
    {
        _view.transform.position = Vector3.MoveTowards(_view.transform.position, _point, Time.deltaTime * 6);
    }
}
