using System.Collections;
using UnityEngine;

public sealed class AdminHome : Usable
{
    [SerializeField]
    private InputHint _hint;
    
    private bool _isComplete;
    
    private IEnumerator Start()
    {
        if (Stats.Instance.Data[70] == 1 || Meta.Instance.IsCompleteAdminTutorial)
        {
            gameObject.SetActive(false);
        }
        else if (Stats.Instance.Data[70] == 2)
        {
            Player.Instance.enabled = false;
            GetComponent<SpriteRenderer>().flipX = transform.position.x > Player.Instance.transform.position.x;
            Player.Instance.SetFlex(!GetComponent<SpriteRenderer>().flipX);

            yield return null;
            
            yield return DialogueWindow.StartDialogue(new[]
            {
                "/n0Обучение это конечно круто.",
                "/n0Но почему ты стоял и смотрел как ломают мой дом?",
                "/a0А что такое?",
                "/n0Это же гриферство!",
                "/a0Если не запревачено, то ломать можно.*Почитай правила.",
            }, false);
            
            _isComplete = true;
            Meta.Instance.IsCompleteAdminTutorial = true;
            Player.Instance.enabled = true;
        }
        
        Stats.Instance.Data[70] = 1;
    }

    public override IEnumerator AwaitUse()
    {
        Player.Instance.enabled = false;
        GetComponent<SpriteRenderer>().flipX = transform.position.x > Player.Instance.transform.position.x;
        
        if (_isComplete)
        {
            yield return DialogueWindow.StartDialogue(new[]
            {
                "/n0Можешь вернуть дом?",
                "/a0Не.",
            }, false);
            
            Player.Instance.enabled = true;
        }
        else
        {
            yield return SelectWindow.StartSelect("Пройти обучение?", "Да", "Нет");
            
            if (!SelectWindow.IsRight)
            {
                _hint.gameObject.SetActive(false);
                StartBattleScreen.Transition("Admin");
            }
            else
            {
                Player.Instance.enabled = true;
            }
        }
    }
}
