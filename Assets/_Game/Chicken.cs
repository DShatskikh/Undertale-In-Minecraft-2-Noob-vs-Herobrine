using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class Chicken : Usable
{
    [SerializeField]
    private Transform[] _points;
    
    private bool _isNotStroked;
    private bool _isStroked;
    private int _index;
    private bool _isMoved;
    private float _speed = 2;
    private float _stay = 5f;

    private void Start()
    {
        if (Stats.Instance.Data[6] == 1)
        {
            gameObject.SetActive(false);
        }

        _isMoved = true;
        GetComponent<Animator>().Play("Stay");
    }

    private void Update()
    {
        if (_isMoved)
        {
            if (_stay > 0)
            {
                GetComponent<Animator>().Play("Stay");
                _stay -= Time.deltaTime;
                return;
            }
            
            if (transform.position == _points[_index].position)
            {
                GetComponent<Animator>().Play("Stay");
                _stay = 3;
                _index = Random.Range(0, _points.Length);
            }
            else
            {
                transform.localScale = new Vector3(transform.position.x < _points[_index].position.x ? 1 : -1, 1, 1);
                
                transform.position = Vector3.MoveTowards(transform.position, _points[_index].position, 
                    _speed * Time.deltaTime);
                GetComponent<Animator>().Play("ChickenStep");
            }
            
            //if (Vector2.Distance(transform.position, _points[_index].position) > 0.1f)
            //    GetComponent<SpriteRenderer>().flipX = _points[_index].position.x < transform.position.x;
        }
    }

    public override IEnumerator AwaitUse()
    {
        Player.Instance.enabled = false;
        _isMoved = false;
        _stay = 1;
        GetComponent<Animator>().Play("Stay");
        
        if (_isStroked)
        {
            yield return DialogueWindow.StartDialogue(new[] { "/n0Наверное хватит на сегодня." }, false);
        }
        else
        {
            _stay = 5;
            
            if (!_isNotStroked)
                yield return DialogueWindow.StartDialogue(new[] { "/n0О курочка!" }, false);
        
            transform.localScale = new Vector3(Player.Instance.transform.position.x > transform.position.x ? 1 : -1, 1, 1);
            
            yield return SelectWindow.StartSelect("Погладить?", "Погладить", "Не гладить");
            
            _isNotStroked = true;
        
            if (!SelectWindow.IsRight)
            {
                Player.Instance.gameObject.SetActive(false);
                transform.localScale = new Vector3(Player.Instance.transform.position.x < transform.position.x ? 1 : -1, 1, 1);
                GetComponent<Animator>().Play("StartChickenStroked");
                yield return  new WaitForSeconds(3f);
                GetComponent<Animator>().Play("EndChickenStroked");
                yield return new WaitForSeconds(1.2f);
                GetComponent<Animator>().Play("Stay");
                yield return null;
                transform.localScale = new Vector3(Player.Instance.transform.position.x > transform.position.x ? 1 : -1, 1, 1);
                Player.Instance.gameObject.SetActive(true);
                _isStroked = true;
            }
            else
            {
            
            }
        }
        
        _isMoved = true;
        Player.Instance.enabled = true;
    }
}
