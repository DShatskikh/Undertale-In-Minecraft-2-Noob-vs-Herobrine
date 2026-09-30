using UnityEngine;

public sealed class GroundChecker : MonoBehaviour
{
    public bool IsGrounded;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        IsGrounded = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        IsGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IsGrounded = false;
    }
}
