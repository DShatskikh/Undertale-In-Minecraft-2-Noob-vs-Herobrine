using UnityEngine;

public sealed class RotateShell : MonoBehaviour
{
    [SerializeField]
    private float _speed = 100;
    
    private void Update()
    {
        transform.eulerAngles = transform.eulerAngles.AddZ(Time.deltaTime * _speed);
    }
}
