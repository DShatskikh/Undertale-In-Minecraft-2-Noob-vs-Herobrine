using System.Collections;
using UnityEngine;

public sealed class HeroFallShell : MonoBehaviour
{
    [SerializeField]
    private GameObject _dogsFly;

    [SerializeField]
    private AudioSource _sfx;
    
    private IEnumerator Start()
    {
        transform.position = new Vector3(0, 9.44f, 0);
        
        var y = -1.88;

        while (transform.position.y > y)
        {
            yield return null;
            transform.position = transform.position.AddY(-Time.deltaTime * 8);
        }
        
        CameraShaker.Instance.Shake();
        _sfx.Play();
        yield return new WaitForSeconds(0.5f);
        CameraShaker.Instance.Shake();
        _dogsFly.SetActive(true);
        Destroy(gameObject);
    }
}
