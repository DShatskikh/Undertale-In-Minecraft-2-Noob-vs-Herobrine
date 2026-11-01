using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class StartBattleScreen : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _background;
    
    [SerializeField]
    private Transform _heart;

    [SerializeField]
    private AudioSource _noiseSFX;
    
    [SerializeField]
    private AudioSource _battleFallSFX;
    
    public static void Transition(string enemyName, string enemyName2 = null, string enemyName3 = null)
    {
        var startBattleScreen = Instantiate(Resources.Load<StartBattleScreen>("StartBattleScreen"), 
            new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y), Quaternion.identity);

        CoroutineRunner.Instance.StartCoroutine(startBattleScreen.AwaitTransition(enemyName, enemyName2, enemyName3));
    }
    
    private IEnumerator AwaitTransition(string enemyName, string enemyName2 = null, string enemyName3 = null)
    {
        Stats.Instance.Position = Player.Instance.transform.position;
        
        Player.Instance.enabled = false;
        _background.enabled = false;
        Player.Instance.Danger.SetActive(true);
        _heart.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        Player.Instance.Danger.SetActive(false);
        _background.enabled = true;
        Player.Instance.GetComponentInChildren<SpriteRenderer>().sortingOrder = 19;
        _heart.position = Player.Instance.transform.position + new Vector3(0f, 0.4f);
        _heart.gameObject.SetActive(true);

        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.1f);
            _heart.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            _heart.gameObject.SetActive(true);
            _noiseSFX.Play();
        }

        yield return new WaitForSeconds(0.1f);
        _battleFallSFX.Play();
        Player.Instance.gameObject.SetActive(false);

        var koef = 5f / 6f;
        var endPosition = new Vector2(-6.69999981f * koef, -5.34499979f * koef);

        while (endPosition != (Vector2)_heart.localPosition)
        {
            yield return null;
            _heart.localPosition = Vector2.MoveTowards(_heart.localPosition, endPosition, Time.deltaTime * 9);
        }
        
        BattleManager.IsStartBlackout = true;
        yield return  SceneManager.UnloadSceneAsync(Stats.Instance.LevelName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        yield return  SceneManager.LoadSceneAsync("Battle",  LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Battle"));
        yield return  SceneManager.UnloadSceneAsync("Overworld", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        
        var enemy = Instantiate(Resources.Load<GameObject>(enemyName));
        
        SceneManager.MoveGameObjectToScene(enemy, SceneManager.GetSceneByName("Battle"));
        
        if (enemyName2 != null)
        {
            var enemy2 = Instantiate(Resources.Load<GameObject>(enemyName2));
            SceneManager.MoveGameObjectToScene(enemy2, SceneManager.GetSceneByName("Battle"));
        }
        
        if (enemyName3 != null)
        {
            var enemy3 = Instantiate(Resources.Load<GameObject>(enemyName3));
            SceneManager.MoveGameObjectToScene(enemy3, SceneManager.GetSceneByName("Battle"));
        }

        var backgroundName = "PVP Background";

        if (enemyName == "Grum")
        {
            backgroundName = "PVP Background Inverted Variant";
        }
        else  if (enemyName == "Mob Arena")
        {
            backgroundName = "Mob Arena Background";
        }

        if (enemyName != "Arrow Enemy" && enemyName != "Hero")
        {
            var background = Instantiate(Resources.Load<GameObject>(backgroundName));
            SceneManager.MoveGameObjectToScene(background, SceneManager.GetSceneByName("Battle"));
        }
    }
}
