using System.Collections;
using System.Collections.Generic;
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
    private SpriteRenderer _frame;
    
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
    
    [Header("Data")]
    public bool IsEnemyTurn;
    public bool IsMain = true;
    public bool IsFightSelect;
    public bool IsActSelect;
    public bool IsAct;
    public bool IsItems;
    public bool IsMercy;
    public bool IsRun;

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
    public List<ActLabel> ActLabels;
    public List<ActLabel> ItemLabels;
    public float Progress;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
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
        if (IsEnemyTurn)
        {
            Progress += Time.deltaTime * 2;
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
                        MainIndex = 3;
                    else
                        _squeakSFX.Play();
                }
                else if (InputManager.Instance.Horizontal < 0)
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
                    else if (ActIndex == 2 && actionsCount > 3)
                    {
                        ActIndex = 3;
                        _squeakSFX.Play();
                    }
                    else if (ActIndex == 4 && actionsCount > 5)
                    {
                        ActIndex = 5;
                        _squeakSFX.Play();
                    }
                }
                else if (InputManager.Instance.Horizontal < 0)
                {
                    if (ActIndex == 1)
                    {
                        ActIndex = 0;
                        _squeakSFX.Play();
                    }
                    else if (ActIndex == 3)
                    {
                        ActIndex = 2;
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
            var allItems = new List<string>();
                
            for (int i = 0; i < Stats.Instance.Items.Length; i++)
            {
                if (string.IsNullOrEmpty(Stats.Instance.Items[i]))
                    continue;
                        
                allItems.Add(Stats.Instance.Items[i]);
            }
            
            if (InputManager.Instance.IsSubmitDown)
            {
                EndTurn();
            }
            else if (InputManager.Instance.IsCancelDown)
            {
                SelectScreen(-1);
            }
            else if (InputManager.Instance.HorizontalDown)
            {
                if (InputManager.Instance.Horizontal > 0)
                {
                    if (ItemsIndex == 0 && allItems.Count > 1)
                    {
                        ItemsIndex = 1;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 1 && allItems.Count > 6)
                    {
                        ItemsIndex = 6;
                        IsKarmaniPage_2 = true;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 2 && allItems.Count > 3)
                    {
                        ItemsIndex = 3;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 3 && allItems.Count > 6)
                    {
                        ItemsIndex = 6;
                        IsKarmaniPage_2 = true;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 4 && allItems.Count > 5)
                    {
                        ItemsIndex = 5;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 5 && allItems.Count > 6)
                    {
                        ItemsIndex = 6;
                        IsKarmaniPage_2 = true;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 6 && allItems.Count > 7)
                    {
                        ItemsIndex = 7;
                        _squeakSFX.Play();
                    }
                }
                else if (InputManager.Instance.Horizontal < 0)
                {
                    if (ItemsIndex == 1)
                    {
                        ItemsIndex = 0;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 3)
                    {
                        ItemsIndex = 2;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 5)
                    {
                        ItemsIndex = 4;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 6)
                    {
                        ItemsIndex = 1;
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
                    if (ItemsIndex == 2)
                    {
                        ItemsIndex = 0;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 3)
                    {
                        ItemsIndex = 1;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 4)
                    {
                        ItemsIndex = 2;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 5)
                    {
                        ItemsIndex = 3;
                        _squeakSFX.Play();
                    }
                }
                else if (InputManager.Instance.Vertical < 0)
                {
                    if (ItemsIndex == 0 && allItems.Count > 2)
                    {
                        ItemsIndex = 2;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 1 && allItems.Count > 3)
                    {
                        ItemsIndex = 3;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 2 && allItems.Count > 4)
                    {
                        ItemsIndex = 4;
                        _squeakSFX.Play();
                    }
                    else if (ItemsIndex == 3 && allItems.Count > 5)
                    {
                        ItemsIndex = 5;
                        _squeakSFX.Play();
                    }
                }
            }

            if (IsKarmaniPage_2 && !IsKarmaniSpawnedPage_2)
            {
                for (int i = 0; i < ItemLabels.Count; i++)
                {
                    Destroy(ItemLabels[i].gameObject);
                }
                
                ItemLabels =  new List<ActLabel>();
                
                for (int i = 6; i < allItems.Count; i++)
                {
                    var itemLabel = Instantiate(Resources.Load<ActLabel>("Action Label"), _itemsScreen.transform);
                    itemLabel.Init(1, i);
                    ItemLabels.Add(itemLabel);
                    
                    ItemLabels[i - 6].transform.localPosition = i switch
                    {
                        6 => new Vector2(-2f, 0.97f),
                        7 => new Vector2(4.3f, 0.97f)
                    };
                    
                    ItemLabels[i - 6].Label.text = Stats.Instance.Items[i];
                }
                
                IsKarmaniSpawnedPage_2 = true;
            }
            else if (!IsKarmaniPage_2 && IsKarmaniSpawnedPage_2)
            {
                for (int i = 0; i < ItemLabels.Count; i++)
                {
                    Destroy(ItemLabels[i].gameObject);
                }
                
                ItemLabels =  new List<ActLabel>();
                
                for (int i = 0; i < 6; i++)
                {
                    var itemLabel = Instantiate(Resources.Load<ActLabel>("Action Label"), _itemsScreen.transform);
                    itemLabel.Init(1, i);
                    ItemLabels.Add(itemLabel);
                    
                    ItemLabels[i].transform.localPosition = i switch
                    {
                        0 => new Vector2(-2f, 0.97f),
                        1 => new Vector2(4.3f, 0.97f),
                        2 => new Vector2(-2f, 0.05f),
                        3 => new Vector2(4.3f, 0.05f),
                        4 => new Vector2(-2f, -0.88f),
                        5 => new Vector2(4.3f, -0.88f)
                    };
                    
                    ItemLabels[i].Label.text = Stats.Instance.Items[i];
                }
                
                IsKarmaniSpawnedPage_2 = false;
            }

            Heart.Instance.transform.localPosition = ItemsIndex switch
            {
                0 => new Vector2(-6.54f, -1.09f),
                1 => new Vector2(-0.26f, -1.09f),
                2 => new Vector2(-6.54f, -2f),
                3 => new Vector2(-0.26f, -2f),
                4 => new Vector2(-6.54f, -2.94f),
                5 => new Vector2(-0.26f, -2.94f),
                6 => new Vector2(-6.54f, -1.09f),
                7 => new Vector2(-0.26f, -1.09f),
                _ => new Vector2(-6.54f, -1.09f)
            };
        }
        else if (IsMercy)
        {
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
                    if (MercyIndex == 1)
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
                }
            }
            
            Heart.Instance.transform.localPosition = MercyIndex switch
            {
                0 => new Vector2(-6.54f, -1.09f),
                1 => new Vector2(-6.54f, -2.08f),
            };
        }
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
            ||  (index == 2 && IsItems) 
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
                Destroy(FightEnemyLabels[i].gameObject);
            }
            
            FightEnemyLabels =  new List<FightLabel>();
        }
        
        if (index != 1)
        {
            IsActSelect = false;
            _actSelectScreen.SetActive(false);
        }
        
        if (index != 2)
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
        }
        else if (index == 2)
        {
            IsItems = true;
            _itemsScreen.SetActive(true);
            _selectSFX.Play();
            
            var itemIndex = 0;
            var itemsCount = 0;

            foreach (var item in Stats.Instance.Items)
            {
                if (string.IsNullOrEmpty(item))
                    continue;

                itemsCount++;
            }
                    
            var items = new string[itemsCount];
            var j = 0;
                    
            foreach (var item in Stats.Instance.Items)
            {
                if (string.IsNullOrEmpty(item))
                    continue;
                        
                items[j] = item;
                j++;
            }

            for (int i = 0; i < items.Length; i++)
            {
                if (string.IsNullOrEmpty(items[i]))
                    continue;
                        
                if (itemIndex >= 6)
                    continue;

                var itemLabel = Instantiate(Resources.Load<ActLabel>("Action Label"), _itemsScreen.transform);
                itemLabel.Init(1, i);
                ItemLabels.Add(itemLabel);
                    
                ItemLabels[i].transform.localPosition = itemIndex switch
                {
                    0 => new Vector2(-2f, 0.97f),
                    1 => new Vector2(4.3f, 0.97f),
                    2 => new Vector2(-2f, 0.05f),
                    3 => new Vector2(4.3f, 0.05f),
                    4 => new Vector2(-2f, -0.88f),
                    5 => new Vector2(4.3f, -0.88f),
                };
                    
                ItemLabels[i].Label.text = items[i];
                itemIndex++;
            }
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
            
            for (int i = 0; i < Enemies[0].Actions.Count; i++)
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
                    
                ActLabels[i].Label.text = Enemies[0].Actions[i];
            }

            SelectScreenIndex = 1;
        }
    }

    private void HideButtons()
    {
        StartCoroutine(AwaitHideButtons());
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
            yield return null;
            _stickAnimator.transform.localPosition = Vector2.MoveTowards(
                _stickAnimator.transform.localPosition, new Vector2(7.47f, 0), 
                Time.deltaTime * 10);

            if (_stickAnimator.transform.localPosition.x >= 7.47f)
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
        var random = Random.Range(0, 3);
        var damage = (int)((at + weaponPower - defence + random) * coefficientAccuracy);
        
        if (Enemies[FightSelectIndex].Relationship >= 0 && damage < Enemies[FightSelectIndex].Health)
        {
            damage = Enemies[FightSelectIndex].Health;
        }
        
        _damageSFX.Play();
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
        
        var isEveryoneNotActive = true;

        foreach (var enemy in Enemies)
        {
            if (enemy.Health > 0)
                isEveryoneNotActive = false;
        }
        
        if (!isEveryoneNotActive)
        {
            EndTurn();
        }
        else
        {
            // StartCoroutine(AwaitExitMessage());
        }
    }

    private IEnumerator AwaitRunHeart()
    {
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
        
        // _runHeart.SetActive(false);
        // _mercyScreen.gameObject.SetActive(false);
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
            WriteMessage(Enemies[ActSelectedIndex].Answers[ActIndex] + "RRRRRRRRRRRR RRRRRR");
            yield return new WaitUntil(() => !MainText.IsWriting);
            yield return InputManager.Instance.AwaitSubmitDownOrClick();
        }
        else
        {
            WriteMessage("Вы использовали ФФФ");
            yield return new WaitUntil(() => !MainText.IsWriting);
            yield return InputManager.Instance.AwaitSubmitDownOrClick();
        }
        
        MainText.gameObject.SetActive(false);
        Heart.Instance.gameObject.SetActive(true);
        IsEnemyTurn = true;
    }
    
    public IEnumerator AwaitStartTurn()
    {
        if (MainIndex == 5)
            MainIndex = 1;

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
        
        while (_frame.size != size)
        {
            _frame.size = Vector2.MoveTowards(_frame.size, 
                size, Time.deltaTime * SPEED);
            yield return null;
        }
    }
    
    public IEnumerator AwaitExit()
    {
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
        SceneManager.LoadScene(Stats.Instance.LevelName, LoadSceneMode.Additive);

        yield return SceneManager.UnloadSceneAsync("Battle", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        
        Player.Instance.transform.position = Stats.Instance.Position;
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
}
