using UnityEngine;

public sealed class TimerSpawner : MonoBehaviour
{
    [SerializeField]
    private ShellSpawner.SpawnTimer[] _spawnTimers;
    
    private float _timer;
    
    private void OnEnable()
    {
        _timer = 0;
        
        foreach (var spawnTimer in _spawnTimers)
        {
            if (spawnTimer.Shell == null)
                continue;
            
            spawnTimer.Shell.SetActive(false);
        }
    }
    
    private void Update()
    {
        _timer += Time.deltaTime;

        foreach (var spawnTimer in _spawnTimers)
        {
            if (spawnTimer.Shell == null)
                continue;
            
            if (spawnTimer.Timer < _timer)
            {
                spawnTimer.Shell.SetActive(true);
            }
        }
    }
}
