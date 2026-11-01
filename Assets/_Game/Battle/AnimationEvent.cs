using UnityEngine;
using UnityEngine.Events;

public sealed class AnimationEvent : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _event;
    
    private void Event()
    {
        _event?.Invoke();
    }
}
