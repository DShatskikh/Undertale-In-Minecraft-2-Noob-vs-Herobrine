using System;
using UnityEngine;

public sealed class RealTimeClock : MonoBehaviour
{
    public static RealTimeClock Instance;
    
    private float _time;
    
    public float GetTime => _time;
    public string GetFormatTime => $"{_time / 3600:00}:{_time / 60 % 60:00}:{_time % 60:00}";

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        _time = Stats.Instance.Time;
    }
    
    private void Update()
    {
        _time += Time.unscaledDeltaTime;
    }
}
