using System;
using UnityEngine;

public sealed class ShellSpawner : MonoBehaviour
{
    [Serializable]
    public struct SpawnTimer
    {
        public GameObject Shell;
        public float Timer;
    }
    
    [SerializeField]
    private SpawnTimer[] _spawnTimers;

    [SerializeField]
    private Vector2 _size = new(2f, 1.75f); // 3f, 1f

    [SerializeField]
    private Vector3 _heartPosition = new(0, -3.39f);
    
    [SerializeField]
    private Vector2 _gravityDirection = new(0, -1);

    [SerializeField]
    private bool _isNoChangeSize;
    
    private float _timer;

    public float Timer = 5;
    public Vector2 GetSize => _size;
    public bool GetIsNoChangeSize => _isNoChangeSize;
    public Vector3 GetStartPosition => _heartPosition;

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

    private void Start()
    {
        BattleManager.Instance.SetFrame(_size); // 2, 2
        Heart.Instance.transform.position = _heartPosition;
        Heart.Instance.GravityDirection = _gravityDirection;
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
