using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class Dance : MonoBehaviour
{
    [SerializeField]
    private float _upSpeed = 1;
    
    [SerializeField]
    private float _downSpeed = 1;

    [SerializeField]
    private float _timer;

    private IEnumerator Start()
    {
        var spartPosition = transform.position;
        yield return new WaitForSeconds(Random.Range(0f, 1f));
        float height = 0.3f;
        
        while (true)
        {
            while (transform.position.y < spartPosition.y + height)
            {
                transform.position = transform.position.AddY(Time.deltaTime * _upSpeed);
                yield return null;
            }
            
            while (transform.position.y > spartPosition.y)
            {
                transform.position = transform.position.AddY(-Time.deltaTime * _downSpeed);
                yield return null;
            }
            
            yield return new WaitForSeconds(_timer);
        }
    }
}
