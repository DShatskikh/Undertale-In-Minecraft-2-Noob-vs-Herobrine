using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class CameraShaker : MonoBehaviour
{
    public  static CameraShaker Instance;
    
    private Vector3 _startPosition;

    private void Start()
    {
        Instance = this;
        _startPosition = transform.position;
    }

    public void Shake()
    {
        StartCoroutine(AwaitShake());
    }

    private IEnumerator AwaitShake()
    {
        var amplitude = 0.1f;
        
        for (int i = 0; i < 10; i++)
        {
            yield return AwaitMoveTo(_startPosition.AddX(Random.Range(-amplitude, amplitude)).AddY(Random.Range(-amplitude, amplitude)));
        }
        
        yield return AwaitMoveTo(_startPosition);
    }
    
    private IEnumerator AwaitMoveTo(Vector3 position)
    {
        while (transform.position != position)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, Time.deltaTime * 5f);
            yield return null;
        }
    }
}
