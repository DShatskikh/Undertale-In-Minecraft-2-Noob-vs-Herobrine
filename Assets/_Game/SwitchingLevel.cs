using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class SwitchingLevel : MonoBehaviour
{
    [SerializeField]
    private string _sceneToLoad;

    [SerializeField]
    private Vector3 _position;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            Switching(_sceneToLoad, _position);
        }
    }

    public static void Switching(string sceneToLoad, Vector3 position)
    {
        CoroutineRunner.Instance.StartCoroutine(AwaitSwitching(sceneToLoad, position));
    }
    
    public static IEnumerator AwaitSwitching(string sceneToLoad, Vector3 position)
    {
        Player.Instance.enabled = false;
        
        var blackout = Instantiate(Resources.Load<SpriteRenderer>("Blackout"));
        SceneManager.MoveGameObjectToScene(blackout.gameObject, SceneManager.GetSceneByName("Overworld"));
        blackout.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y);
        var color = blackout.color;
        color.a = 0;
        blackout.color = color;
        
        while (blackout.color.a < 1)
        {
            yield return null;
            color = blackout.color;
            color.a += Time.deltaTime * 2;
            blackout.color = color;
        }
        
        SceneManager.UnloadSceneAsync(Stats.Instance.LevelName);
        OverworldCamera.Instance.IsUpLimit = false;
        OverworldCamera.Instance.IsDownLimit = false;
        OverworldCamera.Instance.IsRightLimit = false;
        OverworldCamera.Instance.IsLeftLimit = false;
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
        blackout.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y);
        
        Stats.Instance.LevelName = sceneToLoad;
        Player.Instance.transform.position = position;
        
        while (blackout.color.a > 0)
        {
            yield return null;
            color = blackout.color;
            color.a -= Time.deltaTime * 2;
            blackout.color = color;
            blackout.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y);
        }
        
        Destroy(blackout.gameObject);
        Player.Instance.enabled = true;
    }
}
