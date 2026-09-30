using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Hacker : Enemy
{
    [SerializeField]
    private AudioClip _theme;

    [SerializeField]
    private GameObject[] _attacks;
    
    private IEnumerator Start()
    {
        Actions = new List<string>()
        {
            "Оценить",
            "Восхищаться",
            "Жаловаться"
        };

        Answers = new List<string>()
        {
            "Хакер АТК ??? ЗЩ ???. Величайший хакер всех времён (по его словам).",
            "(Кажется это не работает.)",
            "Вы жалуетесь на читы.*Но никто не пришел... ",
        };
        
        Relationship = -20;
        
        BattleManager.Instance.Enemies.Add(this);
        BattleManager.Instance.IsRun = true;
        MusicManager.Instance.Play(_theme);
        // BattleManager.Instance.WriteMessage("Хакер преграждает вам путь");

        while (true)
        {
            BattleManager.Instance.Message = "Хакер преграждает вам путь";
            yield return BattleManager.Instance.AwaitStartTurn();
            yield return new WaitUntil(() => BattleManager.Instance.IsEnemyTurn);
            Heart.Instance.gameObject.SetActive(false);
            yield return BattleManager.Instance.AwaitSetFrame(new Vector2(4.7f, 1.75f));
            yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), "Я величайший Хакер всех времен!");
            Heart.Instance.gameObject.SetActive(true);
            Heart.Instance.enabled = true;
            var attack = Instantiate(_attacks[0]).GetComponent<SwordHackerAttack>();
            yield return attack.AwaitProcess();
            yield return new WaitForSeconds(1888);
            Heart.Instance.enabled = false;
            Heart.Instance.gameObject.SetActive(false);
            //yield return new WaitForSeconds(2);
        }
    }
}
