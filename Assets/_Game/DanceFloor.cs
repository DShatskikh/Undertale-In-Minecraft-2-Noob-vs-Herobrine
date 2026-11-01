using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class DanceFloor : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer[] _views;

    private IEnumerator Start()
    {
        var colors = new[]
        {
            Color.red,
            Color.yellow,
            Color.blue,
            Color.magenta,
            Color.green,
            new(1f, 0.5f, 0f, 1f),
        };
        
        while (true)
        {
            foreach (var view in _views)
            {
                view.color = colors[Random.Range(0, colors.Length)];
            }
            
            yield return new WaitForSeconds(2);
        }
    }
}
