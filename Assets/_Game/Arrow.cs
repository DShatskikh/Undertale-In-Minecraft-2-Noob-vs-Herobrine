using System;
using UnityEngine;

public sealed class Arrow : MonoBehaviour
{
    public static bool IsOtherSide;
    
    private float _timer = 10;
    
    private void Update()
    {
        transform.Translate(Vector2.left * Time.deltaTime * 2);
        
        _timer -= Time.deltaTime;

        if (_timer <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() && other.GetComponent<Player>().enabled)
        {
            StartBattleScreen.Transition("Arrow Enemy");
        }
    }
}
