using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Video;
using Random = UnityEngine.Random;

public sealed class Theatre : MonoBehaviour
{
    [SerializeField]
    private GameObject _wisman;
    
    [SerializeField]
    private GameObject _blackout;
    
    [SerializeField]
    private GameObject _disco;
    
    [SerializeField]
    private Transform _chicken;
    
    [SerializeField]
    private Transform[] _chickenPoints;

    [SerializeField]
    private Transform[] _mobs;
    
    [SerializeField]
    private ChairTheatre _chair;
    
    [SerializeField]
    private GameObject _fakePlayer;

    [SerializeField]
    private VideoPlayer _videoPlayer;

    [SerializeField]
    private GameObject _videoBlackout;

    [SerializeField]
    private AudioClip _theme;
    
    [SerializeField]
    private AudioClip _kyznechikSFX;
    
    [SerializeField]
    private AudioClip _yyySFX;
    
    [SerializeField]
    private UIDocument _endGameUI;
    
    private bool _isChicken;
    
    private IEnumerator Start()
    {
        MusicManager.Instance.Stop();
        
        foreach (var mob in _mobs)
        {
            mob.gameObject.SetActive(false);
        }
        
        yield return null;
        Player.Instance.gameObject.SetActive(false);
        _isChicken = true;
        _chicken.GetComponent<Animator>().Play("Stay");
        yield return new WaitForSeconds(1);
        _chicken.transform.localScale = _chicken.transform.localScale.SetX(-1);
        _chicken.GetComponent<Animator>().Play("ChickenStep");
        yield return AwaitMove(_chicken, _chickenPoints[0].position);
        _chicken.GetComponent<Animator>().Play("Stay");
        yield return new WaitForSeconds(2f);
        _chicken.transform.localScale = _chicken.transform.localScale.SetX(1);
        _chicken.GetComponent<Animator>().Play("ChickenStep");
        yield return AwaitMove(_chicken, _chickenPoints[1].position);
        _chicken.GetComponent<Animator>().Play("Stay");
        yield return new WaitForSeconds(1f);
        yield return AwaitMove(_chicken, _chickenPoints[2].position);
        yield return new WaitForSeconds(3f);
        
        _wisman.SetActive(true);
        _blackout.SetActive(false);
        
        yield return new WaitForSeconds(3f);
        
        yield return DialogueWindow.StartDialogue(new []
        {
            "/w0А?<waitfor=1>*Пришла только Курица?",
            "/w0Возможно мне не стоило делать такой сложный квест.",
            //"/w0C этими сундуками <waitfor=0.5>и золотыми яблоками.",
            //"/w0И одним приглашением для всех <waitfor=0.5>в одном из сундуков.",
            "/w0Стоп.<waitfor=1.0>*Кажется еще кто-то идёт."
        }, true);

        _isChicken = false;

        StartCoroutine(AwaitMove(Player.Instance.transform, new Vector3(4.03854799f, -4.52383471f, 0)));
        MusicManager.Instance.Play(_theme);
        
        foreach (var mob in _mobs)
        {
            StartCoroutine(AwaitMoveMob(mob));
            yield return new WaitForSeconds(0.15f);
        }
        
        yield return new WaitForSeconds(2.5f);
        _chair.GetComponent<SpriteRenderer>().color = Color.white;
        Player.Instance.gameObject.SetActive(true);
        Player.Instance.enabled = false;
        Player.Instance.GetComponent<Collider2D>().enabled = false;
        var position = Player.Instance.transform.position;
        Player.Instance.transform.position = position.AddY(-1);
        yield return AwaitMove(Player.Instance.transform, position);
        Player.Instance.GetComponent<Collider2D>().enabled = true;
        Player.Instance.enabled = true;
    }

    private void Update()
    {
        if (_isChicken)
            Player.Instance.transform.position = _chicken.position;
        
        if (_videoPlayer.canSetDirectAudioVolume)
            _videoPlayer.SetDirectAudioVolume(0, AudioListener.volume);
    }

    private IEnumerator AwaitMove(Transform target, Vector3 position, float speed = 1.5f)
    {
        while (target.position != position)
        {
            target.position = Vector3.MoveTowards(target.position, position, Time.deltaTime * speed);
            yield return null;
        }
    }
    
    private IEnumerator AwaitMoveMob(Transform target)
    {
        var startFlipX = target.GetComponent<SpriteRenderer>().flipX;
        target.gameObject.SetActive(transform);
        var position = target.position;
        target.position = new Vector3(4.03854799f + Random.Range(-1f, 1f), -5.52383471f, 0);
        target.GetComponent<SpriteRenderer>().flipX = target.position.x > position.x;
        yield return AwaitMove(target, new Vector3(target.position.x, position.y), 10);
        yield return AwaitMove(target, position, 3f);
        target.GetComponent<SpriteRenderer>().flipX = startFlipX;
    }

    private IEnumerator AwaitMoveVillager(Transform villager)
    {
        var position = new Vector3(4f + Random.Range(-0.55f, 0.55f), villager.position.y, 0);
        villager.GetComponent<SpriteRenderer>().flipX = villager.position.x > position.x;
        yield return AwaitMove(villager, position, 10);
        position = new Vector3(villager.position.x, 5.25f + Random.Range(-0.5f, 0.5f), 0);
        yield return AwaitMove(villager, position, 5f);
    }
    
    public void StartShow()
    {
        StartCoroutine(AwaitShow());
    }

    private IEnumerator AwaitShow()
    {
        _fakePlayer.transform.position = Player.Instance.transform.position;
        _fakePlayer.SetActive(true);
        Player.Instance.gameObject.SetActive(false);
        StartCoroutine(AwaitMove(Player.Instance.transform, new Vector3(4f, 3f, 0)));
        yield return AwaitMove(_fakePlayer.transform, _chair.transform.position.AddY(0.443682f));
        _fakePlayer.GetComponent<SpriteRenderer>().flipX = false;
        _chair.GetComponent<SpriteRenderer>().color = new Color32(116, 116, 116, 255);
        yield return new WaitForSeconds(1f);
        
        yield return DialogueWindow.StartDialogue(new []
        {
            "/w0Добрый вечер, дамы и господа, <waitfor=0.25>мобы и... <waitfor=0.25>Херобрин.",
            "/w0Ладно!<waitfor=0.5>*Разогрев!",
            "/w0Кто здесь умирал от крипера?",
            "/b0Я умирал.",
            "/i0Да было такое.",
            "/w0Кто терял вещи в лаве?",
            "/g1Я.",
            "/f0У меня такое было.",
        }, true);

        MusicManager.Instance.Stop();
        
        yield return DialogueWindow.StartDialogue(new []
        {
            "/w0Кто... <waitfor=0.25>кхм... <waitfor=0.25>убивал жителей ради изумрудов?",
        }, true);
        
        // Зал молчит.
        
        MusicManager.Instance.PlaySound(_kyznechikSFX);
        yield return new WaitForSeconds(1f);

        yield return DialogueWindow.StartDialogue(new []
        {
            "/v0Это оскорбительно.",
            "/w0Это была шутка!<waitfor=0.5>*Ну, половина.<waitfor=0.5>*Ну, четверть.",
            "/w0Ладно, <waitfor=0.5>это мой обычный геймплей.",
            "/w0Но мы же здесь, чтобы смеяться над жестокостью.",
            "/n0Я не смеюсь.*Мне страшно.",
            "/w0Тогда другия шутка.",
        }, true);

        MusicManager.Instance.Play(_theme);
        
        yield return DialogueWindow.StartDialogue(new []
        {
            "/w0Шутка про ведьму.",
            "/w0Ведьма — это такой моб, который кидает зелья.",
            "/w0Яда.<waitfor=0.25>*Слабости.<waitfor=0.25>*И...",
            "/q0И исцеления.*Если тебе повезёт.<waitfor=0.25>*Но тебе не везёт.",
            "/q0Никогда.<waitfor=0.5>*Потому что ты — плохой человек.",
            "/w0Я.<waitfor=0.25>.<waitfor=0.25>. я не плохой!<waitfor=0.5>*Я... <waitfor=0.25>контентный!<waitfor=0.5>*Это... <waitfor=0.25>развлечение!",
            "/w0Люди смотрят!<waitfor=0.5>*Им нравится!",
            "/b0Чувак, <waitfor=0.5>ты худший.",
            "/w0Но<waitfor=0.25>.<waitfor=0.25>.<waitfor=0.25>. но я же<waitfor=0.25>.<waitfor=0.25>.<waitfor=0.25>. я же просто шучу<waitfor=0.25>.<waitfor=0.25>.<waitfor=0.25>.",
        }, true);
        
        MusicManager.Instance.PlaySound(_yyySFX);
        
        yield return DialogueWindow.StartDialogue(new []
        {
            "/s0<b>Ууу.",
            "/i0Я чуть не умер от кринжа.",
            "/j0Это ужасно.",
            "/w0Я вернусь с армией из 10000 жителей.<waitfor=0.25>*И вы все поплатитесь!",
            "/v0Хватай его.",
            "/w0Что?!*Нет!",
        }, true);
        
        //_wisman.GetComponent<SpriteRenderer>().flipX = true;
        //yield return AwaitMove(_wisman.transform, _wisman.transform.position.AddX(-7.5f), 3);

        for (int i = 16; i < _mobs.Length; i++)
        {
            StartCoroutine(AwaitMoveVillager(_mobs[i]));
        }
        
        yield return new WaitForSeconds(3f);
        
        _wisman.transform.eulerAngles = new Vector3(0, 0, 90);
        _wisman.transform.parent = _mobs[19];
        _wisman.transform.localPosition = new Vector3(0.47f, 1.25f, 0);
        _wisman.GetComponent<SpriteRenderer>().sortingOrder = 1;
        
        for (int i = 16; i < _mobs.Length; i++)
        {
            StartCoroutine(AwaitMove(_mobs[i], new Vector3(_mobs[i].position.x, -3.21f, 0), 3));
        }
        
        yield return new WaitForSeconds(5f);
        
        yield return DialogueWindow.StartDialogue(new []
        {
            "/l0Что это было?",
        }, true);
        
        yield return new WaitForSeconds(1f);
        
        _chicken.transform.localScale = _chicken.transform.localScale.SetX(-1);
        _chicken.GetComponent<Animator>().Play("ChickenStep");
        yield return AwaitMove(_chicken, _chickenPoints[3].position);
        _chicken.transform.localScale = _chicken.transform.localScale.SetX(1);
        yield return AwaitMove(_chicken, _chickenPoints[4].position);
        _chicken.GetComponent<Animator>().Play("Stay");

        yield return AwaitMove(_chicken, _chickenPoints[5].position, 3);
        yield return new WaitForSeconds(1f);

        _chicken.GetComponent<Animator>().Play("Chicken Dance");
        _videoPlayer.Play();
        MusicManager.Instance.Stop();
        yield return new WaitForSeconds(7f);
        _disco.SetActive(true);
        
        yield return new WaitForSeconds(5f);
        
        yield return DialogueWindow.StartDialogue(new []
        {
            "/b0О боже, это прекрасно!",
            "/j1Я плачу.",
            "/i0Это лучшее что я видел в своей жизни.",
        }, true);
        
        yield return new WaitForSeconds(2f);
        
        _videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
        _videoPlayer.targetCamera = Camera.main;
        _videoBlackout.SetActive(true);
        HideScreen.Instance.gameObject.SetActive(false);
        
        yield return new WaitForSeconds(7f);

        _endGameUI.gameObject.SetActive(true);

        _endGameUI.rootVisualElement.Q<Button>("ExitButon").clickable.clicked += Exit;

        while (!InputManager.Instance.IsSubmitDown && _videoPlayer.isPlaying)
        {
            yield return null;
        }

        Exit();
    }

    private void Exit()
    {
        Meta.Instance.IsEndTheatre = true;
        Stats.Instance.Reset();
        SceneManager.LoadScene("Menu");
    }
}
