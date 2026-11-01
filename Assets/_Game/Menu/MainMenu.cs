using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _main, _server, _quest;

    [SerializeField]
    private bool _isMain, _isServer, _isQuest, _isBackgroundRight, _isServerSelected, _isServerDelete, _isServerAllDelete;
    
    [SerializeField]
    private int _mainIndex, _serverIndex, _selectedServerIndex, _serverDeleteIndex, _serverAllDeleteIndex;

    [SerializeField]
    private Transform _heart, _background;

    [SerializeField]
    private GameObject[] _mainIcons, _serverFrames;

    [SerializeField]
    private GameObject _serverIcon;
    
    [SerializeField]
    private AudioSource _sfxSelect, _sfxClick;

    [SerializeField]
    private TextButton[] _mainButtons, _serverButtons, _questButtons, _serverSelectButtons, _serverDeleteButtons, 
        _serverAllDeleteButtons;

    [SerializeField]
    private GameObject _developerServerNotAvailable, _developerServerAvailable;
    
    [SerializeField]
    private GameObject _serverDelete, _serverAllDelete;
    
    private bool _isAccessDeveloper = Meta.Instance.IsEndTheatre;
    
    private void Start()
    {
        _selectedServerIndex = -1;

        ToggleSelectedServerButtons(false);
        
        _mainButtons[0].Click += OnClickPlay;
        _mainButtons[1].Click += OnClickQuest;
        _mainButtons[2].Click += OnClickURL;
        
        _serverButtons[0].Click += () => OnClickServer(0);
        _serverButtons[1].Click += () => OnClickServer(1);
        _serverButtons[2].Click += OnClickExit;
        
        _serverSelectButtons[0].Click += () => OnClickServerAction(0);
        _serverSelectButtons[1].Click += () => OnClickServerAction(1);
        _serverSelectButtons[2].Click += () => OnClickServerAction(2);
        
        _questButtons[0].Click += OnClickExit;
        
        _developerServerNotAvailable.SetActive(!_isAccessDeveloper);
        _developerServerAvailable.SetActive(_isAccessDeveloper);

        _serverDeleteButtons[0].Click += () => OnClickServerDelete(0);
        _serverDeleteButtons[1].Click += () => OnClickServerDelete(1);
        
        _serverAllDeleteButtons[0].Click += () => OnClickServerAllDelete(0);
        _serverAllDeleteButtons[1].Click += () => OnClickServerAllDelete(1);
    }

    private void Update()
    {
        if (!_isBackgroundRight)
        {
            if (_background.transform.position.x < 8)
            {
                _background.position = _background.position.AddX(Time.deltaTime / 2);
            }
            else
            {
                _isBackgroundRight = true;
            }
        }
        else
        {
            if (_background.transform.position.x > -8)
            {
                _background.position = _background.position.AddX(-Time.deltaTime / 2);
            }
            else
            {
                _isBackgroundRight = false;
            }
        }
        
        if (_isMain)
        {
            if (InputManager.Instance.VerticalDown)
            {
                _sfxSelect.Play();
                
                if (InputManager.Instance.Vertical > 0)
                {
                    _mainIndex--;
                }
                else
                {
                    _mainIndex++;
                }
            }

            if (InputManager.Instance.IsSubmitDown)
            {
                if (_mainIndex == 0)
                {
                    OnClickPlay();
                }
                else if (_mainIndex == 1)
                {
                    OnClickQuest();
                }
                else
                {
                    OnClickURL();
                }
            }

            if (_mainIndex > 2)
                _mainIndex = 0;

            if (_mainIndex < 0)
                _mainIndex = 2;

            _heart.position = _mainIndex switch
            {
                0 => new Vector2(-2.24f, 0.88f),
                1 => new Vector2(-2.24f, -0.32f),
                2 => new Vector2(-2.24f, -1.51f),
            };

            foreach (var mainIcon in _mainIcons)
            {
                mainIcon.SetActive(true);
            }
            
            _mainIcons[_mainIndex].SetActive(false);
        }
        else if (_isServer)
        {
            if (InputManager.Instance.VerticalDown)
            {
                _sfxSelect.Play();
                
                if (InputManager.Instance.Vertical > 0)
                {
                    _serverIndex--;
                }
                else
                {
                    _serverIndex++;
                }
            }
            
            if (InputManager.Instance.IsSubmitDown)
            {
                if (_serverIndex == 2)
                {
                    OnClickExit();
                }
                else if (_serverIndex == 0)
                {
                    OnClickServer(0);
                }
                else
                {
                    OnClickServer(1);
                }
            }
            
            if (_serverIndex > 2)
                _serverIndex = 2;

            if (_serverIndex < 0)
                _serverIndex = 0;
            
            _heart.position = _serverIndex switch
            {
                0 => new Vector2(-5.81f, 3.01f),
                1 => new Vector2(-5.81f, 1.29f),
                2 => new Vector2(-2.22f, -4.09f),
            };

            _serverIcon.SetActive(_serverIndex != 2);
        }
        else if (_isServerSelected)
        {
            foreach (var frame in _serverFrames)
            {
                frame.SetActive(false);
            }
            
            if (_serverIndex == 0)
            {
                _serverFrames[0].SetActive(true);
            }
            else if (_serverIndex == 1)
            {
                _serverFrames[1].SetActive(true);
            }
            
            if (InputManager.Instance.HorizontalDown)
            {
                _sfxSelect.Play();
                
                if (InputManager.Instance.Horizontal > 0)
                {
                    _selectedServerIndex++;
                }
                else
                {
                    _selectedServerIndex--;
                }
            }

            if (InputManager.Instance.IsSubmitDown)
            {
                OnClickServerAction(_selectedServerIndex);
            }

            if (_selectedServerIndex > 2)
                _selectedServerIndex = 0;

            if (_selectedServerIndex < 0)
                _selectedServerIndex = 2;
            
            _heart.position = _selectedServerIndex switch
            {
                0 => new Vector3(-4.67999983f, -2.48000002f, 0),
                1 => new Vector3(-0.765f, -2.48000002f, 0),
                2 => new Vector3(3.215f, -2.48000002f, 0),
            };
        }
        else if (_isQuest)
        {
            _heart.position = new Vector2(-2.22f, -4.09f);
            
            if (InputManager.Instance.IsSubmitDown)
            {
                OnClickExit();
            }
        }
        else if (_isServerDelete)
        {
            if (InputManager.Instance.HorizontalDown)
            {
                _sfxSelect.Play();
                
                if (InputManager.Instance.Horizontal > 0)
                {
                    _serverDeleteIndex++;
                }
                else
                {
                    _serverDeleteIndex--;
                }
            }
            
            if (InputManager.Instance.IsSubmitDown)
            {
                OnClickServerDelete(_serverDeleteIndex);
            }
            
            if (_serverDeleteIndex > 1)
                _serverDeleteIndex = 0;

            if (_serverDeleteIndex < 0)
                _serverDeleteIndex = 1;
            
            _heart.position = _serverDeleteIndex switch
            {
                0 => new Vector3(-3.54f, -2.47f, 0),
                1 => new Vector3(0.84f, -2.47f, 0)
            };
        }
        else if (_isServerAllDelete)
        {
            if (InputManager.Instance.HorizontalDown)
            {
                _sfxSelect.Play();
                
                if (InputManager.Instance.Horizontal > 0)
                {
                    _serverAllDeleteIndex++;
                }
                else
                {
                    _serverAllDeleteIndex--;
                }
            }
            
            if (InputManager.Instance.IsSubmitDown)
            {
                OnClickServerAllDelete(_serverAllDeleteIndex);
            }
            
            if (_serverAllDeleteIndex > 1)
                _serverAllDeleteIndex = 0;

            if (_serverAllDeleteIndex < 0)
                _serverAllDeleteIndex = 1;
            
            _heart.position = _serverAllDeleteIndex switch
            {
                0 => new Vector3(-3.54f, -2.47f, 0),
                1 => new Vector3(0.84f, -2.47f, 0)
            };
        }
    }

    private void OnClickServerAllDelete(int index)
    {
        _serverAllDeleteIndex = index;

        if (_serverAllDeleteIndex == 0)
        {
            
        }
        else
        {
            _isServerSelected = true;
            _server.SetActive(true);
            _isServerAllDelete = false;
            _serverAllDelete.SetActive(false);
        }
    }
    
    private void OnClickServerDelete(int index)
    {
        _serverDeleteIndex = index;

        if (_serverDeleteIndex == 0)
        {
            
        }
        else
        {
            _isServerSelected = true;
            _server.SetActive(true);
            _isServerDelete = false;
            _serverDelete.SetActive(false);
        }
    }

    private void OnClickURL()
    {
        Application.OpenURL("https://yandex.ru/games/developer/60379");
    }

    private void OnClickServer(int index)
    {
        if (index == 1 && !Meta.Instance.IsEndTheatre)
        {
            return;
        }
        
        _selectedServerIndex = 0;
        _serverIndex = index;
        _isServer = false;
        _isServerSelected = true;
        
        ToggleSelectedServerButtons(true);
    }

    private void OnClickServerAction(int index)
    {
        if (!_isServerSelected)
            return;
        
        _selectedServerIndex = index;
        
        if (_serverIndex == 0)
        {
            if (_selectedServerIndex == 0)
            {
                if (Stats.Instance.Data[0] == 1) // Загружаем
                {
                    SwitchingLevel.SwitchingNotWorld();
                }
                else // Новая игра
                {
                    SceneManager.LoadScene("Intro", LoadSceneMode.Single);
                }
            }
            else if (_selectedServerIndex == 1) // Stats.Instance.Data[0] == 1
            {
                _isServerSelected = false;
                _server.SetActive(false);
                _isServerDelete = true;
                _serverDelete.SetActive(true);
            }
            else if (_selectedServerIndex == 2)
            {
                _isServerSelected = false;
                _server.SetActive(false);
                _isServerAllDelete = true;
                _serverAllDelete.SetActive(true);
            }
        }
        else
        {
            if (_selectedServerIndex == 0)
            {
                if (!_isAccessDeveloper)
                    return;
        
                SwitchingLevel.SwitchingNotWorld("Developer Room", new Vector3(0, -8.519996f, 0));
            }
        }
    }

    private void OnClickPlay()
    {
        _sfxClick.Play();
        
        _isMain = false;
        _isServer = true;
                
        _main.SetActive(false);
        _server.SetActive(true);
        
        _mainIndex = 0;
    }

    private void OnClickQuest()
    {
        _sfxClick.Play();
        
        _isMain = false;
        _isServer = false;
        _isQuest = true;
                
        _main.SetActive(false);
        _server.SetActive(false);
        _quest.SetActive(true);
        
        _mainIndex = 1;
    }

    private void OnClickExit()
    {
        _sfxClick.Play();
        
        _isMain = true;
        _isServer = false;
        _isQuest = false;
        _isServerSelected = false;

        ToggleSelectedServerButtons(false);
        
        _main.SetActive(true);
        _server.SetActive(false);
        _quest.SetActive(false);

        _serverIndex = 2;
    }

    private void ToggleSelectedServerButtons(bool isSelected)
    {
        if (isSelected)
        {
            for (var i = 0; i < _serverSelectButtons.Length; i++)
            {
                if (_serverIndex == 0)
                {
                    if (i == 1 && Stats.Instance.Data[0] == 1)
                        continue;
                    
                    if (i == 2) // && Meta.Instance.IsEndTheatre
                        continue;
                }
                
                if (_serverIndex == 1 && i != 0)
                {
                    continue;
                }
                
                var serverSelectButton = _serverSelectButtons[i];
                serverSelectButton.GetComponent<SpriteRenderer>().color = Color.white;

                serverSelectButton.GetComponentsInChildren<SpriteRenderer>()[1].color
                    = serverSelectButton.GetComponentsInChildren<SpriteRenderer>()[1].color.SetA(1f);

                serverSelectButton.GetComponentsInChildren<TextMeshPro>()[0].color
                    = serverSelectButton.GetComponentsInChildren<TextMeshPro>()[0].color.SetA(1f);
            }
        }
        else
        {
            foreach (var frame in _serverFrames)
            {
                frame.SetActive(false);
            }
            
            foreach (var serverSelectButton in _serverSelectButtons)
            {
                serverSelectButton.GetComponent<SpriteRenderer>().color = Color.gray;
                serverSelectButton.GetComponentsInChildren<SpriteRenderer>()[1].color
                    = serverSelectButton.GetComponentsInChildren<SpriteRenderer>()[1].color.SetA(0.5f);
                serverSelectButton.GetComponentsInChildren<TextMeshPro>()[0].color
                    = serverSelectButton.GetComponentsInChildren<TextMeshPro>()[0].color.SetA(0.5f);
            }
        }
    }
}
