using System;
using System.Collections;
using UnityEngine;

public class DogFlyShell : MonoBehaviour
{
    [SerializeField]
    private float _ySpeed = 4;

    [SerializeField]
    private bool _isRight;
    
    private IEnumerator Start()
    {
        var y = 3;

        while (transform.position.y < y)
        {
            yield return null;
            var speed = _ySpeed;

            if (transform.position.y > 1.5f)
            {
                speed -= 2;
            }
            
            if (transform.position.y > 2f)
            {
                speed -= 1;
            }
            
            if (transform.position.y > 2.5f)
            {
                speed -= 0.5f;
            }
            
            transform.position = transform.position.AddY(Time.deltaTime * speed);
        }

        GetComponent<Collider2D>().enabled = true;
        
        while (true)
        {
            yield return null;
            
            var speed = _ySpeed;

            speed -= 0.5f;
            
            if (transform.position.y > 1.5f)
            {
                speed -= 2;
            }
            
            if (transform.position.y > 2f)
            {
                speed -= 1;
            }
            
            if (transform.position.y > 2.5f)
            {
                speed -= 0.5f;
            }
            
            transform.position = transform.position.AddY(-Time.deltaTime * speed);
        }
    }

    private void Update()
    {
        if (!_isRight)
        {
            transform.eulerAngles = transform.eulerAngles.AddZ(Time.deltaTime * 100);
        }
        else
        {
            transform.eulerAngles = transform.eulerAngles.AddZ(Time.deltaTime * -100);
        }
    }
}
