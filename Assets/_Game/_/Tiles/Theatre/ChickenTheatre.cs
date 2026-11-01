using UnityEngine;

public sealed class ChickenTheatre : MonoBehaviour
{
    [SerializeField]
    private AudioSource[] _steps;

    private bool _isRight;
    
    public void OnStep()
    {
        if (_isRight)
        {
            _steps[1].Play();
        }
        else
        {
            _steps[0].Play();
        }
    }
}
