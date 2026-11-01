using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class AttacksPool : MonoBehaviour
{
    [SerializeField]
    private bool _isStartRandom = true;
    
    [SerializeField]
    private bool _isEnd;
    
    [SerializeField]
    private ShellSpawner[] _attacks;

    private int _index;
    public Vector2 GetSize
    {
        get
        {
            if (_isStartRandom)
                _index = Random.Range(0, _attacks.Length);
            
            return _attacks[_index].GetSize;
        }
    }

    private void OnEnable()
    {
        foreach (var attack in _attacks)
        {
            attack.gameObject.SetActive(false);
        }
    }

    public IEnumerator AwaitProcess()
    {
        gameObject.SetActive(true);
        Heart.Instance.gameObject.SetActive(true);
        var i = _index;
        var isFirst = true;
        
        while (BattleManager.Instance.Progress < 100)
        {
            if (i >= _attacks.Length)
            {
                i = 0;
            }

            if (!isFirst)
                yield return AwaitTransition(_attacks[i]);
            
            var attack = Instantiate(_attacks[i], transform);
            attack.gameObject.SetActive(true);
            var timer = attack.Timer;

            while (timer > 0 && BattleManager.Instance.Progress < 100)
            {
                timer -= Time.deltaTime;
                yield return null;
            }
            
            Destroy(attack.gameObject);
            isFirst = false;
            i++;

            if (_isEnd && i == _attacks.Length)
            {
                BattleManager.Instance.Progress = 100;
            }
        }
        
        gameObject.SetActive(false);
    }

    private IEnumerator AwaitTransition(ShellSpawner shellSpawner)
    {
        Heart.Instance.enabled = false;
        
        var size = shellSpawner.GetSize;
        const float SPEED = 6f;

        var frame = BattleManager.Instance.GetFrame;
        var heartSpeed = Vector2.Distance(Heart.Instance.transform.position, shellSpawner.GetStartPosition) > 3 ? 9 : 3;
        
        if (!shellSpawner.GetIsNoChangeSize)
        {
            while (frame.GetComponent<SpriteRenderer>().size != size || Heart.Instance.transform.position != shellSpawner.GetStartPosition)
            {
                Heart.Instance.transform.position = Vector2.MoveTowards(Heart.Instance.transform.position,
                    shellSpawner.GetStartPosition, heartSpeed * Time.deltaTime);
            
                frame.SetFrame(Vector2.MoveTowards(frame.GetComponent<SpriteRenderer>().size, 
                    size, Time.deltaTime * SPEED));
                yield return null;
            }
        }
        else
        {
            Heart.Instance.transform.position = shellSpawner.GetStartPosition;
            frame.SetFrame(size);
        }
        
        Heart.Instance.enabled = true;
    }
}
