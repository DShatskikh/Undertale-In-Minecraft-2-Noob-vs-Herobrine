using TMPro;
using UnityEngine;
using UnityEngine.Events;

public sealed class BlackAndWhiteWorld : MonoBehaviour
{
    [SerializeField]
    private Renderer[] _renderers;

    [SerializeField]
    private TextMeshPro[] _labels;
    
    [SerializeField]
    private UnityEvent _event;
 
    [SerializeField]
    private Material _material;
    
    private void Start()
    {
        if (Stats.Instance.Data[7] == 0)
            return;
        
        Activate();
    }

    public void Activate()
    {
        foreach (var renderer in _renderers)
        {
            renderer.material = _material;
        }

        foreach (var label in _labels)
        {
            label.text = "???";
        }
        
        _event.Invoke();
    }
}
