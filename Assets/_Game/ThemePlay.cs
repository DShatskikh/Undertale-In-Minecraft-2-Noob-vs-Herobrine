using UnityEngine;

public sealed class ThemePlay : MonoBehaviour
{
    [SerializeField]
    private AudioClip _clip;

    [SerializeField]
    private bool _isIgnoreGenocide;
    
    private void Start()
    {
        if (!_isIgnoreGenocide)
        {
            if (Stats.Instance.Data[2] == 1)
            {
                MusicManager.Instance.Stop();
                return;
            }
        }
        
        if (Stats.Instance.Data[6] == 1 && Stats.Instance.Data[7] == 0)
        {
            MusicManager.Instance.Stop();
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
