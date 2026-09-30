using UnityEngine;

public sealed class Flower : MonoBehaviour
{
    private float _speed = 0.25f;
    private float _intensivity = 1f;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _intensivity = 4;
            GetComponent<SpriteRenderer>().material.SetFloat("_WindIntensivity", _intensivity);
        }
    }
    
    private void Update()
    {
        if (_speed > 0.25f)
        {
            _speed -= Time.deltaTime * 5;
            
            if (_speed < 0.25f)
                _speed = 0.25f;
            
            GetComponent<SpriteRenderer>().material.SetFloat("_WindSpeed", _speed);
        }
        
        if (_intensivity > 1f)
        {
            _intensivity -= Time.deltaTime * 10;
            
            if (_intensivity < 1f)
                _intensivity = 1f;
            
            GetComponent<SpriteRenderer>().material.SetFloat("_WindIntensivity", _intensivity);
        }
    }
}
