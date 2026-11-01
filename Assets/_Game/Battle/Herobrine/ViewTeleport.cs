using System;
using UnityEngine;

public sealed class ViewTeleport : MonoBehaviour
{
    [SerializeField]
    private Transform _view;
    
    [SerializeField]
    private Vector3 _viewPosition;

    private void Start()
    {
        _view.localPosition = _viewPosition;
    }
}
