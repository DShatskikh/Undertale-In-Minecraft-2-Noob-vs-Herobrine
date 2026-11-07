using System;
using System.Collections;
using UnityEngine;

public sealed class GolemShell : MonoBehaviour
{
    private const float SPEED = 5;

    [SerializeField]
    private Transform _golem;

    [SerializeField]
    private GameObject _boom;
    
    private IEnumerator Start()
    {
        while (_golem && _golem.localPosition.y > -2.22f)
        {
            _golem.localPosition = _golem.localPosition.AddY(-Time.deltaTime * SPEED);
            yield return null;
        }

        if (_golem == null)
            yield break;
        
        CameraShaker.Instance.Shake();
        _golem.gameObject.SetActive(false);
        _boom.transform.position = _golem.position.AddY(-0.75f);
        _boom.SetActive(true);
    }
}
