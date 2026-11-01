using System.Collections;
using Febucci.TextAnimatorForUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Outro_2 : MonoBehaviour
{
    [SerializeField]
    private TypewriterComponent _label;

    [SerializeField]
    private TextButton _button;

    [SerializeField]
    private SpriteRenderer _blackout;
    
    [SerializeField]
    private GameObject[] _slides;
    
    private bool _isSkip;
    private float _timer = 1;
    private int _slideIndex = -1;
    
    private IEnumerator Start()
    {
        _button.gameObject.SetActive(false);
        Heart.Instance.enabled = false;
        Heart.Instance.gameObject.SetActive(false);
        
        _button.Click += Next;
        
        _label.ShowText("");

        yield return new WaitForSeconds(1);

        Heart.Instance.gameObject.SetActive(true);
        _button.gameObject.SetActive(true);
        Next();
    }

    private void Update()
    {
        if (_isSkip)
        {
            _blackout.gameObject.SetActive(true);
            // _button.gameObject.SetActive(false);
            _blackout.color = _blackout.color.AddA(Time.deltaTime);
            
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
            }
            else
            {
                Stats.Instance.Position = new Vector3(1.20000005f, -5.37934971f, 0);
                Stats.Instance.LevelName = "Home";
                SceneManager.LoadScene("Overworld");
                SceneManager.LoadScene("Home", LoadSceneMode.Additive);
            }
        }
        
        if (InputManager.Instance.IsSubmitDown)
        {
            Next();
        }
    }

    private void Next()
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
            0 => "Котик, пошел проходить игру.",
            1 => "Он подружился со всеми противниками.",
            2 => "И одолел Херобрина.",
            3 => "Конец...",
        };
        
        _label.ShowText(message);
        _label.StartShowingText();
    }
    
    public void Skip()
    {
        _isSkip = true;
    }
}
