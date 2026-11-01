using System;
using System.Collections;
using System.Collections.Generic;
using Febucci.TextAnimatorForUnity;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
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
    private TextMeshPro _infoItemLabel;
    
    [SerializeField]
    private TextMeshPro _priceLabel;
    
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

    [SerializeField]
    private TextButton[] _mainButtons;

    [SerializeField]
    private TextButton[] _buyButtons;

    [SerializeField]
    private TextButton[] _buyConfirmButtons;

    [SerializeField]
    private TextButton[] _speakButtons;

    [SerializeField]
    private GameObject _maskDreamTrue, _maskDreamFake;
    
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

        if (Stats.Instance.Data[9] == 1)
        {
            _buyButtons[2].GetComponent<TextMeshPro>().text = "...-";
        }

        if (!Meta.Instance.IsEndEvil)
        {
            _maskDreamTrue.SetActive(false);
            _maskDreamFake.SetActive(true);
        }
        
        _mainButtons[0].Click += () => SelectScreen(1);
        _mainButtons[1].Click += SellSelect;
        _mainButtons[2].Click += () => SelectScreen(2);
        _mainButtons[3].Click += () => CoroutineRunner.Instance.StartCoroutine(AwaitExit());

        _buyButtons[0].Click += () =>
        {
            BuyIndex = 0;
            SelectScreen(4);
        };

        _buyButtons[1].Click += () =>
        {
            BuyIndex = 1;
            SelectScreen(4);
        };

        _buyButtons[2].Click += () =>
        {
            BuyIndex = 2;
            SelectScreen(4);
        };

        _buyButtons[3].Click += () =>
        {
            BuyIndex = 3;
            SelectScreen(4);
        };

        _buyButtons[4].Click += () => SelectScreen(0);

        _buyConfirmButtons[0].Click += () => BuyConfirm(0);
        _buyConfirmButtons[1].Click += () => BuyConfirm(1);

        _speakButtons[0].Click += () => StartCoroutine(AwaitSpeak(0));
        _speakButtons[1].Click += () => StartCoroutine(AwaitSpeak(1));
        _speakButtons[2].Click += () => StartCoroutine(AwaitSpeak(2));
        _speakButtons[3].Click += () => StartCoroutine(AwaitSpeak(3));
        _speakButtons[4].Click += () => SelectScreen(0);
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

    private void BuyConfirm(int index = -1)
    {
        if (index != -1)
        {
            BuyConfirmIndex = index;
        }
        
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
                _buyButtons[2].GetComponent<TextMeshPro>().text = "...-";
                Buy("GoldApple");
                Stats.Instance.Data[9] = 1;
            }
                    
            SelectScreen(1);
        }
        else
        {
            SelectScreen(1);
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
                    SellSelect();
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
            _infoItemLabel.text = BuyIndex switch
            {
                0 => $"+{Stats.GetItemHPText("Steak")}ОЗ",
                1 => $"+{Stats.GetItemHPText("Potion")}ОЗ",
                2 => (BuyIndex == 2 && Stats.Instance.Data[9] == 0) ? "Распродано." : "+20 ОЗ и +4 МАКС ОЗ",
                3 => Meta.Instance.IsEndEvil ? $"+Спидраны\n+Новый контент." : "Нет в наличии.",
                4 => $"",
            };
            
            if (InputManager.Instance.IsSubmitDown)
            {
                if (BuyIndex == 4)
                {
                    SelectScreen(0);
                }
                else
                {
                    if (BuyIndex == 3 && !Meta.Instance.IsEndEvil)
                        return;
                    
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
                if (SpeakIndex == 4)
                {
                    SelectScreen(0);
                }
                else
                {
                    StartCoroutine(AwaitSpeak());
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
                BuyConfirm();
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

    private void SellSelect()
    {
        MessageText = "Прости, я только продаю.";
        SelectScreen(3);
        StartCoroutine(AwaitMessage());
    }

    private IEnumerator AwaitSpeak(int index = -1)
    {
        // О себе - 
        // Моб-арена - 
        // Маска Хакера - 
        // Секреты -   

        if (index != -1)
        {
            SpeakIndex = index;
        }
        
        if (SpeakIndex == 0)
        {
            MessageText = "Бизнесмен, предпрениматель, филантроп.*Это всё я."; // Я просто обычный продавец на спавне
            SelectScreen(3);
            yield return AwaitMessage();
            
            MessageText = "А еще самый скромный игрок сервера.";
            SelectScreen(3);
            yield return AwaitMessage();
        }
        else if (SpeakIndex == 1)
        {
            MessageText = "Там можно заработать $ и потратить у меня в магазине.";
            SelectScreen(3);
            yield return AwaitMessage();
            
            MessageText = "Не забывай покупать лечилки, иначе ты будешь часто проигрывать.";
            SelectScreen(3);
            yield return AwaitMessage();
        }
        else if (SpeakIndex == 2)
        {
            MessageText = "Говорят что она открывает новый контент и даёт доступ к спидранам.";
            SelectScreen(3);
            yield return AwaitMessage();
            
            MessageText = "Но каждый сам должен решить готов ли он заплатить такую цену.";
            SelectScreen(3);
            yield return AwaitMessage();
        }
        else if (SpeakIndex == 3)
        {
            if (Meta.Instance.IsFoundCastle)
            {
                MessageText = "Изначально там должен был быть спавн, но <color=red>Админу</color> он не понравился.*Поэтому он построил другой.";
                SelectScreen(3);
                yield return AwaitMessage();
                
                MessageText = "Раньше на старом спавне <color=red>Админ</color> тестировал новые ивенты, предметы и мобов.*Наверное так там оказался тот сильный моб и <color=red>админский предмет</color>.";
                SelectScreen(3);
                yield return AwaitMessage();
            }
            else
            {
                MessageText = "На Старом спавне есть очень <color=yellow>дорогой</color> админский предмет который <color=red>Админ</color> там случайно оставил.";
                SelectScreen(3);
                yield return AwaitMessage();
            
                MessageText = "Многие игроки пытались его забрать.*Но он охраняется очень сильным мобом.*Поэтому он всё еще там.";
                SelectScreen(3);
                yield return AwaitMessage();
            
                MessageText = "Но кто знает может у тебя получится.*Удачи.";
                SelectScreen(3);
                yield return AwaitMessage();

                MessageText = "(Вы получили новую точку для быстрого перемещения.)";
                SelectScreen(3);
                yield return AwaitMessage();
                Meta.Instance.IsFoundCastle = true;
            }
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
        if (index == 4 && BuyIndex == 2 && Stats.Instance.Data[9] == 1)
        {
            return;          
        }
        
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
            WriteMessageMain("Привет! Рад видеть тебя в моём магазине.");
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
                message = "Спасибо за покупку.";
            }
            else if (_isNotEnoughMoney)
            {
                message = "Тебе нужно еще поднакопить $.";
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
            
            _speakMessageLabel.ShowText("О чем ты хочешь поговорить?");
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

            var price = BuyIndex switch
            {
                0 => "5$",
                1 => "9$",
                2 => "22$",
                3 => "200Ян",
            };
            
            _priceLabel.text = $"Купить за {price}?";
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
        var createdLabels = new List<TypewriterComponent>();
        string[] parts = MessageText.Split('*', StringSplitOptions.RemoveEmptyEntries);
        var currentLabel = _messageLabel;
        var previousLabel = _messageLabel;

        var isSkip = false;
        
        for (var i = 0; i < parts.Length; i++)
        {
            var part = parts[i];

            if (i != 0)
            {
                currentLabel = Instantiate(previousLabel, transform);
                currentLabel.StopShowingText();
                currentLabel.GetComponent<TextMeshPro>().text = null;
                
                if (!currentLabel.gameObject.GetComponent<SortingGroup>())
                    currentLabel.gameObject.AddComponent<SortingGroup>().sortingOrder = 5;
                
                createdLabels.Add(currentLabel);
                var textMesh = previousLabel.GetComponentInChildren<TextMeshPro>();

                currentLabel.transform.position = new Vector3(previousLabel.transform.position.x,
                    previousLabel.transform.position.y - textMesh.renderedHeight,
                    previousLabel.transform.position.z); // -1

                currentLabel.GetComponentInChildren<TextMeshPro>().text = string.Empty;
            }

            previousLabel = currentLabel;
            currentLabel.ShowText(part);
            currentLabel.StartShowingText();

            while (currentLabel.IsShowingText)
            {
                if (InputManager.Instance.IsCancelDown)
                {
                    isSkip = true;
                }

                if (isSkip)
                {
                    currentLabel.SkipTypewriter();
                }
                
                yield return null;
            }
        }
        
        
        yield return InputManager.Instance.AwaitSubmitDownOrClick();
        
        for (int i = 0; i < createdLabels.Count; i++)
        {
            Destroy(createdLabels[i].gameObject);
        }
        
        SelectScreen(0);
    }
}
