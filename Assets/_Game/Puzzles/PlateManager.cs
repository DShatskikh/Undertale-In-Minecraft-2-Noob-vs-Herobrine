using UnityEngine;
using UnityEngine.Events;

public sealed class PlateManager : MonoBehaviour
{
    [SerializeField]
    private int _saveIndex;
    
    [SerializeField]
    private Plate _resetPlate;

    [SerializeField]
    private Plate[] _plates;

    [SerializeField]
    private string _answer;

    [SerializeField]
    private AudioSource _sfx;
    
    private string _currentCode;
    private bool _isActivated;
    
    public UnityEvent Event;
    
    private void Start()
    {
        if (Stats.Instance.Data[_saveIndex] == 1)
        {
            _isActivated = true;
            return;
        }
        
        _resetPlate.OnActivate += OnResetActivate;
        _resetPlate.OnExit += () => _resetPlate.Reset();

        foreach (var plate in _plates)
        {
            plate.OnActivate += OnPlaceActivate;
        }
    }

    private void OnPlaceActivate(char symbol)
    {
        _currentCode += symbol;
    }
    
    private void OnResetActivate(char symbol)
    {
        if (_currentCode == _answer)
        {
            _isActivated = true;
                
            foreach (var plate in _plates)
            {
                plate.Reset();
                plate.OnActivate -= OnPlaceActivate;
                plate.OnExit += () =>
                {
                    plate.Reset();
                };
            }
                
            _resetPlate.OnActivate -= OnResetActivate;
            _sfx.Play();
            Event?.Invoke();
        }
        else
        {
            foreach (var plate in _plates)
            {
                plate.Reset();
            } 
        }
    }
}
