using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    public static bool IsStartBlackout;

    [Header("Links")]
    [SerializeField]
    private GameObject _fightSelectScreen;
    
    [SerializeField]
    private GameObject _fightScreen;
    
    [SerializeField]
    private GameObject _actSelectScreen;
    
    [SerializeField]
    private GameObject _actScreen;
    
    [SerializeField]
    private GameObject _itemsScreen;
    
    [SerializeField]
    private GameObject _mercyScreen;
    
    [SerializeField]
    private ActLabel _mercyLabel;
    
    [SerializeField]
    private ActLabel _runLabel;

    [SerializeField]
    private BattleButton[] _mainButtons;

    [SerializeField]
    private Transform _mainButtonsContainer;
    
    [SerializeField]
    private Animator _targetAnimator;

    [SerializeField]
    private Animator _stickAnimator;
    
    [SerializeField]
    private GameObject _runHeart;
    
    [SerializeField]
    private Frame _frame;
    
    [SerializeField]
    private SpriteRenderer _blackout;

    [SerializeField]
    private AudioSource _selectSFX;
    
    [SerializeField]
    private AudioSource _squeakSFX;
    
    [SerializeField]
    private AudioSource _runSFX;
    
    [SerializeField]
    private AudioSource _damageSFX;

    [SerializeField]
    private TextMeshPro _pageLabel;
    
    [SerializeField]
    private GameObject _itemHPFrame;
    
    [SerializeField]
    private TextMeshPro _itemHPLabel;
    
    [SerializeField]
    private AudioSource _healthSFX;
    
    [SerializeField]
    private AudioSource _mercySFX;
    
    [Header("Data")]
    public bool IsEnemyTurn;
    public bool IsMain = true;
    public bool IsFightSelect;
    public bool IsFight;
    public bool IsActSelect;
    public bool IsAct;
    public bool IsItems;
    public bool IsMercy;
    public bool IsRun;
    public bool IsRunAnimation;

    public int ItemsIndex;
    public bool IsKarmaniPage_2;
    public bool IsKarmaniSpawnedPage_2;
    
    public int MainIndex;
    public int SelectScreenIndex;
    public int FightSelectIndex;
    public int ActSelectedIndex;
    public int ActIndex;
    public int MercyIndex;
    public string Message;
    public BattleMainText MainText;
    public List<Enemy> Enemies;
    public List<FightLabel> FightEnemyLabels;
    public List<ActSelectLabel> ActEnemyLabels;
    public List<ActLabel> ActLabels;
    public List<ActLabel> ItemLabels;
    public float Progress;
    public int Turn;
    public bool IsAttack => EnemyMessageBox.Instance == null;
    public Frame GetFrame => _frame;
    public float ProgressMultiply = 1;
    
    public int StartMaxHP;
    public string[] StartItems;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartItems = Stats.Instance.Items;
        StartMaxHP = Stats.Instance.MaxHP;
        Stats.Instance.HP = Stats.Instance.MaxHP;
        _mercyLabel.Init(2, 0);
        _runLabel.Init(2, 1);
        
        Heart.Instance.transform.position = new Vector3(-6.69999981f,-5.34499979f,0);
        Heart.Instance.enabled = false;
        
        if (IsStartBlackout)
        {
            _blackout.gameObject.SetActive(true);
            _blackout.color = new Color(0, 0, 0, 1);
        }
    }

    private void Update()
    {
        if (IsEnemyTurn && IsAttack)
        {
            Progress += Time.deltaTime * ProgressMultiply;

            if (Stats.Instance.HP <= 0)
            {
                SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
            }
        }
        
        if (IsStartBlackout)
        {
            if (_blackout.color.a > 0)
            {
                var alpha = _blackout.color.a;
                alpha -= Time.deltaTime;
                _blackout.color = new Color(0, 0, 0, alpha);
            }
            else
            {
                _blackout.gameObject.SetActive(false);
                IsStartBlackout = false;
            }
        }
        
        if (IsMain)
        {
            if (InputManager.Instance.IsSubmitDown)
            {
                if (MainIndex == 0)
                {
                    SelectScreen(0);
                }
                else if (MainIndex == 1)
                {
                    if (Enemies.Count == 1)
                    {
                        SelectScreen(5);
                    }
                    else
                    {
                        SelectScreen(1);   
                    }
                }
                else if (MainIndex == 2)
                {
                    SelectScreen(2);
                }
                else if (MainIndex == 3)
                {
                    SelectScreen(3);
                }
            }
            else if (InputManager.Instance.HorizontalDown)
            {
                if (InputManager.Instance.Horizontal > 0)
                {
                    MainIndex++;
                    
                    if (MainIndex > 3)
                        MainIndex = 0;
                    
                    _squeakSFX.Play();
                }
                else if (InputManager.Instance.Horizontal < 0)
                {
                    MainIndex--;
                    
                    if (MainIndex < 0)
                        MainIndex = 3;
                    
                    _squeakSFX.Play();
                }
            }
        
            Heart.Instance.transform.position = MainIndex switch
            {
                0 => new Vector3(-6.69999981f,-5.34499979f,0),
                1 => new Vector3(-2.901f,-5.34499979f,0),
                2 => new Vector3(1.072f,-5.34499979f,0),
                3 => new Vector3(4.92f,-5.34499979f,0),
            }; 
        }
        else if (IsFightSelect)
        {
            if (InputManager.Instance.IsSubmitDown)
            {
                if (!Enemies[FightSelectIndex].IsActive)
                    return;
                
                SelectScreen(4);
            }
            else if (InputManager.Instance.IsCancelDown)
            {
                SelectScreen(-1);
            }
            else if (InputManager.Instance.VerticalDown)
            {
                if (InputManager.Instance.Vertical > 0)
                {
                    if (FightSelectIndex == 2)
                    {
                        FightSelectIndex = 1;
                        _squeakSFX.Play();
                    }
                    else if (FightSelectIndex == 1)
                    {
                        FightSelectIndex = 0;
                        _squeakSFX.Play();
                    }
                }
                else if (InputManager.Instance.Vertical < 0)
                {
                    if (FightSelectIndex == 0 && Enemies.Count > 1)
                    {
                        FightSelectIndex = 1;
                        _squeakSFX.Play();
                    }
                    else if (FightSelectIndex == 1 && Enemies.Count > 2)
                    {
                        FightSelectIndex = 2;
                        _squeakSFX.Play();
                    }
                }
            }
            
            Heart.Instance.transform.localPosition = FightSelectIndex switch
            {
                0 => new Vector2(-6.54f, -1.09f),
                1 => new Vector2(-6.54f, -2f),
                2 => new Vector2(-6.54f, -2.94f)
            };
        }
        else if (IsActSelect)
        {
            if (InputManager.Instance.IsSubmitDown)
            {
                if (!Enemies[ActSelectedIndex].IsActive)
                    return;
                
                SelectScreen(5);
            }
            else if (InputManager.Instance.IsCancelDown)
            {
                SelectScreen(-1);
            }
            else if (InputManager.Instance.VerticalDown)
            {
                if (InputManager.Instance.Vertical > 0)
                {
                    if (ActSelectedIndex == 2)
                    {
                        ActSelectedIndex = 1;
                        _squeakSFX.Play();
                    }
                    else if (ActSelectedIndex == 1)
                    {
                        ActSelectedIndex = 0;
                        _squeakSFX.Play();
                    }
                }
                else if (InputManager.Instance.Vertical < 0)
                {
                    if (ActSelectedIndex == 0 && Enemies.Count > 1)
                    {
                        ActSelectedIndex = 1;
                        _squeakSFX.Play();
                    }
                    else if (ActSelectedIndex == 1 && Enemies.Count > 2)
                    {
                        ActSelectedIndex = 2;
                        _squeakSFX.Play();
                    }
                }
            }
            
            Heart.Instance.transform.localPosition = ActSelectedIndex switch
            {
                0 => new Vector2(-6.54f, -1.09f),
                1 => new Vector2(-6.54f, -2f),
                2 => new Vector2(-6.54f, -2.94f)
            };
        }
        else if (IsAct)
        {
            var actionsCount = Enemies[ActSelectedIndex].Actions.Count;
            
            if (InputManager.Instance.IsSubmitDown)
            {
                EndTurn();
            }
            else if (InputManager.Instance.IsCancelDown)
            {
                if (Enemies.Count == 1)
                {
                    SelectScreen(-1);
                }
                else
                {
                    SelectScreen(1);
                }
            }
            else if (InputManager.Instance.HorizontalDown)
            {
                if (InputManager.Instance.Horizontal > 0)
                {
                    if (ActIndex == 0 && actionsCount > 1)
                    {
                        ActIndex = 1;
                        _squeakSFX.Play();
                    }
                    else if (ActIndex == 1)
                    {
                        ActIndex = 0;
                        _squeakSFX.Play();
                    }
                    else if (ActIndex == 2 && actionsCount > 3)
                    {
                        ActIndex = 3;
                        _squeakSFX.Play();
                    }
                    else if (ActIndex == 3)
                    {
                        ActIndex = 2;
                        _squeakSFX.Play();
                    }
                    else if (ActIndex == 4 && actionsCount > 5)
                    {
                        ActIndex = 5;
                        _squeakSFX.Play();
                    }
                    else if (ActIndex == 5)
                    {
                        ActIndex = 4;
                        _squeakSFX.Play();
                    }
                }
                else if (InputManager.Instance.Horizontal < 0)
                {
                    if (ActIndex == 0 && actionsCount > 1)
                    {
                        ActIndex = 1;
                        _squeakSFX.Play();
                    }
                    else if (ActIndex == 1)
                    {
                        ActIndex = 0;
                        _squeakSFX.Play();
                    }
                    else if (ActIndex == 2 && actionsCount > 3)
                    {
                        ActIndex = 3;
                        _squeakSFX.Play();
                    }
                    else if (ActIndex == 3)
                    {
                        ActIndex = 2;
                        _squeakSFX.Play();
                    }
                    else if (ActIndex == 4 && actionsCount > 5)
                    {
                        ActIndex = 5;
                        _squeakSFX.Play();
                    }
                    else if (ActIndex == 5)
                    {
                        ActIndex = 4;
                        _squeakSFX.Play();
                    }
                }
            } 
            else if (InputManager.Instance.VerticalDown)
            {
                if (InputManager.Instance.Vertical > 0)
                {
                    if (ActIndex == 2)
                    {
                        ActIndex = 0;
                        _squeakSFX.Play();
                    }
                    else if (ActIndex == 3)
                    {
                        ActIndex = 1;
                        _squeakSFX.Play();
                    }
                    else if (ActIndex == 4)
                    {
                        ActIndex = 2;
                        _squeakSFX.Play();
                    }
                    else if (ActIndex == 5)
                    {
                        ActIndex = 3;
                        _squeakSFX.Play();
                    }
                }
                else if (InputManager.Instance.Vertical < 0)
                {
                    if (ActIndex == 0 && actionsCount > 2)
                    {
                        ActIndex = 2;
                        _squeakSFX.Play();
                    }
                    else if (ActIndex == 1 && actionsCount > 3)
                    {
                        ActIndex = 3;
                        _squeakSFX.Play();
                    }
                    else if (ActIndex == 2 && actionsCount > 4)
                    {
                        ActIndex = 4;
                        _squeakSFX.Play();
                    }
                    else if (ActIndex == 3 && actionsCount > 5)
                    {
                        ActIndex = 5;
                        _squeakSFX.Play();
                    }
                }
            }
            
            Heart.Instance.transform.localPosition = ActIndex switch
            {
                0 => new Vector2(-6.54f, -1.09f),
                1 => new Vector2(-0.26f, -1.09f),
                2 => new Vector2(-6.54f, -2f),
                3 => new Vector2(-0.26f, -2f),
                4 => new Vector2(-6.54f, -2.94f),
                5 => new Vector2(-0.26f, -2.94f),
                _ => new Vector2(-6.54f, -1.09f)
            };
        }
        else if (IsItems)
        {
            var allItems = GetAllItems();
            
            if (InputManager.Instance.IsSubmitDown)
            {
                StartCoroutine(AwaitUseItem());
            }
            else if (InputManager.Instance.IsCancelDown)
            {
                SelectScreen(-1);
            }
            else if (InputManager.Instance.HorizontalDown)
            {
                if (InputManager.Instance.Horizontal > 0)
                {
                    if (ItemsIndex == 0 && allItems.Length > 1)
                    {
                        ItemsIndex = 1;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 1)
                    {
                        if (allItems.Length > 4)
                        {
                            ItemsIndex = 4;
                            IsKarmaniPage_2 = true;
                        }
                        else
                        {
                            ItemsIndex = 0;
                        }
                        
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 2 && allItems.Length > 3)
                    {
                        ItemsIndex = 3;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 3 && allItems.Length > 4)
                    {
                        ItemsIndex = allItems.Length > 6 ? 6 : 4;
                        IsKarmaniPage_2 = true;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 4)
                    {
                        if (allItems.Length > 5)
                        {
                            ItemsIndex = 5;
                        }
                        else
                        {
                            ItemsIndex = 0;
                            IsKarmaniPage_2 = false;
                        }
                        
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 5)
                    {
                        ItemsIndex = 0;
                        IsKarmaniPage_2 = false;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 6)
                    {
                        ItemsIndex = allItems.Length > 7 ? 7 : 0;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 7)
                    {
                        ItemsIndex = 2;
                        IsKarmaniPage_2 = false;
                        _squeakSFX.Play();
                    }
                }
                else if (InputManager.Instance.Horizontal < 0)
                {
                    if (ItemsIndex == 0)
                    {
                        if (allItems.Length > 5)
                        {
                            ItemsIndex = 5;  
                            IsKarmaniPage_2 = true;
                        }
                        else if (allItems.Length > 4)
                        {
                            ItemsIndex = 4;
                            IsKarmaniPage_2 = true;
                        }
                        else
                        {
                            ItemsIndex = 1;  
                        }
                        
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 1)
                    {
                        ItemsIndex = 0;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 2 && allItems.Length > 4)
                    {
                        if (allItems.Length > 7)
                        {
                            ItemsIndex = 7;
                        }
                        else if (allItems.Length > 6)
                        {
                            ItemsIndex = 6;
                        }
                        else if (allItems.Length > 5)
                        {
                            ItemsIndex = 5;
                        }
                        else
                        {
                            ItemsIndex = 4;
                        }
                        
                        IsKarmaniPage_2 = true;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 3)
                    {
                        ItemsIndex = 2;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 4)
                    {
                        ItemsIndex = 1;
                        IsKarmaniPage_2 = false;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 5)
                    {
                        ItemsIndex = 4;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 6)
                    {
                        ItemsIndex = 3;
                        IsKarmaniPage_2 = false;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 7)
                    {
                        ItemsIndex = 6;
                        _squeakSFX.Play();
                    }
                }
            } 
            else if (InputManager.Instance.VerticalDown)
            {
                if (InputManager.Instance.Vertical > 0)
                {
                    if (ItemsIndex == 0 && allItems.Length > 2)
                    {
                        ItemsIndex = 2;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 1 && allItems.Length > 3)
                    {
                        ItemsIndex = 3;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 2)
                    {
                        ItemsIndex = 0;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 3)
                    {
                        ItemsIndex = 1;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 4 && allItems.Length > 6)
                    {
                        ItemsIndex = 6;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 5 && allItems.Length > 7)
                    {
                        ItemsIndex = 7;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 6)
                    {
                        ItemsIndex = 4;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 7)
                    {
                        ItemsIndex = 5;
                        _squeakSFX.Play();
                    }
                }
                else if (InputManager.Instance.Vertical < 0)
                {
                    if (ItemsIndex == 0 && allItems.Length > 2)
                    {
                        ItemsIndex = 2;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 1 && allItems.Length > 3)
                    {
                        ItemsIndex = 3;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 2 && allItems.Length > 4)
                    {
                        ItemsIndex = 0;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 3 && allItems.Length > 5)
                    {
                        ItemsIndex = 1;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 4 && allItems.Length > 6)
                    {
                        ItemsIndex = 6;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 5 && allItems.Length > 7)
                    {
                        ItemsIndex = 7;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 6)
                    {
                        ItemsIndex = 4;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 7)
                    {
                        ItemsIndex = 5;
                        _squeakSFX.Play();
                    }
                }
            }
            
            if (IsKarmaniPage_2 && !IsKarmaniSpawnedPage_2)
            {
                OpenItems();
            }
            else if (!IsKarmaniPage_2 && IsKarmaniSpawnedPage_2)
            {
                OpenItems();
            }

            var hp = Stats.GetItemHPText(allItems[ItemsIndex]);
            _itemHPLabel.text = $"+{hp}ОЗ";
            
            Heart.Instance.transform.localPosition = ItemsIndex switch
            {
                0 => new Vector2(-6.54f, -1.09f),
                1 => new Vector2(-0.26f, -1.09f),
                2 => new Vector2(-6.54f, -2f),
                3 => new Vector2(-0.26f, -2f),
                4 => new Vector2(-6.54f, -1.09f),
                5 => new Vector2(-0.26f, -1.09f),
                6 => new Vector2(-6.54f, -2f),
                7 => new Vector2(-0.26f, -2f),
                _ => new Vector2(-6.54f, -1.09f)
            };
        }
        else if (IsMercy)
        {
            if (IsRunAnimation)
                return;
            
            if (InputManager.Instance.IsSubmitDown)
            {
                EndTurn();
            }
            else if (InputManager.Instance.IsCancelDown)
            {
                SelectScreen(-1);
            }
            else if (InputManager.Instance.VerticalDown)
            {
                if (InputManager.Instance.Vertical > 0)
                {
                    if (MercyIndex == 0 && IsRun)
                    {
                        MercyIndex = 1;
                        _squeakSFX.Play();
                    }
                    else if (MercyIndex == 1)
                    {
                        MercyIndex = 0;
                        _squeakSFX.Play();
                    }
                }
                else if (InputManager.Instance.Vertical < 0)
                {
                    if (MercyIndex == 0 && IsRun)
                    {
                        MercyIndex = 1;
                        _squeakSFX.Play();
                    }
                    else if (MercyIndex == 1)
                    {
                        MercyIndex = 0;
                        _squeakSFX.Play();
                    }
                }
            }
            
            Heart.Instance.transform.localPosition = MercyIndex switch
            {
                0 => new Vector2(-6.54f, -1.09f),
                1 => new Vector2(-6.54f, -2.08f),
            };
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach (var enemy in Enemies)
            {
                enemy.Relationship = 20;
            }
        }
#endif
    }

    private string[] GetAllItems()
    {
        var allItems = new List<string>();

        for (int i = 0; i < Stats.Instance.Items.Length; i++)
        {
            if (!string.IsNullOrEmpty(Stats.Instance.Items[i]))
            {
                allItems.Add(Stats.Instance.Items[i]);
            }
        }
        
        allItems.Add("Ads"); // Реклама
        return allItems.ToArray();
    }
    
    private void OpenItems()
    {
        var allItems = GetAllItems();
        
        if (IsKarmaniPage_2)
        {
            _pageLabel.text = "СТР 2";
            for (int i = 0; i < ItemLabels.Count; i++)
            {
                Destroy(ItemLabels[i].gameObject);
            }
                
            ItemLabels =  new List<ActLabel>();
                
            for (int i = 4; i < allItems.Length; i++)
            {
                var itemLabel = Instantiate(Resources.Load<ActLabel>("Action Label"), _itemsScreen.transform);
                itemLabel.Init(1, i);
                ItemLabels.Add(itemLabel);
                    
                ItemLabels[i - 4].transform.localPosition = i switch
                {
                    4 => new Vector2(-2f, 0.97f),
                    5 => new Vector2(4.3f, 0.97f),
                    6 => new Vector2(-2f, 0.05f),
                    7 => new Vector2(4.3f, 0.05f)
                };
                    
                ItemLabels[i - 4].Label.text = Stats.GetItemName(allItems[i]);;
            }
                
            IsKarmaniSpawnedPage_2 = true;
        }
        else
        {
            _pageLabel.text = "СТР 1";
            for (int i = 0; i < ItemLabels.Count; i++)
            {
                Destroy(ItemLabels[i].gameObject);
            }
                
            ItemLabels =  new List<ActLabel>();
            var count = allItems.Length;
                
            if (count > 4)
                count = 4;
                
            for (int i = 0; i < count; i++)
            {
                var itemLabel = Instantiate(Resources.Load<ActLabel>("Action Label"), _itemsScreen.transform);
                itemLabel.Init(1, i);
                ItemLabels.Add(itemLabel);
                    
                ItemLabels[i].transform.localPosition = i switch
                {
                    0 => new Vector2(-2f, 0.97f),
                    1 => new Vector2(4.3f, 0.97f),
                    2 => new Vector2(-2f, 0.05f),
                    3 => new Vector2(4.3f, 0.05f)
                };
                    
                ItemLabels[i].Label.text = Stats.GetItemName(allItems[i]);
            }
                
            IsKarmaniSpawnedPage_2 = false;
        }
    }

    private IEnumerator AwaitUseItem()
    {
        var activeItems = 0;

        foreach (var item1 in Stats.Instance.Items)
        {
            if (!string.IsNullOrEmpty(item1))
            {
                activeItems++;
            }
        }
        
        var item = GetAllItems()[ItemsIndex];
        Stats.Instance.RemoveItem(item);
        
        SelectScreen(-99);
        Heart.Instance.gameObject.SetActive(false);
        // По какой-то причине вы восстановили ОЗ.

        var itemName = Stats.GetItemName(item);
        
        if (item == "Ads")
        {
            itemName = "Рекламу";
        }
        
        var message = $"Вы использовали <Color=\"yellow\">{itemName}</Color>.";

        if (item != "Ads")
        {
            message += ((Stats.Instance.HP + Stats.GetItemHP(item) >= Stats.Instance.MaxHP)
                ? "*Вы восстановили всё ОЗ."
                : $"*Вы восстановили <Color=\"yellow\">{Stats.GetItemHPText(item)}</Color>ОЗ.");
        }
        else
        {
            Stats.Instance.MaxHP += 2;
            
            message += ((Stats.Instance.HP + Stats.GetItemHP(item) >= Stats.Instance.MaxHP)
                ? "*По какой-то причине вы восстановили всё ОЗ."
                : $"*По какой-то причине вы  восстановили <Color=\"yellow\">{Stats.GetItemHPText(item)}</Color>ОЗ.");
        }
        
        if (item == "GoldApple")
        {
            Stats.Instance.MaxHP += 4;
            message += "*И даже больше.";
        }
        
        Health(Stats.GetItemHP(item));
        
        yield return MainText.AwaitWrite(message);
        
        Heart.Instance.gameObject.SetActive(true);
        SelectScreen(-1);

        yield return null;
    }

    public void Health(int health)
    {
        var difference = Stats.Instance.MaxHP - Stats.Instance.HP;
        var add = Mathf.Min(difference, health);
        
        if (add < 0)
            add = 0;
        
        Stats.Instance.HP += add;
        _healthSFX.Play();
    }
    
    public void WriteMessage(string message)
    {
        MainText.gameObject.SetActive(true);
        Message = message;
        MainText.Write(Message);
    }

    /*
     * -1 - Main
     * 0 - Fight Select
     * 1 - ActSelect
     * 2 - Items
     * 3 - Mercy
     * 4 - Fight
     * 5 - Act
     */
    public void SelectScreen(int index)
    {
        if (index >= 0 && index <= 3)
            SelectScreenIndex = index;
        
        if ((index == 0 && IsFightSelect) 
            || (index == 1 && IsActSelect) 
            //||  (index == 2 && IsItems) 
            || (index == 3 && IsMercy)
            || (index == -1 && IsMain)
            || (index == 5 && IsAct))
            return;
        
        if (index != -1)
        {
            IsMain = false; 
            MainText.gameObject.SetActive(false);
        }
        
        if (index != 0)
        {
            IsFightSelect = false;
            _fightSelectScreen.SetActive(false);

            for (int i = 0; i < FightEnemyLabels.Count; i++)
            {
                if (FightEnemyLabels[i])
                    Destroy(FightEnemyLabels[i].gameObject);
            }
            
            FightEnemyLabels =  new List<FightLabel>();
        }
        
        if (index != 1)
        {
            IsActSelect = false;
            _actSelectScreen.SetActive(false);
            
            for (int i = 0; i < ActEnemyLabels.Count; i++)
            {
                if (ActEnemyLabels[i])
                    Destroy(ActEnemyLabels[i].gameObject);
            }
            
            ActEnemyLabels =  new List<ActSelectLabel>();
        }
        
        if (index != 2 || (IsItems))
        {
            IsItems = false;
            _itemsScreen.SetActive(false);
            
            for (int i = 0; i < ItemLabels.Count; i++)
            {
                Destroy(ItemLabels[i].gameObject);
            }
            
            ItemLabels =  new List<ActLabel>();
        }
        
        if (index != 3)
        {
            IsMercy = false;
            _mercyScreen.SetActive(false);
        }

        if (index != 5)
        {
            IsAct = false; 
            _actScreen.SetActive(false);
            
            for (int i = 0; i < ActLabels.Count; i++)
            {
                Destroy(ActLabels[i].gameObject);
            }
            
            ActLabels =  new List<ActLabel>();
        }
        
        if (index == -1)
        {
            IsMain = true;
            MainText.Write(Message);
        }
        else if (index == 0)
        {
            IsFightSelect = true;
            _fightSelectScreen.SetActive(true);
            _selectSFX.Play();
            
            for (int i = 0; i < Enemies.Count; i++)
            {
                if (!Enemies[i].IsActive)
                {
                    FightEnemyLabels.Add(null);
                    continue;
                }
                        
                FightEnemyLabels.Add(Instantiate(Resources.Load<FightLabel>("Fight Label"),
                    _fightSelectScreen.transform));
                    
                FightEnemyLabels[i].transform.localPosition = i switch
                {
                    0 => new Vector2(-2.1f, 0.97f),
                    1 => new Vector2(-2.1f, 0.05f),
                    2 => new Vector2(-2.1f, -0.88f)
                };
                    
                FightEnemyLabels[i].Label.text = Enemies[i].Name;
                FightEnemyLabels[i].SetHealth(i, Enemies[i].Health, Enemies[i].MaxHealth, Enemies[i].Relationship >= 0);
            }
        }
        else if (index == 1)
        {
            IsActSelect = true;
            _actSelectScreen.SetActive(true);
            _selectSFX.Play();
            
            for (int i = 0; i < Enemies.Count; i++)
            {
                if (!Enemies[i].IsActive)
                {
                    ActEnemyLabels.Add(null);
                    continue;
                }
                        
                ActEnemyLabels.Add(Instantiate(Resources.Load<ActSelectLabel>("Act Select Label"),
                    _actSelectScreen.transform));
                    
                ActEnemyLabels[i].transform.localPosition = i switch
                {
                    0 => new Vector2(-2.1f, 0.97f),
                    1 => new Vector2(-2.1f, 0.05f),
                    2 => new Vector2(-2.1f, -0.88f)
                };
                    
                ActEnemyLabels[i].Label.text = Enemies[i].Name;
                ActEnemyLabels[i].SetHealth(i, Enemies[i].Health, Enemies[i].MaxHealth, Enemies[i].Relationship >= 0);
            }
        }
        else if (index == 2)
        {
            IsItems = true;
            _itemsScreen.SetActive(true);
            _selectSFX.Play();
            IsKarmaniPage_2 = false;
            IsKarmaniSpawnedPage_2 = false;
            ItemsIndex = 0;
            OpenItems();
        }
        else if (index == 3)
        {
            IsMercy = true;
            _mercyScreen.SetActive(true);
            _selectSFX.Play();
            
            if (IsRun)
            {
                _runLabel.gameObject.SetActive(true);
            }
            else
            {
                _runLabel.gameObject.SetActive(false);
            }

            var isSoryan = false;

            foreach (var enemy in Enemies)
            {
                if (enemy.Relationship >= 0 && enemy.IsActive)
                {
                    isSoryan = true;
                }
            }
                    
            _mercyLabel.Label.color = isSoryan ? Color.yellow : Color.white;
            _mercyLabel.Star.color = isSoryan ? Color.yellow : Color.white;
        }
        else if (index == 4)
        {
            _fightScreen.SetActive(true);
            HideButtons();
            StartCoroutine(AwaitFight());
            _selectSFX.Play();
            
            SelectScreenIndex = 0;
        }
        else if (index == 5)
        {
            IsAct = true;
            _actScreen.SetActive(true);
            _selectSFX.Play();
            
            for (int i = 0; i < Enemies[ActSelectedIndex].Actions.Count; i++)
            {
                var actLabel = Instantiate(Resources.Load<ActLabel>("Action Label"), _actScreen.transform);
                actLabel.Init(0, i);
                ActLabels.Add(actLabel);
                    
                ActLabels[i].transform.localPosition = i switch
                {
                    0 => new Vector2(-2f, 0.97f),
                    1 => new Vector2(4.3f, 0.97f),
                    2 => new Vector2(-2f, 0.05f),
                    3 => new Vector2(4.3f, 0.05f),
                    4 => new Vector2(-2f, -0.88f),
                    5 => new Vector2(4.3f, -0.88f),
                };
                    
                ActLabels[i].Label.text = Enemies[ActSelectedIndex].Actions[i];
            }

            SelectScreenIndex = 1;
        }
    }

    public void HideButtons(bool isSkip = false)
    {
        if (isSkip)
        {
            foreach (var button in _mainButtons)
            {
                button.enabled = false;
            }
            
            _mainButtonsContainer.position = _mainButtonsContainer.position.SetY(-1.55f);
        }
        else
        {
            StartCoroutine(AwaitHideButtons());
        }
    }

    private IEnumerator AwaitHideButtons()
    {
        if (!_mainButtons[0].enabled)
            yield break;
        
        foreach (var button in _mainButtons)
        {
            button.enabled = false;
        }

        const float TARGET = -1.55f;
        const float SPEED = 3f;
        var y = _mainButtonsContainer.position.y;
        
        while (y > TARGET)
        {
            yield return null;
            y -= Time.deltaTime * SPEED;
            _mainButtonsContainer.position = _mainButtonsContainer.position.SetY(y);
        }
    }

    private IEnumerator AwaitFight()
    {
        IsFight = true;
        Heart.Instance.gameObject.SetActive(false);
        _targetAnimator.gameObject.SetActive(true);
        _targetAnimator.Play("Target_Intro");
        _stickAnimator.Play("Stick Normal");
        _stickAnimator.transform.localPosition = new Vector2(-7.47f, 0);
        
        yield return new WaitUntil(() => _targetAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        
        _stickAnimator.gameObject.SetActive(true);
        var isMoveStick = true;
        
        while (isMoveStick)
        {
            var TARGET_X = 8.4f;
            
            yield return null;
            _stickAnimator.transform.localPosition = Vector2.MoveTowards(
                _stickAnimator.transform.localPosition, new Vector2(TARGET_X, 0), 
                Time.deltaTime * 10);

            if (_stickAnimator.transform.localPosition.x >= TARGET_X)
            {
                isMoveStick = false;
            }
            
            if (InputManager.Instance.IsSubmitDown || Input.GetMouseButtonDown(0))
            {
                isMoveStick = false;
            }
        }
        
        _stickAnimator.Play("Stick");
        // _lazSFX.Play();
        
        var coefficientAccuracy = 3f;
        var distance = Mathf.Abs(_stickAnimator.transform.localPosition.x);

        if (distance < 0.6f)
        {
            coefficientAccuracy = 3f;
        }
        else if (distance < 2.5f)
        {
            coefficientAccuracy = 2.5f;
        }
        else if (distance < 5.31f)
        {
            coefficientAccuracy = 2f;
        }
        else if (distance < 6.64f)
        {
            coefficientAccuracy = 1f;
        }
        else
        {
            coefficientAccuracy = 0f;
        }
        
        var at = 0;
        var weaponPower = 1;
        var defence = 0;
        var random = 0;
        var damage = (int)((at + weaponPower - defence + random) * coefficientAccuracy + 4f);
        
        if (Enemies[FightSelectIndex].Relationship >= 0 && damage < Enemies[FightSelectIndex].Health)
        {
            damage = Enemies[FightSelectIndex].Health;
        }

        if (Enemies[FightSelectIndex].ID == "Mob Arena" 
            || Enemies[FightSelectIndex].ID == "Admin")
        {
            damage = 0;
        }
        
        if (damage > 0)
        {
            Enemies[FightSelectIndex].Damage();
            _damageSFX.Play();
        }

        var damageLabel = Instantiate(Resources.Load<DamageIndicator>("Damage Indicator"));
        yield return damageLabel.Init(Enemies[FightSelectIndex], damage);
        yield return new WaitForSeconds(1);
        
        Enemies[FightSelectIndex].Health -= damage;

        if (Enemies[FightSelectIndex].Health <= 0)
        {
            // IsEnemyDead = true;
        }
        
        Destroy(damageLabel.gameObject);
        
        _targetAnimator.Play("Target_Outro");
        yield return new WaitUntil(() => _targetAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        
        _targetAnimator.gameObject.SetActive(false);
        _stickAnimator.gameObject.SetActive(false);
        IsFight = false;
        
        EndTurn();
        
        // var isEveryoneNotActive = true;
        //
        // foreach (var enemy in Enemies)
        // {
        //     if (enemy.Health > 0)
        //         isEveryoneNotActive = false;
        // }
        //
        // if (!isEveryoneNotActive)
        // {
        //     
        // }
        // else
        // {
        //     // StartCoroutine(AwaitExitMessage());
        // }
    }

    private IEnumerator AwaitRunHeart()
    {
        IsRunAnimation = true;
        
        Destroy(_mercyLabel);
        Destroy(_runLabel);
        _runHeart.SetActive(true);
        _runSFX.Play();

        while (_runHeart.transform.localPosition != new Vector3(-6.5f, 0.06f))
        {
            _runHeart.transform.localPosition = Vector2.MoveTowards(_runHeart.transform.localPosition, 
                new Vector3(-6.5f, 0.06f), Time.deltaTime * 2);
            yield return null;
        }

        yield return Enemies[0].AwaitRun();
        
        RollBackData();
        CoroutineRunner.Instance.StartCoroutine(AwaitExit());
    }
    
    public void StartTurn()
    {
        StartCoroutine(AwaitStartTurn());
    }
    
    public void EndTurn()
    {
        if (SelectScreenIndex != 0)
            _selectSFX.Play();
        
        StartCoroutine(AwaitEndTurn());
    }

    private IEnumerator AwaitEndTurn()
    {
        if (ActIndex != 0)
            HideButtons();
        
        Heart.Instance.gameObject.SetActive(false);

        if (SelectScreenIndex == 3 && MercyIndex == 1)
        {
            yield return AwaitRunHeart();
            yield break;
        }
        
        SelectScreen(-100);

        if (SelectScreenIndex == 0)
        {
            
        }
        else if (SelectScreenIndex == 1)
        {
            yield return MainText.AwaitWrite(Enemies[ActSelectedIndex].Answers[ActIndex]);
        }
        else if (SelectScreenIndex == 3)
        {
            var isMercy = false;
            
            foreach (var enemy in Enemies)
            {
                if (enemy.Relationship > 0)
                {
                    isMercy = true;
                }
            }
            
            if (isMercy)
            {
                _mercySFX.Play();
            }
            else
            {
                yield return MainText.AwaitWrite(
                    "Вы попытались использовать пощаду, но ник не <color=\"yellow\">ЖЕЛТЫЙ</color>.");
            }
        }
        // else
        // {
        //     WriteMessage("Вы использовали ФФФ");
        //     yield return new WaitUntil(() => !MainText.IsWriting);
        //     yield return InputManager.Instance.AwaitSubmitDownOrClick();
        // }
        
        MainText.gameObject.SetActive(false);
        Heart.Instance.gameObject.SetActive(true);

        if (SelectScreenIndex == 1 && ActIndex == 0)
        {
            SelectScreen(-1);
            yield break;
        }

        Progress = 0;
        IsEnemyTurn = true;
    }
    
    public IEnumerator AwaitStartTurn()
    {
        Turn++;
        
        ActSelectedIndex = 0;
        FightSelectIndex = 0;
        ActIndex = 0;
        ItemsIndex = 0;
        MercyIndex = 0;
        
        Heart.Instance.gameObject.SetActive(false);
        
        if (MainText.gameObject.activeSelf)
            MainText.Reset();
        
        MainText.gameObject.SetActive(false);
        
        if (MainIndex == 5)
            MainIndex = 1;

        ProgressMultiply = 1;
        yield return AwaitSetFrame(new Vector2(7.177707f, 1.75f));
        SelectScreen(-1);
        yield return AwaitShowButtons();
        Heart.Instance.gameObject.SetActive(true);
        IsEnemyTurn = false;
    }

    private void ShowButtons()
    {
        StartCoroutine(AwaitShowButtons());
    }

    private IEnumerator AwaitShowButtons()
    {
        if (_mainButtons[0].enabled)
            yield break;
        
        const float TARGET = 0f;
        const float SPEED = 3f;
        var y = _mainButtonsContainer.position.y;
        
        while (y < TARGET)
        {
            yield return null;
            y += Time.deltaTime * SPEED;
            _mainButtonsContainer.position = _mainButtonsContainer.position.SetY(y);
        }
        
        foreach (var button in _mainButtons)
        {
            button.enabled = true;
        }
    }
    
    public IEnumerator AwaitSetFrame(Vector2 size)
    {
        const float SPEED = 6f;
        
        while (_frame.GetComponent<SpriteRenderer>().size != size)
        {
            _frame.SetFrame(Vector2.MoveTowards(_frame.GetComponent<SpriteRenderer>().size, 
                size, Time.deltaTime * SPEED));
            yield return null;
        }
    }

    public void SetFrame(Vector2 size)
    {
        _frame.GetComponent<Frame>().SetFrame(size);
    }
    
    public IEnumerator AwaitExit()
    {
        if (Stats.Instance.Data[32] == 2 
            || (Stats.Instance.Data[33] == 2 
                && Stats.Instance.Data[34] == 2 
                && Stats.Instance.Data[35] == 2))
        {
            Stats.Instance.Data[2] = 1;
        }

        for (int i = 32; i < 36; i++)
        {
            if (Stats.Instance.Data[i] == 1)
            {
                Stats.Instance.Data[2] = 0;
            }
        }
        
        _blackout.gameObject.SetActive(true);
        // _spareSFX.Play();
        Heart.Instance.GetComponent<SpriteRenderer>().sortingOrder = 1;

        while (_blackout.color.a < 1)
        {
            yield return null;
            var color = _blackout.color;
            color.a += Time.deltaTime;
            _blackout.color = color;
        }
        
        yield return SceneManager.LoadSceneAsync("Overworld", LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Overworld"));
        Player.Instance.transform.position = Stats.Instance.Position;
        SceneManager.LoadScene(Stats.Instance.LevelName, LoadSceneMode.Additive);

        yield return SceneManager.UnloadSceneAsync("Battle", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        Debug.Log("Перешли на другой уровень");
    }
    
    public bool IsEnemySelected(Enemy enemy)
    {
        if (SelectScreenIndex == 0 && Enemies[FightSelectIndex].IsActive)
        {
            return Enemies[FightSelectIndex] == enemy;
        }
        else if (SelectScreenIndex == 1 && Enemies[ActSelectedIndex].IsActive)
        {
            return Enemies[ActSelectedIndex] == enemy;
        }

        if (Enemies[0].IsActive)
        {
            if (enemy == Enemies[0])
                return true;

            return false;
        }


        if (Enemies.Count > 1 && Enemies[1].IsActive)
        {
            if (enemy == Enemies[1])
                return true;

            return false;
        }

        if (Enemies.Count > 2 && Enemies[2].IsActive && enemy == Enemies[2])
        {
            if (enemy == Enemies[2])
                return true;

            return false;
        }
        
        return Enemies[0] == enemy;
    }

    public void RollBackData()
    {
        Stats.Instance.Items = StartItems;
        Stats.Instance.MaxHP = StartMaxHP;

        if (Enemies[0].ID == "Hacker")
        {
            Stats.Instance.Data[31] = 3;
        }
        else if (Enemies[0].ID == "Grum")
        {
            Stats.Instance.Data[32] = 3;
        }
        else if (Enemies[0].ID == "Herobrine")
        {
            Stats.Instance.Data[36] = 3;
        }
        else if (Enemies[0].ID == "Fivfiv")
        {
            Stats.Instance.Data[33] = 3;
        }
        else if (Enemies[0].ID == "Hero")
        {
            Stats.Instance.Data[37] = 3;
        }
    }

    public void StartEnemyTurnTest()
    {
        IsEnemyTurn = true;
        MainText.gameObject.SetActive(false);
        HideButtons();
        Heart.Instance.enabled = true;
    }
}
