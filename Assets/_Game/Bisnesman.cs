using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Bisnesman : BannedUsable
{
    protected override int _id => 12;
    protected override string _message => "/b0Хочешь что-нибудь купить?";
    
    [SerializeField]
    private SpriteRenderer _view;

    private float _timer;
    
    protected override void Start()
    {
        _gameObject = _view.gameObject;
        
        if (Stats.Instance.Data[_id] == 1)
        {
            gameObject.SetActive(false);
            enabled = false;
        }
    }

    private void Update()
    {
        var speed = 0.2f;
        
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            return;
        }
        
        if (Vector2.Distance(transform.position, Player.Instance.transform.position) < 5f)
        {
            _view.flipX = transform.position.x > Player.Instance.transform.position.x;
            _timer = speed;
        }
        else
        {
            _view.flipX = false;
            _timer = speed;
        }
    }

    public override IEnumerator AwaitUse()
    {
        if (Stats.Instance.Data[6] == 1)
        {
            yield return AwaitBan();
        }
        else if (Stats.Instance.Data[12] == 0)
        {
            SceneManager.LoadScene("Shop", LoadSceneMode.Single);
        }
    }
}
