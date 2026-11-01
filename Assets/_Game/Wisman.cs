using System.Collections;
using UnityEngine;

public sealed class Wisman : MonoBehaviour
{
    private IEnumerator Start()
    {
        if (Stats.Instance.Data[6] != 1)
        {
            gameObject.SetActive(false);
            yield break;
        }
        
        if (Stats.Instance.Data[21] == 1)
        {
            gameObject.SetActive(false);
            yield break;
        }
        
        yield return new WaitForSeconds(0.5f);
        
        var spriteRenderer = GetComponent<SpriteRenderer>();

        while (spriteRenderer.color.a > 0)
        {
            spriteRenderer.color = spriteRenderer.color.AddA(-Time.deltaTime);
            yield return null;
        }

        Stats.Instance.Data[21] = 1;
        gameObject.SetActive(false);
    }
}
