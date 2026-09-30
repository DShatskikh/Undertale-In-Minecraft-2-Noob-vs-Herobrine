using System;
using System.Collections;
using Febucci.TextAnimatorForUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Shop : MonoBehaviour
{
    [SerializeField]
    private AudioSource _selectSFX;
    
    [SerializeField]
    private AudioSource _squeakSFX;
    
    [SerializeField]
    private AudioSource _buySFX;
    
    [SerializeField]
    private TypewriterComponent _mainLabel;

    [SerializeField]
    private TypewriterComponent _messageLabel;
    
    [SerializeField]
    private TypewriterComponent _buyMessageLabel;
    
    [SerializeField]
    private TypewriterComponent _speakMessageLabel;
    
    [SerializeField]
    private GameObject _main;
    
    [SerializeField]
    private GameObject _buy;
    
    [SerializeField]
    private GameObject _speak;
    
    [SerializeField]
    private GameObject _message;
    
    [SerializeField]
    private GameObject _buyConfirm;

    private bool _isBuy;
    private bool _isNotEnoughMoney;
    private int _previousIndex;
    
    public bool IsMain;
    public int MainIndex;
    public bool IsBuy;
    public int BuyIndex;
    public bool IsBuyConfirm;
    public int BuyConfirmIndex;
    public bool IsSpeak;
    public int SpeakIndex;
    public string MessageText;
    
    private void Start()
    {
        Heart.Instance.enabled = false;
        IsMain = true;
        SelectScreen(0);
    }

    private void Buy(string id)
    {
        var price = id switch
        {
            "Steak" => 5,
            "HealthPotion" => 9,
            "GoldApple" => 22,
        };
        
        if (Stats.Instance.Money >= price)
        {
            _isBuy = true;
            _buySFX.Play();
            Stats.Instance.Money -= price;
        }
        else
        {
            _isNotEnoughMoney = true; 
        }
    }
    
    private void Update()
    {
        if (IsMain)
        {
            if (InputManager.Instance.IsSubmitDown)
            {
                if (MainIndex == 0)
                {
                    SelectScreen(1);
                }
                else if (MainIndex == 1)
                {
                    MessageText = "Прости, но я ничего не продаю";
                    SelectScreen(3);
                    StartCoroutine(AwaitMessage());
                }
                else if (MainIndex == 2)
                {
                    SelectScreen(2);
                }
                else if (MainIndex == 3)
                {
                    CoroutineRunner.Instance.StartCoroutine(AwaitExit());
                }
            }
            else if (InputManager.Instance.VerticalDown)
            {
                if (InputManager.Instance.Vertical < 0)
                {
                    MainIndex++;
                    
                    if (MainIndex > 3)
                        MainIndex = 3;
                    else
                        _squeakSFX.Play();
                }
                else if (InputManager.Instance.Vertical > 0)
                {
                    MainIndex--;
                    
                    if (MainIndex < 0)
                        MainIndex = 0;
                    else
                        _squeakSFX.Play();
                }
            }
        
            Heart.Instance.transform.position = MainIndex switch
            {
                0 => new Vector3(2.88400006f, -0.75f, 0),
                1 => new Vector3(2.88400006f, -1.57f, 0),
                2 => new Vector3(2.88400006f, -2.45f, 0),
                3 => new Vector3(2.88400006f, -3.27999997f, 0),
            };
        }
        else if (IsBuy)
        {
            if (InputManager.Instance.IsSubmitDown)
            {
                if (BuyIndex == 4)
                {
                    SelectScreen(0);
                }
                else
                {
                    SelectScreen(4);
                }
            }
            else if (InputManager.Instance.IsCancelDown)
            {
                SelectScreen(0);
            }
            else if (InputManager.Instance.VerticalDown)
            {
                if (InputManager.Instance.Vertical < 0)
                {
                    BuyIndex++;
                    
                    if (BuyIndex > 4)
                        BuyIndex = 4;
                    else
                        _squeakSFX.Play();
                }
                else if (InputManager.Instance.Vertical > 0)
                {
                    BuyIndex--;
                    
                    if (BuyIndex < 0)
                        BuyIndex = 0;
                    else
                        _squeakSFX.Play();
                }
            }
            
            Heart.Instance.transform.position = BuyIndex switch
            {
                0 => new Vector3(-5.9000001f, -0.777999997f, 0),
                1 => new Vector3(-5.9000001f, -1.591f, 0),
                2 => new Vector3(-5.9000001f, -2.385f, 0),
                3 => new Vector3(-5.9000001f, -3.238f, 0),
                4 => new Vector3(-5.9000001f, -4.066f, 0),
            };
        }
        else if (IsSpeak)
        {
            if (InputManager.Instance.IsSubmitDown)
            {
                if (SpeakIndex == 0)
                {
                    MessageText = "Реплика 1";
                    SelectScreen(3);
                    StartCoroutine(AwaitMessage());
                }
                else if (SpeakIndex == 1)
                {
                    MessageText = "Реплика 2";
                    SelectScreen(3);
                    StartCoroutine(AwaitMessage());
                }
                else if (SpeakIndex == 2)
                {
                    MessageText = "Реплика 3";
                    SelectScreen(3);
                    StartCoroutine(AwaitMessage());
                }
                else if (SpeakIndex == 3)
                {
                    MessageText = "Реплика 4";
                    SelectScreen(3);
                    StartCoroutine(AwaitMessage());
                }
                else if (SpeakIndex == 4)
                {
                    SelectScreen(0);
                }
            }
            else if (InputManager.Instance.IsCancelDown)
            {
                SelectScreen(0);
            }
            else if (InputManager.Instance.VerticalDown)
            {
                if (InputManager.Instance.Vertical < 0)
                {
                    SpeakIndex++;
                    
                    if (SpeakIndex > 4)
                        SpeakIndex = 4;
                    else
                        _squeakSFX.Play();
                }
                else if (InputManager.Instance.Vertical > 0)
                {
                    SpeakIndex--;
                    
                    if (SpeakIndex < 0)
                        SpeakIndex = 0;
                    else
                        _squeakSFX.Play();
                }
            }
            
            Heart.Instance.transform.position = SpeakIndex switch
            {
                0 => new Vector3(-5.9000001f, -0.777999997f, 0),
                1 => new Vector3(-5.9000001f, -1.591f, 0),
                2 => new Vector3(-5.9000001f, -2.385f, 0),
                3 => new Vector3(-5.9000001f, -3.238f, 0),
                4 => new Vector3(-5.9000001f, -4.066f, 0),
            };
        }
        else if (IsBuyConfirm)
        {
            if (InputManager.Instance.IsSubmitDown)
            {
                if (BuyConfirmIndex == 0)
                {
                    if (BuyIndex == 0)
                    {
                        Buy("Steak");
                    }
                    else if (BuyIndex == 1)
                    {
                        Buy("HealthPotion");
                    }
                    else if (BuyIndex == 2)
                    {
                        Buy("GoldApple");
                    }
                    
                    SelectScreen(1);
                }
                else
                {
                    SelectScreen(1);
                }
            }
            else if (InputManager.Instance.IsCancelDown)
            {
                SelectScreen(1);
            }
            else if (InputManager.Instance.VerticalDown)
            {
                if (InputManager.Instance.Vertical < 0)
                {
                    BuyConfirmIndex++;
                    
                    if (BuyConfirmIndex > 1)
                        BuyConfirmIndex = 1;
                    else
                        _squeakSFX.Play();
                }
                else if (InputManager.Instance.Vertical > 0)
                {
                    BuyConfirmIndex--;
                    
                    if (BuyConfirmIndex < 0)
                        BuyConfirmIndex = 0;
                    else
                        _squeakSFX.Play();
                }
            }
            
            Heart.Instance.transform.position = BuyConfirmIndex switch
            {
                0 => new Vector3(2.88400006f,-2.35100007f,0),
                1 => new Vector3(2.88400006f, -3.045f, 0)
            };
        }
    }
    
    private void WriteMessageMain(string message)
    {
        _mainLabel.ShowText(message);
        _mainLabel.StartShowingText();
    }

    /*
     * 0 - Main
     * 1 - Buy
     * 2 - Speak
     * 3 - Message
     * 4 - Buy Confirm
     */
    public void SelectScreen(int index)
    {
        if (index != 0)
        {
            IsMain = false;
            _main.SetActive(false);
        }
        
        if (index != 1)
        {
            IsBuy = false;
            
            if (index != 4)
                _buy.SetActive(false);
        }
        
        if (index != 2)
        {
            IsSpeak = false;
            _speak.SetActive(false);
        }
        
        if (index != 3)
        {
            _message.SetActive(false);
        }
        
        if (index != 4)
        {
            IsBuyConfirm = false;
            _buyConfirm.SetActive(false);
            _buyMessageLabel.gameObject.SetActive(true);
        }
        
        if (index == 0)
        {
            IsMain = true;
            _main.SetActive(true);
            WriteMessageMain("Это я твой лучший друг!");
            Heart.Instance.gameObject.SetActive(true);
        }
        else if (index == 1)
        {
            IsBuy = true;
            _buy.SetActive(true);
            
            if (_previousIndex != 4)
                _selectSFX.Play();

            var message = "Что тебе нужно?";
            
            if (_isBuy)
            {
                message = "Спасибо за покупку";
            }
            else if (_isNotEnoughMoney)
            {
                message = "Не хватает денег";
            }
            
            _buyMessageLabel.ShowText(message);
            _buyMessageLabel.StartShowingText();

            _isBuy = false;
            _isNotEnoughMoney = false;
        }
        else if (index == 2)
        {
            IsSpeak = true;
            _speak.SetActive(true);
            _selectSFX.Play();
            
            _speakMessageLabel.ShowText("Что тебя тревожит?");
            _speakMessageLabel.StartShowingText();
        }
        else if (index == 3)
        {
            _message.SetActive(true);
            _selectSFX.Play();
            Heart.Instance.gameObject.SetActive(false);
        }
        else if (index == 4)
        {
            IsBuyConfirm = true;
            _buyMessageLabel.gameObject.SetActive(false);
            _buyConfirm.SetActive(true);
            _selectSFX.Play();
        }
        
        _previousIndex = index;
    }
    
    private IEnumerator AwaitExit()
    {
        SelectScreen(3);
        enabled = false;
        
        MessageText = "Удачного тебе дня.";
        
        _messageLabel.ShowText(MessageText);
        _messageLabel.StartShowingText();
        yield return new WaitUntil(() => !_messageLabel.IsShowingText);
        yield return InputManager.Instance.AwaitSubmitDownOrClick();
        
        yield return SceneManager.LoadSceneAsync("Overworld", LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Overworld"));
        SceneManager.LoadScene(Stats.Instance.LevelName, LoadSceneMode.Additive);
        yield return SceneManager.UnloadSceneAsync("Shop", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        Player.Instance.transform.position = Stats.Instance.Position;
    }

    private IEnumerator AwaitMessage()
    {
        _messageLabel.ShowText(MessageText);
        _messageLabel.StartShowingText();
        yield return new WaitUntil(() => !_messageLabel.IsShowingText);
        yield return InputManager.Instance.AwaitSubmitDownOrClick();
        SelectScreen(0);
    }
}
