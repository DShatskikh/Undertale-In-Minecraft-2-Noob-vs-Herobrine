using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Bisnesman : BannedUsable
{
    protected override int _id => 12;
    protected override string _message => "/b0Хочешь что-нибудь купить?";
    
    [SerializeField]
    private SpriteRenderer _view;
    
    protected override void Start()
    {
        _gameObject = _view.gameObject;
        
        if (Stats.Instance.Data[_id]  == 1)
        {
            gameObject.SetActive(false);
            enabled = false;
        }
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, Player.Instance.transform.position) < 5f)
        {
            _view.flipX = transform.position.x > Player.Instance.transform.position.x;
        }
        else
        {
            _view.flipX = false;
        }
    }

    public override IEnumerator AwaitUse()
    {
        if (Stats.Instance.Data[6] == 1)
        {
            yield return AwaitBan();
        }
        else
        {
            SceneManager.LoadScene("Shop", LoadSceneMode.Single);
        }
    }
}
