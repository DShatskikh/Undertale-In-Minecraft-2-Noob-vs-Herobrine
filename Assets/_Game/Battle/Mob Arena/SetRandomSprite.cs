using System;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class SetRandomSprite : MonoBehaviour
{
    [SerializeField]
    private Sprite[] _sprites;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = _sprites[Random.Range(0, _sprites.Length)];
    }
}
