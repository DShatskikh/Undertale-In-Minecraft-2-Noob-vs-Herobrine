using System.Collections;
using UnityEngine;

public sealed class SaveWindow : MonoBehaviour
{
    [SerializeField]
    private GameObject _heart;

    [SerializeField]
    private GameObject _main;
    
    [SerializeField]
    private GameObject _teleport;
    
    [SerializeField]
    private GameObject _saveLabel;
    
    [SerializeField]
    private TextButton[] _mainButtons;
    
    [SerializeField]
    private TextButton[] _teleportButtons;
    
    [SerializeField]
    private AudioSource _selectSound;
    
    [SerializeField]
    private AudioClip _clickSFX;
    
    private int _mainIndex;
    private int _teleportIndex = -1;
    private bool _isMain = true;
    private bool _isTeleport;
    private bool _isInit;
    private int _locationIndex;

    private void Awake()
    {
        _mainButtons[0].Select = () => MainSelect(0);
        _mainButtons[1].Select = () => MainSelect(1);
        _mainButtons[2].Select = () => MainSelect(2);
        _mainButtons[3].Select = () => MainSelect(3);
        
        _mainButtons[0].Click = () => MainClick(0);
        _mainButtons[1].Click = () => MainClick(1);
        _mainButtons[2].Click = () => MainClick(2);
        _mainButtons[3].Click = () => MainClick(3);
        
        MusicManager.Instance.PlaySound(_clickSFX);
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
                0 => new Vector3(-0.88f, 1.87f),
                1 => new Vector3(1.56f, 0.43f),
                2 => new Vector3(-1.2f, -0.9f),
                3 => new Vector3(-4.23f, 0.47f),
            };
            
            if (InputManager.Instance.IsSubmitDown)
            {
                TeleportClick(_teleportIndex);
            }
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
        MusicManager.Instance.PlaySound(_clickSFX);
        
        if (_locationIndex != _teleportIndex)
        {
            Debug.Log(index);
            
            if (index == 0)
            {
                // Spawn
                SwitchingLevel.Switching("Spawn (0, 0)", new Vector3(0.400000006f,-4.26000023f,0));
            }
            else if (index == 1)
            {
                // Home
                SwitchingLevel.Switching("Home", new Vector3(-2.20912743f, -6.60043097f, 0));
            }
            else if (index == 2)
            {
                // Комната Р.
            }
            else if (index == 3)
            {
                // Герой
                // Старый спавн
            }
        }
        else
        {
            CoroutineRunner.Instance.StartCoroutine(AwaitMessage());
        }
        
        Destroy(gameObject);
    }

    private static IEnumerator AwaitMessage()
    {
        yield return DialogueWindow.StartDialogue(new string[] { "Удивительно, но вы уже здесь" }, false);
        Player.Instance.enabled = true;
    }
    
    private IEnumerator AwaitSave()
    {
        _saveLabel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => InputManager.Instance.IsSubmitDown);
        Destroy(gameObject);
        Player.Instance.enabled = true;
    }
}
