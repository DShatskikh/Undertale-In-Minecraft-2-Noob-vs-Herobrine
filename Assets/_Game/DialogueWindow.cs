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
    private GameObject[] _speakers;

    /*
     * 0 - System
     * 1 - Normal
     * 2 - Noobik
     * 3 - Herobrine
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
        if (InputManager.Instance.IsCancelDown)
            _isSkip = true;
    }

    private IEnumerator AwaitWrite()
    {
        _sfx = GetComponent<AudioSource>();
        var createdLabels = new List<TypewriterComponent>();
        
        foreach (var replica in _replicas)
        {
            var currentReplica = replica;
            var isDialogue = false;
            _sfx.clip = _clips[0];
            
            if (replica[0] == '/')
            {
                isDialogue = true;
                _sfx.clip = _clips[1];
                
                if (replica[1] == 'h')
                {
                    if (replica[2] == '0')
                    {
                        _speakers[2].SetActive(true);
                    }
                }
                
                if (replica[1] == 'n')
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
                    _currentLabel = Instantiate(previousLabel, transform);
                    createdLabels.Add(_currentLabel);
                    var textMesh = _currentLabel.GetComponentInChildren<TextMeshPro>();

                    _currentLabel.transform.position = new Vector3(previousLabel.transform.position.x,
                        previousLabel.transform.position.y - textMesh.renderedHeight,
                        previousLabel.transform.position.z); // -1

                    textMesh.text = string.Empty;
                }

                _currentLabel.ShowText(part);
                _currentLabel.StartShowingText();
                _currentLabel.onCharacterVisible.AddListener(OnWrite);

                while (_currentLabel.IsShowingText && !_isSkip)
                {
                    yield return null;
                }

                if (_isSkip)
                    _currentLabel.SkipTypewriter();

                var timer = 0.25f;

                while (timer > 0)
                {
                    timer -= Time.deltaTime;

                    if (!_isSkip)
                        yield return null;
                }
            }

            _isSkip = false;
            yield return null;
            yield return new WaitUntil(() => InputManager.Instance.IsSubmitDown);
            
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
        if (!_sfx.isPlaying)
            _sfx.Play();
    }
}
