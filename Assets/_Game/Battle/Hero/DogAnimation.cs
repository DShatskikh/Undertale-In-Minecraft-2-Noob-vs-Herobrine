using System;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class DogAnimation : MonoBehaviour
{
    private float _startScaleX;
    private bool _isOpposition;
    private float _stay;

    private void Start()
    {
        _startScaleX = transform.localScale.x;
        _stay = Random.Range(0, 1f);
    }

    private void Update()
    {
        if (_stay > 0)
        {
            _stay -= Time.deltaTime;
            return;
        }

        const float SPEED = 20;
        const float DURATION = 0.5f;
        
        if (_isOpposition)
        {
            float speed = _startScaleX > 0 ? -1 : 1;
            speed *= SPEED;
            transform.localScale = transform.localScale.AddX(Time.deltaTime * speed);

            if (transform.localScale.x <= -_startScaleX)
            {
                _isOpposition = false;
                _stay = DURATION;
            }
        }
        else
        {
            float speed = _startScaleX < 0 ? -1 : 1;
            speed *= SPEED;
            transform.localScale = transform.localScale.AddX(Time.deltaTime * speed);

            if (transform.localScale.x >= _startScaleX)
            {
                _isOpposition = true;
                _stay = DURATION;
            }
        }
    }
}
