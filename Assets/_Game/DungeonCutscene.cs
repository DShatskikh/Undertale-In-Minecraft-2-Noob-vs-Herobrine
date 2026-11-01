using System.Collections;
using UnityEngine;

public sealed class DungeonCutscene : MonoBehaviour
{
    [SerializeField]
    private GameObject _herobrine;
    
    [SerializeField]
    private AudioSource _sfx;
    
    private IEnumerator Start()
    {
        if (Stats.Instance.Data[44] == 1 || Stats.Instance.Data[7] == 1)
        {
            gameObject.SetActive(false);
            yield break;
        }
        
        Player.Instance.enabled = false;
        Player.Instance.transform.position = new Vector3(16.0714512f,-1.97280979f,0);
        yield return null;

        yield return DialogueWindow.StartDialogue(new []
        {
            "/n0Херобрин!",
            "/y0Ахаха.*Ты опоздал нубяра.",
            "/n0НЕЕЕЕТ!",
            "/y0Я исчезаю.",
        }, true);

        _sfx.Play();
        
        while (_herobrine.GetComponent<SpriteRenderer>().color.a > 0f)
        {
            yield return null;
            _herobrine.GetComponent<SpriteRenderer>().color = _herobrine.GetComponent<SpriteRenderer>().color.AddA(-Time.deltaTime);
            _herobrine.GetComponentsInChildren<SpriteRenderer>()[1].color = _herobrine.GetComponent<SpriteRenderer>().color;
        }
        
        _herobrine.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        
        yield return DialogueWindow.StartDialogue(new []
        {
            "/n0Я был так близок...",
        }, true);

        Stats.Instance.Data[44] = 1;
        Player.Instance.enabled = true;
    }
}
