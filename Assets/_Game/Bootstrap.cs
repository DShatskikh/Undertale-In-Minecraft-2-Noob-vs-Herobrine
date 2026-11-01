using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 120;
#if !UNITY_EDITOR
        Stats.Instance = SaveSystem.Load();
        Meta.Instance = SaveSystem.MetaLoad();

        Player.IsLoad = true;
        
        if (SaveSystem.IsSave())
        {
            if (Stats.Instance.Data[1] == 1)
            {
                SceneManager.LoadScene("Menu", LoadSceneMode.Single);
            }
            else
            {
                SwitchingLevel.SwitchingNotWorld(Stats.Instance.LevelName, Stats.Instance.Position);
            }
        }
        else
        {
            Stats.Instance = Stats.GetDefault();
            Meta.Instance = SaveSystem.MetaLoad();
            SceneManager.LoadScene("Intro", LoadSceneMode.Single);
        }
        
        return;
#endif
        
        Stats.Instance = Stats.GetDefault();
        Meta.Instance = Meta.GetDefault();
        Player.IsLoad = true;
        Stats.Instance.LevelName = "Home";

        // SceneManager.LoadScene("ScreenSaver", LoadSceneMode.Single);

        // Meta.Instance.IsCompleteDemo = true;
        // Stats.Instance.IsGenocide = true;

        // Stats.Instance = SaveSystem.Load();
        // Stats.Instance.Data[6] = 1;
        // Stats.Instance.Data[7] = 1;
        // Stats.Instance.Money = 22;

        // Stats.Instance.Data[5] = 1;
        // Stats.Instance.Data[31] = 1;
        // Stats.Instance.Data[32] = 1;
        // Stats.Instance.Data[33] = 1;
        
        // SceneManager.LoadScene("Menu", LoadSceneMode.Single);

        // Stats.Instance = Stats.GetDefault();

        // SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        // SceneManager.LoadSceneAsync("ScreenSaver",  LoadSceneMode.Additive);

        // Meta.Instance.IsCompleteTutorial = true;
        Meta.Instance.Volume = 25f;

        // Meta.Instance.IsCompleteTutorial = true;
        // Stats.Instance.Data[2] = 1;
        // Stats.Instance.Data[5] = 1;
        // Stats.Instance.Data[31] = 1;
        // Stats.Instance.Data[32] = 1;
        // Stats.Instance.Data[33] = 1;
        // Stats.Instance.Data[34] = 1;
        // Stats.Instance.Data[35] = 1;
        
        // Meta.Instance.IsEndEvil = true;
        // Meta.Instance.IsEndOldSpawn = true;
        // Meta.Instance.IsEndPacifist = true;
        
        // Stats.Instance.Position = new Vector3(0f, -1.5f, 0);
        // Stats.Instance.LevelName = "Home"; // Spawn (-1, 0)
        // SwitchingLevel.SwitchingNotWorld(Stats.Instance.LevelName, Stats.Instance.Position);
        
        SceneManager.LoadScene("Battle", LoadSceneMode.Single);

        // SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        // SceneManager.LoadScene("Intro", LoadSceneMode.Single);
        // SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        // SceneManager.LoadScene("Outro 1", LoadSceneMode.Single);

        // SceneManager.LoadScene("Shop", LoadSceneMode.Single);
        
        // Stats.Instance.TryAddItem("Steak"); // Стейк
        // Stats.Instance.TryAddItem("Steak");
        // Stats.Instance.TryAddItem("Steak");
        // Stats.Instance.TryAddItem("Steak");
        // Stats.Instance.TryAddItem("Potion"); // Зелье Л.
        // Stats.Instance.TryAddItem("Potion");
        // Stats.Instance.TryAddItem("GoldApple"); // Зол.Яблк.
    }

    private void Start()
    {
#if !UNITY_EDITOR
        return;
#endif
    }
}
