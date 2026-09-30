using UnityEngine;

public sealed class ThemePlay : MonoBehaviour
{
    [SerializeField]
    private AudioClip _clip;
    
    private void Start()
    {
        if (Stats.Instance.Data[6] == 1 && Stats.Instance.Data[7] == 0)
        {
            return;
        }

        if (Stats.Instance.Data[7] == 1)
        {
            MusicManager.Instance.Play(Resources.Load<AudioClip>("DELTARUNE OST — man"));
            return;
        }
        
        MusicManager.Instance.Play(_clip);
    }
}
