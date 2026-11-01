using UnityEngine;

public sealed class DiscoRotate : MonoBehaviour
{
    private void Update()
    {
        transform.eulerAngles = transform.eulerAngles.AddZ(Time.deltaTime * 20);
    }
}
