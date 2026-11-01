using System.Collections;
using UnityEngine;

public sealed class GrumSpawn : BannedUsable
{
    [SerializeField]
    private Sprite _notPumpkin;
    
    protected override int _id => 16;
    protected override string _message => "/g0Я урод.";
    private bool _isSpeak;
    
    protected override void Start()
    {
        base.Start();

        if (Stats.Instance.Data[8] == 1)
        {
            StartCoroutine(AwaitUse());
            return;
        }
        
        if (Stats.Instance.Data[32] == 2)
        {
            gameObject.SetActive(false);
        }

        if (Stats.Instance.Data[32] == 1)
        {
            GetComponent<SpriteRenderer>().sprite = _notPumpkin;
        }
    }

    public override IEnumerator AwaitUse()
    {
        if (Stats.Instance.Data[8] == 1)
        {
            Player.Instance.enabled = false;
            
            yield return DialogueWindow.StartDialogue(new string[]
            {
                "/g1Спасибо.",
                "/g1Благодаря тебе я понял что мне не нужно прятать свой скин!"
            }, false);
            
            Stats.Instance.Data[8] = 0;
            Player.Instance.enabled = true;
            yield break;
        }
        
        if (Stats.Instance.Data[32] == 1)
        {
            Player.Instance.enabled = false;
            
            yield return DialogueWindow.StartDialogue(new string[]
            {
                "/g1Спасибо.",
                "/g1Благодаря тебе я понял что мне не нужно прятать свой скин!"
            }, false);
            
            Player.Instance.enabled = true;
            yield break;
        }
        
        if (Stats.Instance.Data[6] == 1)
        {
            yield return AwaitBan();
        }
        else if (Stats.Instance.Data[5] == 1)
        {
            Player.Instance.enabled = false;

            if (Stats.Instance.Data[32] == 0)
            {
                yield return DialogueWindow.StartDialogue(new string[]
                {
                    "/g0Ты хочешь сразится?",
                }, false);
            
                yield return SelectWindow.StartSelect("Сразится?", "Да", "Нет");
            
                if (!SelectWindow.IsRight)
                {
                    yield return DialogueWindow.StartDialogue(new string[]
                    {
                        "/g0Ну давай."
                    }, false);
                
                    StartBattleScreen.Transition("Grum");
                }
                else
                {
                    Player.Instance.enabled = true;
                }
            }
            else
            {
                yield return DialogueWindow.StartDialogue(new string[]
                {
                    "/g0Хочешь взять реванш?",
                }, false);
                
                yield return SelectWindow.StartSelect("Сразится?", "Да", "Нет");
            
                if (!SelectWindow.IsRight)
                {
                    StartBattleScreen.Transition("Grum");
                }
                else
                {
                    Player.Instance.enabled = true;
                }
            }
        }
        else
        {
            Player.Instance.enabled = false;

            if (_isSpeak)
            {
                yield return DialogueWindow.StartDialogue(new string[]
                {
                    "/n0Не грусти.",
                    "/g0Спасибо.",
                }, false);
            }
            else
            {
                yield return DialogueWindow.StartDialogue(new string[]
                {
                    "/g0Я потратил кучу денег на лицензионный ключ... ",
                    "/g0чтобы ходить с крутым скином.",
                    "/g0Но кто-то зарегистрировал мой ник в пиратской...",
                    "/g0системе скинов за 0 рублей и теперь мне приходится...",
                    "/g0носить тыкву чтобы никто не видел мой уродский скин.",
                    "/n0Оу сочувствую.",
                }, false);
                
                _isSpeak = true;
            }
            
            Player.Instance.enabled = true;
        }
    }
}
