using UnityEngine;

public sealed class EnergyBeam : MonoBehaviour
{
    [SerializeField]
    private GameObject _parent;
    
    private void Destroy()
    {
        Destroy(_parent);
    }
}
