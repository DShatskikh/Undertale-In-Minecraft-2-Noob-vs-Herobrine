using System.Collections;
using UnityEngine;

public sealed class TrioSpawn : BannedUsable
{
    [SerializeField]
    private GameObject _fivfiv, _nastya, _sheep;
    
    protected override int _id => 17;
    protected override string _message => "/i0А потом он говорит дурак ты мы айсберг обплыли.";
    
    private bool _isSpeak;
    
    protected override void Start()
    {
        base.Start();

        if (Stats.Instance.Data[8] == 1)
        {
            StartCoroutine(AwaitUse());
            return;
        }
        
        if (Stats.Instance.Data[33] == 2)
        {
            _fivfiv.SetActive(false);
        }
        
        if (Stats.Instance.Data[34] == 2)
        {
            _nastya.SetActive(false);
        }
        
        if (Stats.Instance.Data[35] == 2)
        {
            _sheep.SetActive(false);
        }
        
        if (Stats.Instance.Data[33] == 2 && Stats.Instance.Data[34] == 2 && Stats.Instance.Data[35] == 2)
        {
            gameObject.SetActive(false);
        }
    }
    
    public override IEnumerator AwaitUse()
    {
        if (Stats.Instance.Data[8] == 1)
        {
            Player.Instance.enabled = false;
            
            yield return DialogueWindow.StartDialogue(new string[]
            {
                "/j0Прикольная была битва.",
                "/n0Ага...",
            }, false);
            
            Player.Instance.enabled = true;
            
            Stats.Instance.Data[8] = 0;
            yield break;    
        }
        
        if (Stats.Instance.Data[33] == 2 || Stats.Instance.Data[34] == 2 || Stats.Instance.Data[35] == 2)
        {
            Player.Instance.enabled = false;

            if (Stats.Instance.Data[33] == 1)
            {
                yield return DialogueWindow.StartDialogue(new string[]
                {
                    "/n0 Сори.",
                    "/i0Да ничего это просто игра.",
                }, false);
            }
            else if (Stats.Instance.Data[34] == 1)
            {
                yield return DialogueWindow.StartDialogue(new string[]
                {
                    "/n0 Сори.",
                    "/j0Да ничего это просто игра.",
                }, false);
            }
            else if (Stats.Instance.Data[35] == 1)
            {
                yield return DialogueWindow.StartDialogue(new string[]
                {
                    "/n0 Сори.",
                    "/o0Да ничего это просто игра.",
                }, false);
            }
            
            Player.Instance.enabled = true;
        }
        else if (Stats.Instance.Data[33] == 1)
        {
            Player.Instance.enabled = false;
            
            yield return DialogueWindow.StartDialogue(new string[]
            {
                "/j0 Нубик!",
                "/n0Что делаете?",
                "/j0Да так зависаем.",
            }, false);
            
            Player.Instance.enabled = true;
        }
        else if (Stats.Instance.Data[33] == 3)
        {
            yield return SelectWindow.StartSelect("Сразится?", "Да", "Нет");

            if (!SelectWindow.IsRight)
            {
                StartBattleScreen.Transition("Nastya", "Fivfiv", "Sheep");
            }
            else
            {
                Player.Instance.enabled = true;
            }
        }
        else if (Stats.Instance.Data[6] == 1)
        {
            yield return AwaitBan();
        }
        else if (Stats.Instance.Data[5] == 1)
        {
            Player.Instance.enabled = false;

            if (!_isSpeak)
            {
                yield return DialogueWindow.StartDialogue(new string[]
                {
                    "/n0Почему Херобрин считает вас сильными?",
                    "/j0Мы деремся трое на одного.",
                    "/n0Отстой.",
                    "/i0Ты хочешь подраться?",
                }, false);
            }
            
            _isSpeak = true;
            
            yield return SelectWindow.StartSelect("Сразится?", "Да", "Нет");

            if (!SelectWindow.IsRight)
            {
                StartBattleScreen.Transition("Nastya", "Fivfiv", "Sheep");
            }
            else
            {
                Player.Instance.enabled = true;
            }
        }
        else
        {
            Player.Instance.enabled = false;
            yield return DialogueWindow.StartDialogue(new string[] { 
                "/n0Что делаете?",
                "/i0Да так зависаем.", 
            }, false);
            Player.Instance.enabled = true;
        }
    }
}
