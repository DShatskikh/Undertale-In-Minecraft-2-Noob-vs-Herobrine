using System.Collections;

public sealed class HackerSpawn : BannedUsable
{
    protected override int _id => 15;
    protected override string _message => "/h0Хочешь со мной сразится?";

    private bool _isSpeak;
    
    protected override void Start()
    {
        base.Start();
        
        if (Stats.Instance.Data[31] == 1)
        {
            gameObject.SetActive(false);
        }
    }

    public override IEnumerator AwaitUse()
    {
        if (Stats.Instance.Data[6] == 1)
        {
            yield return AwaitBan();
        }
        else if (Stats.Instance.Data[5] == 1) // Поговорили с Херобрином
        {
            Player.Instance.enabled = false;

            if (Stats.Instance.Data[31] == 0)
            {
                yield return SelectWindow.StartSelect("Ты решил сразится?", "Да", "Нет");

                if (!SelectWindow.IsRight)
                {
                    yield return DialogueWindow.StartDialogue(new string[]
                    {
                        "/h0Кажется я забыл представится.",
                        "/h0Я величайший хакер в истории!",
                        "/h0Узри же силу читов!!!",
                    }, true);
                
                    StartBattleScreen.Transition("Hacker");
                }
                else
                {
                    yield return DialogueWindow.StartDialogue(new string[]
                    {
                        "/h0Жаль.",
                    }, true);
                
                    Player.Instance.enabled = true;
                }
            }
            else if (Stats.Instance.Data[31] == 1)
            {
                
            }
            else
            {
                yield return DialogueWindow.StartDialogue(new string[]
                {
                    "/h0Хочешь взять реванш?",
                }, true);
                
                yield return SelectWindow.StartSelect("Сразится?", "Да", "Нет");

                if (!SelectWindow.IsRight)
                {
                    StartBattleScreen.Transition("Hacker");
                }
                else
                {
                    yield return DialogueWindow.StartDialogue(new string[]
                    {
                        "/h0Жаль.",
                    }, true);
                
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
                    "/h0Пазязя.",
                    "/n0Нет!",
                }, true);
            }
            else
            {
                yield return DialogueWindow.StartDialogue(new string[]
                {
                    "/h0Хочешь со мной сразится?",
                    "/h0Я одолею тебя своими читами.",
                    "/n0Эй, так не честно!",
                    "/h0Ну пожалуйста я уже всех победил.*Мне скучно.",
                    "/n0Нет.",
                }, true);

                _isSpeak = true;
            }
           
            Player.Instance.enabled = true;
        }
    }
}
