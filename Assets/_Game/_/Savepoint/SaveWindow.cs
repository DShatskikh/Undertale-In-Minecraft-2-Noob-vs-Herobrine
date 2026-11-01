using System.Collections;
using TMPro;
using UnityEngine;

public sealed class SaveWindow : MonoBehaviour
{
    public static SaveWindow Instance;
    
    [SerializeField]
    private GameObject _heart;

    [SerializeField]
    private GameObject _main;
    
    [SerializeField]
    private GameObject _teleport;
    
    [SerializeField]
    private GameObject _inventory;
    
    [SerializeField]
    private GameObject _frame;
    
    [SerializeField]
    private TextMeshPro _saveLabel;
    
    [SerializeField]
    private TextButton[] _mainButtons;
    
    [SerializeField]
    private TextButton[] _teleportButtons;
    
    [SerializeField]
    private TextButton[] _inventoryButtons;
    
    [SerializeField]
    private AudioSource _selectSound;
    
    [SerializeField]
    private AudioClip _clickSFX;

    [SerializeField]
    private TextMeshPro _locationLabel;
    
    [SerializeField]
    private TextMeshPro _timeLabel;

    [SerializeField]
    private GameObject _inventoryRight;
    
    [SerializeField]
    private TextButton[] _inventoryRightButtons;
    
    [SerializeField]
    private TextButton[] _itemButtons;
    
    private int _mainIndex;
    private int _teleportIndex = -1;
    private int _inventoryIndex;
    private int _healthIndex;
    private int _healthSelectIndex;
    private int _itemIndex;
    private int _metaIndex;
    private bool _isMain = true;
    private bool _isTeleport;
    private bool _isInit;
    private bool _isHealth;
    private bool _isHealthSelect;
    private bool _isItem;
    private bool _isMeta;
    private int _locationIndex;
    private bool _isInventory;

    private void Awake()
    {
        Instance = this;
        
        _frame.SetActive(true);
        _main.SetActive(true);
        _inventory.SetActive(false);
        _inventoryRight.SetActive(false);
        _inventoryRightButtons[1].gameObject.SetActive(false);
        
        for (int i = 0; i < _itemButtons.Length; i++)
        {
            _itemButtons[i].GetComponent<TextMeshPro>().text = "(Пусто)";
        }
        
        _mainButtons[0].Select = () => MainSelect(0);
        _mainButtons[1].Select = () => MainSelect(1);
        _mainButtons[2].Select = () => MainSelect(2);
        _mainButtons[3].Select = () => MainSelect(3);
        
        _mainButtons[0].Click = () => MainClick(0);
        _mainButtons[1].Click = () => MainClick(1);
        _mainButtons[2].Click = () => MainClick(2);
        _mainButtons[3].Click = () => MainClick(3);
        
        _teleportButtons[0].Click = () => TeleportClick(0);
        _teleportButtons[1].Click = () => TeleportClick(1);
        _teleportButtons[2].Click = () => TeleportClick(2);
        _teleportButtons[3].Click = () => TeleportClick(3);
        
        _itemButtons[0].Select = () => InventorySelect(0);
        _itemButtons[1].Select = () => InventorySelect(1);
        _itemButtons[2].Select = () => InventorySelect(2);
        _itemButtons[3].Select = () => InventorySelect(3);
        
        _inventoryButtons[0].Click = () => InventoryClick(0);
        _inventoryButtons[1].Click = () => InventoryClick(1);
        _inventoryButtons[2].Click = () => InventoryClick(2);
        _inventoryButtons[3].Click = () => InventoryClick(3);
        
        _inventoryRightButtons[0].Click = () => HealthClick(7);
        _inventoryRightButtons[1].Click = () => HealthSelectClick(1);
        
        _inventoryRightButtons[0].Select = () => HealthSelect(7);
        _inventoryRightButtons[1].Select = () => HealthSelectSelect(1);
        
        _itemButtons[0].Click = () => HealthClick(0);
        _itemButtons[1].Click = () => HealthClick(1);
        _itemButtons[2].Click = () => HealthClick(2);
        _itemButtons[3].Click = () => HealthClick(3);
        _itemButtons[4].Click = () => HealthClick(4);
        _itemButtons[5].Click = () => HealthClick(5);
        _itemButtons[6].Click = () => HealthClick(6);
        
        _itemButtons[0].Select = () => HealthSelect(0);
        _itemButtons[1].Select = () => HealthSelect(1);
        _itemButtons[2].Select = () => HealthSelect(2);
        _itemButtons[3].Select = () => HealthSelect(3);
        _itemButtons[4].Select = () => HealthSelect(4);
        _itemButtons[5].Select = () => HealthSelect(5);
        _itemButtons[6].Select = () => HealthSelect(6);
        
        MusicManager.Instance.PlaySound(_clickSFX);
    }

    private void Start()
    {
        if (Stats.Instance.Data[6] == 1)
        {
            var value = 0;

            for (int i = 11; i <= 21; i++)
            {
                if (Stats.Instance.Data[i] == 0)
                    value += 1;

                if (i == 17 && Stats.Instance.Data[i] == 0)
                    value += 2;
            }

            if (value > 0)
            {
                _locationLabel.text = $"Еще {value}";
                _locationLabel.color = Color.red;
            }
            else
            {
                _locationLabel.text = "???";
            }
        }
        else
        {
            if (!Meta.Instance.IsFoundCastle)
            {
                _teleportButtons[3].GetComponentInChildren<TextMeshPro>().text = "???";
            }
            
            if (!Meta.Instance.IsFoundDungeous)
            {
                _teleportButtons[2].GetComponentInChildren<TextMeshPro>().text = "???";
            }
        }
    }

    private void Update()
    {
        if (!_isInit)
        {
            _isInit = true;
            return;
        }

        if (_isMain)
        {
            _timeLabel.text = RealTimeClock.Instance.GetFormatTime;
            
            if (InputManager.Instance.HorizontalDown)
            {
                if (InputManager.Instance.Horizontal > 0)
                {
                    if (_mainIndex == 0)
                    {
                        MainSelect(1);
                    }
                    else if (_mainIndex == 2)
                    {
                        MainSelect(3);
                    }
                }
                else if (InputManager.Instance.Horizontal < 0)
                {
                    if (_mainIndex == 1)
                    {
                        MainSelect(0);
                    }
                    else if (_mainIndex == 3)
                    {
                        MainSelect(2);
                    }
                }
            }
            else if (InputManager.Instance.VerticalDown)
            {
                if (InputManager.Instance.Vertical > 0)
                {
                    if (_mainIndex == 2)
                    {
                        MainSelect(0);
                    }
                    else if (_mainIndex == 3)
                    {
                        MainSelect(1);
                    }
                }
                else if (InputManager.Instance.Vertical < 0)
                {
                    if (_mainIndex == 0)
                    {
                        MainSelect(2);
                    }
                    else if (_mainIndex == 1)
                    {
                        MainSelect(3);
                    }
                }
            }
        
            _heart.transform.localPosition = _mainIndex switch
            {
                0 => new Vector3(-3.75f, -0.16f),
                1 => new Vector3(0.375f, -0.16f),
                2 => new Vector3(-3.75f, -1f),
                3 => new Vector3(0.375f, -1f),
            };

            if (InputManager.Instance.IsSubmitDown)
            {
                MainClick(_mainIndex);
            }
        }
        else if (_isTeleport)
        {
            if (InputManager.Instance.VerticalDown)
            {
                if (InputManager.Instance.Vertical > 0)
                {
                    TeleportSelect(0);
                }
                else if (InputManager.Instance.Vertical < 0)
                {
                    TeleportSelect(2);
                }
            }
            else if (InputManager.Instance.HorizontalDown)
            {
                if (InputManager.Instance.Horizontal > 0)
                {
                    TeleportSelect(1);
                }
                else if (InputManager.Instance.Horizontal < 0)
                {
                    TeleportSelect(3);
                }
            }
            
            _heart.transform.localPosition = _teleportIndex switch
            {
                -1 => new Vector3(0, 0.483f),
                0 => new Vector3(-1.18f, 1.87f),
                1 => new Vector3(2.649f, 0.43f),
                2 => new Vector3(-2.086f, -0.9f),
                3 => new Vector3(-4.23f, 0.47f),
            };
            
            if (InputManager.Instance.IsSubmitDown)
            {
                TeleportClick(_teleportIndex);
            }
        }
        else if (_isInventory)
        {
            if (InputManager.Instance.VerticalDown)
            {
                if (InputManager.Instance.Vertical > 0)
                {
                    _inventoryIndex--;
                    
                }
                else if (InputManager.Instance.Vertical < 0)
                {
                    _inventoryIndex++;
                }

                if (_inventoryIndex < 0)
                    _inventoryIndex = 3;

                if (_inventoryIndex > 3)
                    _inventoryIndex = 0;
                
                InventorySelect(_inventoryIndex);
            }
            
            _heart.transform.localPosition = _inventoryIndex switch
            {
                0 => new Vector3(-5.13999987f, 3.07999992f,0),
                1 => new Vector3(-5.13999987f, 2.25999999f,0),
                2 => new Vector3(-5.13999987f, 1.43f),
                3 => new Vector3(-5.13999987f, 0.63f)
            };
            
            if (InputManager.Instance.IsSubmitDown)
            {
                InventoryClick(_inventoryIndex);
            }
        }
        else if (_isHealth)
        {
            if (InputManager.Instance.VerticalDown)
            {
                if (InputManager.Instance.Vertical > 0)
                {
                    _healthIndex--;

                    if (_healthIndex < 0)
                        _healthIndex = 0;
                }
                else if (InputManager.Instance.Vertical < 0)
                {
                    if (_healthIndex < 7)
                        _healthIndex++;
                }
                
                HealthSelect(_healthIndex);
            }
            
            if (InputManager.Instance.IsSubmitDown)
            {
                HealthClick(_healthIndex);
            }
            
            _heart.transform.localPosition = _healthIndex switch
            {
                0 => new Vector3(-0.610000014f, 3.07999992f, 0),
                1 => new Vector3(-0.610000014f, 2.25f, 0),
                2 => new Vector3(-0.610000014f, 1.41f, 0),
                3 => new Vector3(-0.610000014f, 0.6f, 0),
                4 => new Vector3(-0.610000014f, -0.26f, 0),
                5 => new Vector3(-0.610000014f, -0.96f, 0),
                6 => new Vector3(-0.610000014f, -1.81f, 0),
                7 => new Vector3(-0.610000014f, -2.6f, 0)
            };
        }
        else if (_isHealthSelect)
        {
            if (InputManager.Instance.HorizontalDown)
            {
                if (InputManager.Instance.Horizontal > 0)
                {
                    _healthSelectIndex = 1;
                }
                else if (InputManager.Instance.Horizontal < 0)
                {
                    _healthSelectIndex = 0;
                }
                
                HealthSelectSelect(_healthSelectIndex);
            }

            if (InputManager.Instance.IsSubmitDown)
            {
                HealthSelectClick(_healthSelectIndex);
            }
            
            _heart.transform.localPosition = _healthSelectIndex switch
            {
                0 => new Vector3(-0.610000014f, -2.6f, 0),
                1 => new Vector3(1.98f, -2.6f, 0),
            };
        }
        else if (_isItem)
        {
            if (InputManager.Instance.VerticalDown)
            {
                if (InputManager.Instance.Vertical > 0)
                {
                    _itemIndex--;

                    if (_itemIndex < 0)
                        _itemIndex = 0;
                }
                else if (InputManager.Instance.Vertical < 0)
                {
                    if (_itemIndex < 7)
                        _itemIndex++;
                }
                
                ItemSelect(_itemIndex);
            }
            
            if (InputManager.Instance.IsSubmitDown)
            {
                if (_itemIndex == 7)
                    HealthClick(_itemIndex);
            }
            
            _heart.transform.localPosition = _itemIndex switch
            {
                0 => new Vector3(-0.610000014f, 3.07999992f, 0),
                1 => new Vector3(-0.610000014f, 2.25f, 0),
                2 => new Vector3(-0.610000014f, 1.41f, 0),
                3 => new Vector3(-0.610000014f, 0.6f, 0),
                4 => new Vector3(-0.610000014f, -0.26f, 0),
                5 => new Vector3(-0.610000014f, -0.96f, 0),
                6 => new Vector3(-0.610000014f, -1.81f, 0),
                7 => new Vector3(-0.610000014f, -2.6f, 0)
            };
        }
        else if (_isMeta)
        {
            if (InputManager.Instance.VerticalDown)
            {
                if (InputManager.Instance.Vertical > 0)
                {
                    _metaIndex--;

                    if (_metaIndex < 0)
                        _metaIndex = 0;
                }
                else if (InputManager.Instance.Vertical < 0)
                {
                    if (_metaIndex < 7)
                        _metaIndex++;
                }
                
                MetaSelect(_metaIndex);
            }
            
            if (InputManager.Instance.IsSubmitDown)
            {
                if (_metaIndex == 7)
                    HealthClick(_metaIndex);
            }
            
            _heart.transform.localPosition = _metaIndex switch
            {
                0 => new Vector3(-0.610000014f, 3.07999992f, 0),
                1 => new Vector3(-0.610000014f, 2.25f, 0),
                2 => new Vector3(-0.610000014f, 1.41f, 0),
                3 => new Vector3(-0.610000014f, 0.6f, 0),
                4 => new Vector3(-0.610000014f, -0.26f, 0),
                5 => new Vector3(-0.610000014f, -0.96f, 0),
                6 => new Vector3(-0.610000014f, -1.81f, 0),
                7 => new Vector3(-0.610000014f, -2.6f, 0)
            };
        }
    }

    public void Init(int index)
    {
        _locationIndex =  index;
    }

    private void MainSelect(int index)
    {
        _selectSound.Play();
        _mainIndex = index;
    }

    private void MainClick(int index)
    {
        _mainIndex = index;
        MusicManager.Instance.PlaySound(_clickSFX);

        if (_mainIndex == 0)
        {
            _isMain = false; 
            _main.SetActive(false);
            _heart.SetActive(false);
            StartCoroutine(AwaitSave());
        }
        else if (_mainIndex == 1)
        {
            _isMain = false;
            _isTeleport = true;
            _main.SetActive(false);
            _teleport.SetActive(true);
        }
        else if (_mainIndex == 2)
        {
            _isMain = false;
            _isInventory = true;
            _main.SetActive(false);
            _inventory.SetActive(true);
            _frame.SetActive(false);
        }
        else if (_mainIndex == 3)
        {
            Destroy(gameObject);
            Player.Instance.enabled = true;
        }
    }

    private void TeleportSelect(int index)
    {
        _selectSound.Play();
        _teleportIndex = index;
    }

    private void TeleportClick(int index)
    {
        if (_locationIndex != _teleportIndex)
        {
            if (index == 0)
            {
                // Spawn
                SwitchingLevel.Switching("Spawn (0, 0)", new Vector3(0.400000006f,-4.26000023f,0));
                MusicManager.Instance.PlaySound(_clickSFX);
                Destroy(gameObject);
            }
            else if (index == 1)
            {
                // Home
                SwitchingLevel.Switching("Home", new Vector3(-2.20912743f, -6.60043097f, 0));
                MusicManager.Instance.PlaySound(_clickSFX);
                Destroy(gameObject);
            }
            else if (index == 2)
            {
                // Подземелье
                if (Meta.Instance.IsFoundDungeous || Stats.Instance.Data[6] == 1)
                {
                    MusicManager.Instance.PlaySound(_clickSFX);
                    SwitchingLevel.Switching("Dungeos Hub", new Vector3(0f, 0f, 0));
                    Destroy(gameObject);
                }
            }
            else if (index == 3)
            {
                // Крепость -> Старый спавн

                if (Meta.Instance.IsFoundCastle || Stats.Instance.Data[6] == 1)
                {
                    MusicManager.Instance.PlaySound(_clickSFX);
                    SwitchingLevel.Switching("Castle 1", new Vector3(0f, 0f, 0));
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            CoroutineRunner.Instance.StartCoroutine(AwaitMessage());
            Destroy(gameObject);
        }
    }

    private void InventorySelect(int inventoryIndex)
    {
        _selectSound.Play();
        _inventoryIndex = inventoryIndex;
    }

    private void MetaSelect(int inventoryIndex)
    {
        _selectSound.Play();
        _metaIndex = inventoryIndex;
    }
    
    private void InventoryClick(int index)
    {
        _inventoryIndex = index;
        MusicManager.Instance.PlaySound(_clickSFX);
        _inventoryRightButtons[1].gameObject.SetActive(false);
        _isHealth = false;
        _isHealthSelect = false;
        _isItem = false;
        _isMeta = false;
        _isInventory = true;
        _healthIndex = 0;
        _healthSelectIndex = 0;
        _metaIndex = 0;
        _itemIndex = 0;
            
        if (_inventoryIndex == 0)
        {
            _isInventory = false; 
            _inventoryRight.SetActive(true);
            _isHealth = true;
            
            for (int i = 0; i < _itemButtons.Length; i++)
            {
                var itemName = Stats.GetItemName(Stats.Instance.Items[i]);

                if (string.IsNullOrEmpty(itemName))
                    itemName = "(Пусто)";
                
                _itemButtons[i].GetComponent<TextMeshPro>().text = itemName;
            }
        }
        else if (_inventoryIndex == 1)
        {
            _isInventory = false; 
            _inventoryRight.SetActive(true);
            _isItem = true;
            
            for (int i = 0; i < _itemButtons.Length; i++)
            {
                _itemButtons[i].GetComponent<TextMeshPro>().text = Stats.Instance.OrdinaryItems.Count > i ? Stats.GetItemName(Stats.Instance.OrdinaryItems[i]) : "(Пусто)";
            }
        }
        else if (_inventoryIndex == 2)
        {
            _isInventory = false; 
            _inventoryRight.SetActive(true);
            _isMeta = true;
            
            for (int i = 0; i < _itemButtons.Length; i++)
            {
                _itemButtons[i].GetComponent<TextMeshPro>().text = "(Пусто)";

                if (i == 0 && Meta.Instance.IsEndEvil)
                {
                    _itemButtons[i].GetComponent<TextMeshPro>().text = "Золотой Билет";
                }
            }
        }
        else if (_inventoryIndex == 3)
        {
            _isInventory = false; 
            _isMain = true; 
            _frame.SetActive(true);
            _inventory.SetActive(false);
            _main.SetActive(true);
            _inventoryRight.SetActive(false);
            _inventoryRightButtons[1].gameObject.SetActive(false);
        }
    }

    private void HealthClick(int index)
    {
        if (_isItem)
        {
            _itemIndex = index;
        }
        
        if (_isMeta)
        {
            _metaIndex = index;
        }
        
        MusicManager.Instance.PlaySound(_clickSFX);
        
        if (!_isHealth && !_isHealthSelect && index != 7)
            return;
        
        _healthIndex = index;

        if (Stats.Instance.Items[_healthIndex] == null && _healthIndex != 7)
        {
            return;
        }
        
        _inventoryRightButtons[1].gameObject.SetActive(true);
        _isHealth = false;
        _isItem = false;
        _isHealthSelect = true;
        _isMeta = true;
        
        if (_healthIndex == 7)
        {
            _isHealthSelect = false; 
            _inventoryRight.SetActive(false);
            _inventoryRightButtons[1].gameObject.SetActive(false);
            _isHealth = false;
            _isInventory = true;
        }
    }

    private void HealthSelect(int inventoryIndex)
    {
        _selectSound.Play();

        if (_isItem)
        {
            _itemIndex = inventoryIndex;
        }
        
        if (_isMeta)
        {
            _metaIndex = inventoryIndex;
        }
        
        if (!_isHealth && !_isHealthSelect && inventoryIndex != 7)
            return;
        
        _healthIndex = inventoryIndex;
    }
    
    private void ItemSelect(int inventoryIndex)
    {
        _selectSound.Play();
        _itemIndex = inventoryIndex;
    }
    
    private void HealthSelectSelect(int inventoryIndex)
    {
        _selectSound.Play();
        _healthSelectIndex = inventoryIndex;
    }

    private void HealthSelectClick(int index)
    {
        _healthSelectIndex = index;

        if (_healthSelectIndex == 0)
        {
            _isHealthSelect = false; 
            _isHealth = true;
            MusicManager.Instance.PlaySound(_clickSFX);
            _inventoryRightButtons[1].gameObject.SetActive(false);
        }
        else if (_healthSelectIndex == 1)
        {
            if (Stats.Instance.Items[_healthIndex] != null)
            {
                MusicManager.Instance.PlaySound(_clickSFX);
                // Stats.Instance.RemoveItem();
                Stats.Instance.Items[_healthIndex] = string.Empty;
                _itemButtons[_healthIndex].GetComponent<TextMeshPro>().text = "(Пусто)";
                _isHealthSelect = false;
                _isHealth = true;
                _inventoryRightButtons[1].gameObject.SetActive(false);
            }
        }
    }
    
    private static IEnumerator AwaitMessage()
    {
        yield return DialogueWindow.StartDialogue(new string[] { "Удивительно, но вы уже здесь." }, false);
        Player.Instance.enabled = true;
    }

    private IEnumerator AwaitSave()
    {
        _saveLabel.gameObject.SetActive(true);

        var location = _locationIndex switch
        {
            0 => "на спавне",
            1 => "в доме",
            2 => "в старом спавне",
            3 => "в крепосте",
        };

        Stats.Instance.Data[3] = _locationIndex;
        _saveLabel.text = $"Вы установили точку возрождения {location}.";
        yield return new WaitForSeconds(0.5f);
        yield return InputManager.Instance.AwaitSubmitDownOrClick();
        Destroy(gameObject);
        Player.Instance.enabled = true;
    }
}
