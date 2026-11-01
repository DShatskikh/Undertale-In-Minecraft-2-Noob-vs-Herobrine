using System.Collections;
using UnityEngine;

public sealed class HeroStand : BannedUsable
{
    [SerializeField]
    private GameObject _companionContainer;
    
    protected override int _id => 19;
    protected override string _message => "/e0Куда все пропали.";
    protected override void Start()
    {
        base.Start();

        if (Stats.Instance.Data[37] == 1 || Stats.Instance.Data[37] == 2)
        {
            gameObject.SetActive(false);
        }
        
        if (Stats.Instance.Data[6] == 1)
        {
            _companionContainer.SetActive(false);
        }
    }

    public override IEnumerator AwaitUse()
    {
        var view = GetComponent<SpriteRenderer>();
        
        if (Stats.Instance.Data[6] == 1)
        {
            yield return AwaitBan();
        }
        else if (Stats.Instance.Data[37] == 0)
        {
            Player.Instance.enabled = false;
            
            yield return DialogueWindow.StartDialogue(new []
            {
                "/e0Ну давай-давай нападай.",
                "/s0Ну давай-давай нападай.",
                "/e0Ну давай-давай нападай.",
                "/s0Ну давай-давай нападай.",
                "/n0Эй я хочу сразится с чудищем!",
            }, false);

            view.flipX = true;
            
            yield return DialogueWindow.StartDialogue(new []
            {
                "/e0Подожди когда мы закончим.",
            }, false);
            
            view.flipX = false;
            
            yield return DialogueWindow.StartDialogue(new []
            {
                "/e0Ну давай-давай нападай.",
                "/s0Ну давай-давай нападай.",
            }, false);
            
            yield return DialogueWindow.StartDialogue(new []
            {
                "/n0Эй вы так никогда не начнёте.",
            }, false);
            
            view.flipX = true;
            
            yield return DialogueWindow.StartDialogue(new []
            {
                "/e0Ладно давай если ты одолеешь меня, то я тебе уступлю.",
                "/n0Может я просто сражусь первым?",
                "/e0Нет.",
            }, false);
            
            do
            {
                yield return SelectWindow.StartSelect("Напасть?", "Напасть", "Ну давай-давай нападай");

                if (SelectWindow.IsRight)
                {
                    yield return DialogueWindow.StartDialogue(new []
                    {
                        "/n0Ну давай-давай нападай.",
                        "/e0Ну давай-давай нападай.",
                        "/n0Ну давай-давай нападай.",
                        "/e0Ну давай-давай нападай.",
                    }, false);
                }
            } while (SelectWindow.IsRight);
            
            StartBattleScreen.Transition("Hero");
        }
        else
        {
            StartBattleScreen.Transition("Hero");
        }
    }
}
