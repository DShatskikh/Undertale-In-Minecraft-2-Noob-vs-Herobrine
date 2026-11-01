using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class BanMenu : MonoBehaviour
{
    [SerializeField]
    private TextButton _button;
    
    private void Start()
    {
        _button.Click += ResetSave;
    }

    private void Update()
    {
        if (InputManager.Instance.IsSubmitDown)
        {
            ResetSave();
        }
    }

    private void ResetSave()
    {
        SceneManager.LoadScene("Menu");
    }
}
