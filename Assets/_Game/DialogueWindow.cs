using System;
using System.Collections;
using System.Collections.Generic;
using Febucci.TextAnimatorCore.Text;
using Febucci.TextAnimatorForUnity;
using TMPro;
using UnityEngine;

public class DialogueWindow : MonoBehaviour
{
    public static DialogueWindow Instance;
    
    [SerializeField]
    private TypewriterComponent _label;
    
    [SerializeField]
    private TypewriterComponent _dialogueLabel;

    [SerializeField]
    private Animator _animator;
    
    [SerializeField]
    private GameObject[] _speakers;

    /*
     * 0 - System
     * 1 - Normal
     * 2 - Noobik
     * 3 - Herobrine
     * 4 - Squid Boss
     * 5 - Palisos
     * 6 - Grox
     * 7 - Girl Voice
     */
    [SerializeField]
    private AudioClip[] _clips;
    
    private string[] _replicas;
    private TypewriterComponent _currentLabel;
    private bool _isSkip;
    private AudioSource _sfx;

    public static IEnumerator StartDialogue(string[] dialogues, bool isDown)
    {
        var dialogueWindow = Instantiate(Resources.Load<DialogueWindow>("Dialogue Window"),
            new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + (isDown ? -3.4f : 2.884f)),
            Camera.main.transform.rotation);

        Instance = dialogueWindow;
        Instance._replicas = dialogues;
        // Instance._label.GetComponent<TextMeshPro>().text = string.Empty;

        yield return dialogueWindow.AwaitWrite();
    }

    private void Update()
    {
        if (InputManager.Instance.IsCancelDown) //  || Input.GetMouseButtonDown(0)
            _isSkip = true;
    }

    private IEnumerator AwaitWrite()
    {
        _sfx = GetComponent<AudioSource>();
        var createdLabels = new List<TypewriterComponent>();
        
        foreach (var replica in _replicas)
        {
            _isSkip = false;
            var currentReplica = replica;
            var isDialogue = false;
            _sfx.clip = _clips[0];
            
            if (replica[0] == '/')
            {
                _animator.Play("Head Container", 0, 0f);
                
                isDialogue = true;
                _sfx.clip = _clips[1];
                
                if (replica[1] == 'h') // Hacker
                {
                    if (replica[2] == '0')
                    {
                        _speakers[2].SetActive(true);
                    }
                }
                
                if (replica[1] == 'n') // Noobik
                {
                    if (replica[2] == '0')
                    {
                        _speakers[0].SetActive(true);
                        _sfx.clip = _clips[2];
                    }
                }
                
                if (replica[1] == 'c') // cat
                {
                    if (replica[2] == '0')
                    {
                        _speakers[1].SetActive(true);
                        _speakers[1].GetComponent<AudioSource>().Play();
                        _sfx.clip = null;
                    }
                }
                
                if (replica[1] == 't') // notch
                {
                    if (replica[2] == '0')
                    {
                        _speakers[3].SetActive(true);
                    }
                }
                
                if (replica[1] == 'f') // Fir
                {
                    if (replica[2] == '0')
                    {
                        _speakers[4].SetActive(true);
                    }
                }
                
                if (replica[1] == 'y') // herobrine
                {
                    if (replica[2] == '0')
                    {
                        _speakers[5].SetActive(true);
                        _sfx.clip = _clips[3];
                    }
                }
                
                if (replica[1] == 'b') // bisnesmen
                {
                    if (replica[2] == '0')
                    {
                        _speakers[6].SetActive(true);
                    }
                }
                
                if (replica[1] == 'g') // Grum
                {
                    if (replica[2] == '0')
                    {
                        _speakers[7].SetActive(true);
                    }
                    
                    if (replica[2] == '1')
                    {
                        _speakers[13].SetActive(true);
                    }
                }
                
                if (replica[1] == 'i') // Fivfiv
                {
                    if (replica[2] == '0')
                    {
                        _speakers[8].SetActive(true);
                    }
                }
                
                if (replica[1] == 'a') // Admin
                {
                    if (replica[2] == '0')
                    {
                        _speakers[9].SetActive(true);
                    }
                }
                
                if (replica[1] == 'e') // Hero
                {
                    if (replica[2] == '0')
                    {
                        _speakers[10].SetActive(true);
                    }
                }
                
                if (replica[1] == 's') // Squid Boss
                {
                    if (replica[2] == '0')
                    {
                        _speakers[11].SetActive(true);
                        _sfx.clip = _clips[4];
                    }
                }
                
                if (replica[1] == 'p') // Palisos
                {
                    if (replica[2] == '0')
                    {
                        _speakers[12].SetActive(true);
                        _sfx.clip = _clips[5];
                    }
                }
                
                if (replica[1] == 'k') // Skeleton
                {
                    if (replica[2] == '0')
                    {
                        _speakers[14].SetActive(true);
                    }
                }
                
                if (replica[1] == 'z') // enderman
                {
                    if (replica[2] == '0')
                    {
                        _speakers[15].SetActive(true);
                    }
                }
                
                if (replica[1] == 'v') // villager
                {
                    if (replica[2] == '0')
                    {
                        _speakers[16].SetActive(true);
                    }
                }
                
                if (replica[1] == 'q') // witch
                {
                    if (replica[2] == '0')
                    {
                        _speakers[17].SetActive(true);
                    }
                }
                
                if (replica[1] == 'w') // wisman
                {
                    if (replica[2] == '0')
                    {
                        _speakers[18].SetActive(true);
                        _sfx.clip = _clips[6];
                    }
                }
                
                if (replica[1] == 'j') // nastya
                {
                    if (replica[2] == '0')
                    {
                        _speakers[19].SetActive(true);
                        _sfx.clip = _clips[7];
                    }
                    else if (replica[2] == '1')
                    {
                        _speakers[20].SetActive(true);
                        _sfx.clip = _clips[7];
                    }
                }
                
                if (replica[1] == 'd') // dog
                {
                    _sfx.clip = null;
                    
                    if (replica[2] == '0')
                    {
                        _speakers[21].SetActive(true);
                    }
                    else if (replica[2] == '1')
                    {
                        _speakers[22].SetActive(true);
                    }
                }
                
                if (replica[1] == 'l') // snow
                {
                    _sfx.clip = null;
                    
                    if (replica[2] == '0')
                    {
                        _speakers[23].SetActive(true);
                    }
                    else if (replica[2] == '1')
                    {
                        _speakers[24].SetActive(true);
                    }
                }
                
                if (replica[1] == 'u') // slime
                {
                    _sfx.clip = null;
                    
                    if (replica[2] == '0') // mini
                    {
                        _speakers[25].SetActive(true);
                    }
                }
                
                if (replica[1] == 'o') // sheep
                {
                    if (replica[2] == '0')
                    {
                        _speakers[26].SetActive(true);
                    }
                }
                
                currentReplica = replica.Substring(3);
            }

            if (isDialogue)
            {
                _label.gameObject.SetActive(false);
                _dialogueLabel.gameObject.SetActive(true);
                _currentLabel = _dialogueLabel;
            }
            else
            {
                _label.gameObject.SetActive(true);
                _dialogueLabel.gameObject.SetActive(false);
                _currentLabel  = _label;
            }
            
            string[] parts = currentReplica.Split('*', StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < parts.Length; i++)
            {
                var part = parts[i];

                if (i != 0)
                {
                    var previousLabel = _currentLabel;
                    
                    _currentLabel = Instantiate(Resources.Load<TypewriterComponent>(
                        isDialogue ? "Dialogue Label" : "Monologue Label"), transform);
                    createdLabels.Add(_currentLabel);
                    var textMesh = previousLabel.GetComponentInChildren<TextMeshPro>();

                    _currentLabel.transform.position = new Vector3(previousLabel.transform.position.x,
                        previousLabel.transform.position.y - textMesh.renderedHeight,
                        previousLabel.transform.position.z); // -1

                    _currentLabel.GetComponentInChildren<TextMeshPro>().text = string.Empty;
                }

                _currentLabel.StopShowingText();
                _currentLabel.ShowText(null);
                _currentLabel.ShowText(part);
                _currentLabel.StartShowingText();
                _currentLabel.onCharacterVisible.AddListener(OnWrite);

                while (_currentLabel.IsShowingText && !_isSkip)
                {
                    yield return null;
                }

                if (_isSkip)
                    _currentLabel.SkipTypewriter();

                // var timer = 0.25f;
                //
                // while (timer > 0)
                // {
                //     timer -= Time.deltaTime;
                //
                //     if (!_isSkip)
                //         yield return null;
                // }
            }

            _isSkip = false;
            yield return null;
            yield return InputManager.Instance.AwaitSubmitDownOrClick();
            
            //_label.ShowText(replica);
            //_label.StartShowingText();
            
            // _textAnimatorPlayer.onCharacterVisible.AddListener((c) => OnWrite());
            // _textAnimatorPlayer.onTextShowed.AddListener(Stop);

            for (int i = 0; i < createdLabels.Count; i++)
            {
                Destroy(createdLabels[i].gameObject);
            }

            createdLabels = new List<TypewriterComponent>();
            
            foreach (var speaker in _speakers)
            {
                speaker.SetActive(false);
            }
        }
        
        Destroy(gameObject);
    }

    private void OnWrite(CharacterData arg0)
    {
        var character = arg0.info.character;
        
        if (character == ' ' || character == '.' || character == ',')
            return;
        
        if (!_sfx.isPlaying)
            _sfx.Play();
    }
}
