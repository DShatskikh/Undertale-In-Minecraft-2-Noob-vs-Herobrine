using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ExitMenu : MonoBehaviour
{
    private void Start()
    {
        if (!Meta.Instance.IsEndOldSpawn)
        {
            Destroy(gameObject);
            return;
        }

        var button = GetComponent<UIDocument>().rootVisualElement.Q<Button>("Menu");
        button.focusable = false;
        
        button.clicked += () =>
        {
            SceneManager.LoadScene("Menu");
        };       
    }
}
