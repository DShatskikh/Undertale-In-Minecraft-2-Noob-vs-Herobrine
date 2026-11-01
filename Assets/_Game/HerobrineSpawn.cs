using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class HerobrineSpawn : BannedUsable
{
    [SerializeField]
    private GameObject _notch;

    [SerializeField]
    private GameObject _miss;
    
    protected override int _id => 11;
    protected override string _message => "/y0Ха-ха, научись приватить.";

    private bool _isSpeak;
    
    private IEnumerator Start()
    {
        base.Start();
        
        if (Stats.Instance.Data[36] == 1)
        {
            Player.Instance.enabled = false;
            Player.Instance.SetFlex(true);
            GetComponent<SpriteRenderer>().enabled = false;
            _notch.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            
            yield return DialogueWindow.StartDialogue(new[]
            {
                "/t0У тебя получилось!*Ты одолел Херобрина!",
                "/t0А это значит что... ",
            }, false);
            
            SceneManager.LoadScene("Outro 1");
        }
    }

    public override IEnumerator AwaitUse()
    {
        GetComponent<SpriteRenderer>().flipX = transform.position.x > Player.Instance.transform.position.x;
        
        if (Stats.Instance.Data[6] == 1)
        {
            yield return AwaitBan();
        }
        else if (Stats.Instance.Data[31] != 1 || Stats.Instance.Data[32] != 1 || Stats.Instance.Data[33] != 1)
        {
            if (Stats.Instance.Data[5] == 0)
            {
                Player.Instance.enabled = false;
                yield return DialogueWindow.StartDialogue(new[]
                {
                    "/y0Снова ты?",
                    "/n0Получай.",
                }, false);
             
                // (Бьёт)
                yield return Player.Instance.WaitAttack();
                _miss.SetActive(true);
                yield return new WaitForSeconds(0.5f);

                var progress = 0f;

                while (progress < 1)
                {
                    progress += Time.deltaTime * 2;
                    _miss.transform.localScale = _miss.transform.localScale.SetY(1 + progress);
                    _miss.GetComponent<TextMeshPro>().color = new Color(1, 1, 1, 1 - progress);
                    yield return null;
                }
                
                _miss.SetActive(false);
                yield return new WaitForSeconds(0.5f);
                
                yield return DialogueWindow.StartDialogue(new[]
                {
                    "/y0Ха-ха.", 
                    "/n0Почему ты не бьешся?",
                    "/y0Глупый нуб на спавне не работает ПВП.",
                    "/n0Черт...",
                    "/y0Тебе никогда меня не одолеть.",
                }, false);

                // (Подлетает Нотч)
                _notch.transform.localPosition = transform.position.x < Player.Instance.transform.position.x 
                    ? new Vector3(-1.01000023f,0,0) : new Vector3(1.01000023f,0,0);
                _notch.GetComponentInChildren<SpriteRenderer>().flipX = GetComponent<SpriteRenderer>().flipX;
                _notch.SetActive(true);

                yield return new WaitForSeconds(1.5f);
                GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;

                yield return new WaitForSeconds(1f);
            
                yield return DialogueWindow.StartDialogue(new[]
                {
                    "/t0Я тут вас подслушал.",
                    "/y0Эй лысая башка дай пирожка.",
                    "/t0Херобринка, спорим что Нубик победит тебя.",
                    "/y0Ха-ха, я даже время не буду тратить.*У него нет шансов.",
                    "/t0Спорим на желание.",
                    "/y0Ха-ха.",
                    "/y0Тогда пусть он сначала одолеет сильнейших игроков сервера.",
                    "/y0Тогда я с ним сражусь.*Но у него не получится.",
                    "/t0Нубик, я верю в тебя.",
                }, false);
            
                // (Улетает)
                _notch.GetComponent<Animator>().Play("Notch Fly Up");
                yield return new WaitForSeconds(1f);
                _notch.SetActive(false);
                
                GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
            
                yield return DialogueWindow.StartDialogue(new[]
                {
                    "/n0И что это было?.",
                    "/y0Тебе не когда не одолеть <sprite=0><sprite=1><sprite=2><sprite=4><sprite=3>.",
                }, false);
                
                Stats.Instance.Data[5] = 1;
                Player.Instance.enabled = true;
            }
            else
            {
                Player.Instance.enabled = false;
                var message = "/y0Еще ";

                if (Stats.Instance.Data[2] == 1)
                {
                    message = "/y0<color=red>Еще</color> ";
                }
                
                if (Stats.Instance.Data[33] == 0)
                {
                    message += "<sprite=0>";
                    message += "<sprite=1>";
                    message += "<sprite=2>";
                }
                
                if (Stats.Instance.Data[31] == 0)
                {
                    message += "<sprite=3>";
                }
                
                if (Stats.Instance.Data[32] == 0)
                {
                    message += "<sprite=4>";
                }
                
                message += ".";
                
                yield return DialogueWindow.StartDialogue(new[]
                {
                    message,
                }, false);
                
                Player.Instance.enabled = true;
            }
        }
        else
        {
            Player.Instance.enabled = false;

            if (Stats.Instance.Data[36] == 0)
            {
                if (!_isSpeak)
                {
                    yield return DialogueWindow.StartDialogue(new string[]
                    {
                        "/y0У тебя получилось?",
                    }, false);

                    yield return new WaitForSeconds(1);
            
                    yield return DialogueWindow.StartDialogue(new string[]
                    {
                        "/y0...",
                        "/y0Хорошо, я сражусь стобой.",
                    }, false);
                }
            
                _isSpeak = true;
            
                yield return SelectWindow.StartSelect("Сразится?", "Да", "Нет");

                if (!SelectWindow.IsRight)
                {
                    StartBattleScreen.Transition("Herobrine");
                }
                else
                {
                    Player.Instance.enabled = true;
                }
            }
            else
            {
                yield return SelectWindow.StartSelect("Сразится?", "Да", "Нет");

                if (!SelectWindow.IsRight)
                {
                    StartBattleScreen.Transition("Herobrine");
                }
                else
                {
                    Player.Instance.enabled = true;
                }
            }
        }
    }
}
