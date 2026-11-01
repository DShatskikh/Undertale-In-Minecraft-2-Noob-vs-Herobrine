using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public sealed class CatEnd : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _off;
    
    [SerializeField]
    private GameObject _tilemap_2;
    
    [SerializeField]
    private Tilemap[] _tilemaps;
    
    private IEnumerator Start()
    {
        if (Stats.Instance.Data[36] == 0)
        {
            gameObject.SetActive(false);
            yield break;
        }
        
        foreach (var item in _off)
        {
            item.SetActive(false);
        }

        _tilemap_2.SetActive(true);
        
        foreach (var item in _tilemaps)
        {
            item.color = new Color32(53, 92, 178, 255);
        }
        
        Player.Instance.enabled = false;
        Player.Instance.SetFlex(true);
        
        Player.Instance.transform.position = new Vector3(1.20000005f, -5.37934971f, 0);
        
        yield return null;
        MusicManager.Instance.Stop();
        
        if (Stats.Instance.Data[90] == 0)
        {
            yield return DialogueWindow.StartDialogue(new []
            {
                "/n0Наконец-то я восстановил дом.",
                "/n0О котик!",
                "/c0Мяу.",
                "/n0Черт, я же обещал Нотчу прийти.",
            }, false);
        }
        else
        {
            yield return DialogueWindow.StartDialogue(new []
            {
                "/n0Котик у тебя получилось!",
                "/c0Мяу.",
                "/n0Ладно, пора идти к Нотчу.",
            }, false); 
        }
        
        Player.Instance.enabled = true;
    }
}
