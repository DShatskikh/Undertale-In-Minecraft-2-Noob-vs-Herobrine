using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Cafe : MonoBehaviour
{
    [SerializeField]
    private AudioClip _theme;

    [SerializeField]
    private AudioSource _stop;

    [SerializeField]
    private SpriteRenderer[] _all;

    [SerializeField]
    private Transform _skeleton;

    [SerializeField]
    private GameObject _cat, _normal;
    
    private IEnumerator Start()
    {
        if (Stats.Instance.Data[90] == 1)
        {
            _cat.SetActive(true);
            _normal.SetActive(false);
        }
        
        var dances = FindObjectsByType<Dance>(FindObjectsSortMode.None);
        
        if (Stats.Instance.Data[2] == 1)
        {
            MusicManager.Instance.Stop();
            _stop.Play();
            Player.Instance.enabled = false;

            yield return new WaitForSeconds(1);

            var defaultX = new bool[_all.Length];

            for (var i = 0; i < _all.Length; i++)
            {
                var enemy = _all[i];
                defaultX[i] = enemy.flipX;
                enemy.flipX = Player.Instance.transform.position.x < enemy.transform.position.x;
            }

            _skeleton.localScale = _skeleton.localScale.SetX(-1);
            
            yield return new WaitForSeconds(1);
            
            yield return DialogueWindow.StartDialogue(new[]
            {
                "/n0...",
                "/n0Вы все живы?",
            }, false);
            
            yield return DialogueWindow.StartDialogue(new[]
            {
                "/g0...",
                "/i0Чувак, это просто игра.",
                "/j0Мы нажали кнопку \"Возродиться\".",
            }, false);
            
            yield return new WaitForSeconds(0.5f);
            
            foreach (var dance in dances)
            {
                dance.enabled = true;
            }
            
            for (var i = 0; i < _all.Length; i++)
            {
                var enemy = _all[i];
                enemy.flipX = defaultX[i];
            }
            
            _skeleton.localScale = _skeleton.localScale.SetX(1);
            
            MusicManager.Instance.Play(_theme); 
            Player.Instance.enabled = true;
        }
        else
        {
            foreach (var dance in dances)
            {
                dance.enabled = true;
            }
            
            MusicManager.Instance.Play(_theme); 
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Player>() == null)
            return;

        StartCoroutine(AwaitTrigger());
    }

    private IEnumerator AwaitTrigger()
    {
        Player.Instance.enabled = false;
        yield return SelectWindow.StartSelect("Закончить игру?", "Да", "Нет");

        if (!SelectWindow.IsRight)
        {
            if (Meta.Instance.IsEndEvil)
            {
                yield return DialogueWindow.StartDialogue(new []
                {
                    "/n0Я вспомнил, у меня же есть..."
                }, true);
            
                CoroutineRunner.Instance.StartCoroutine(
                    SwitchingLevel.AwaitSwitching("Theatre", new Vector3(4.03854799f, -4.52383471f, 0)));
            }
            else
            {
                if (Stats.Instance.Data[2] == 0)
                {
                    Meta.Instance.IsEndPacifist = true;
                    Stats.Instance.Reset();
                    SceneManager.LoadScene("Menu");
                }
                else
                {
                    Meta.Instance.IsEndGenocide = true;
                    Stats.Instance.Reset();
                    SceneManager.LoadScene("Menu");
                }
            }
        }
        else
        {
            Player.Instance.enabled = true;
        }
    }
}
