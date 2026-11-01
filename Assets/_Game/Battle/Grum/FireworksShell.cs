using System;
using System.Collections;
using UnityEngine;

public sealed class FireworksShell : MonoBehaviour
{
    [SerializeField]
    private float _timer;

    [SerializeField]
    private GameObject _fireworks, explosion, _miniShells;

    private bool _isVisible;
    
    private IEnumerator Start()
    {
        while (_timer > 0)
        {
            if (_isVisible)
                _timer -= Time.deltaTime;
            
            yield return null;
        }

        if (_fireworks)
        {
            _fireworks.SetActive(false);
        
            explosion.transform.position = _fireworks.transform.position;
            _miniShells.transform.position = _fireworks.transform.position;
        
            explosion.SetActive(true);
            _miniShells.SetActive(true);
        }
    }

    private void Update()
    {
        if (_fireworks)
        {
            if (_fireworks.transform.position.x < 8.76f && _fireworks.transform.position.x > -8.76f)
            {
                _isVisible = true;
            }
        }
    }
}
