using System;
using System.Collections;
using Febucci.TextAnimatorForUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Intro : MonoBehaviour
{
    // Курица рассказала о планах Херобрина
    // Отнес торт в тюрьму
    // И устроил лучшую вечеринку в мире!
    // Некоторое время до этого...
    // Это Нубик он впервые зашел на сервер

    public static Intro Instance;

    [SerializeField]
    private TypewriterComponent _label;
    
    [SerializeField]
    private GameObject[] _slides;

    [SerializeField]
    private SpriteRenderer _blackout;
    
    private int _slideIndex = -1;
    
    public int ButtonIndex;
    private bool _isSkip;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Heart.Instance.enabled = false;
        Next();
    }

    private void Update()
    {
        if (_isSkip)
            return;
        
        if (InputManager.Instance.IsSubmitDown)
        {
            if (ButtonIndex == 0)
            {
                Next();
            }
            else
            {
                Skip();
            }
        }
        else if (InputManager.Instance.HorizontalDown)
        {
            if (InputManager.Instance.Horizontal > 0)
            {
                ButtonIndex = 0;
            }
            else
            {
                ButtonIndex = 1;
            }
        }

        Heart.Instance.transform.position = ButtonIndex switch
        {
            0 => new Vector3(4.94000006f, -5.37300014f, 0),
            1 => new Vector3(1.71f, -5.37300014f, 0),
        };
    }

    public void Next()
    {
        if (_isSkip)
            return;
        
        if (_slideIndex != -1)
            _slides[_slideIndex].SetActive(false);
        
        _slideIndex++;

        if (_slideIndex >= _slides.Length)
        {
            Skip();
            return;
        }
        
        _slides[_slideIndex].SetActive(true);

        var message = _slideIndex switch
        {
            0 => "Однажды Курица спасла сервер.",
            1 => "Завела новых друзей.",
            2 => "А затем устроил лучшую вечеринку в мире!",
            3 => "Некоторое время до этого...",
            4 => "Это Нубик он впервые зашел на сервер.",
            5 => "Он построил свой первый дом.",
            6 => "И просто наслаждается жизнью.",
            7 => "Но счастье не могло длится вечно...",
            8 => "Херобрин загриферил домик Нубика.",
            9 => "Потому что Нубик не умел приватить.",
            10 => "И теперь ему предстоит отомстить Херобрину.",
        };
        
        _label.ShowText(message);
        _label.StartShowingText();
    }

    public void Skip()
    {
        _isSkip = true;
        StartCoroutine(AwaitSkip());
    }

    private IEnumerator AwaitSkip()
    {
        Heart.Instance.gameObject.SetActive(false);
        _blackout.gameObject.SetActive(true);
        
        while (_blackout.color.a < 1)
        {
            _blackout.color = _blackout.color.AddA(Time.deltaTime);
            yield return null;
        }

        Stats.Instance.Data[0] = 1;
        Stats.Instance.Position = new Vector3(0.419999838f, -0.839999557f, 0);
        Stats.Instance.LevelName = "Home";
        
        SwitchingLevel.SwitchingNotWorld();
    }
}
