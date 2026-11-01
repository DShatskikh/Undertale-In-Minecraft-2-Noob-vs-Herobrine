using UnityEngine;

public sealed class AxeShell : MonoBehaviour
{
    [SerializeField]
    private GameObject _energyBeam;
    
    private void Update()
    {
        if (transform.position.y < -6.69f)
        {
            gameObject.SetActive(false);
            _energyBeam.SetActive(true); 
        }
    }
}
