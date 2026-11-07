using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class GhastFlyShell : MonoBehaviour
{
    private const float CREATE_SHELL_DURATION = 0.15f;
    private const float SPEED = 7;

    [SerializeField]
    private bool _isRight;
    
    [SerializeField]
    private Transform _ghast;

    [SerializeField]
    private GameObject _moneyPrefab, _fireballPrefab;

    private float _timerCreateShell;
    private bool _isSpawnedMoney;
    private bool _isSpawnedEmpty;
    private int _spawnedCounter;
    private List<GameObject> _createdShells = new();

    private void Start()
    {
        if (_isRight)
        {
            _ghast.transform.localPosition = new Vector3(10f, 0.87f,0);
            _ghast.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            _ghast.transform.localPosition = new Vector3(-10f, 0.87f,0);
        }
    }

    private void Update()
    {
        if (_isRight)
        {
            _ghast.transform.localPosition = _ghast.transform.localPosition.AddX(-Time.deltaTime * SPEED);
        }
        else
        {
            _ghast.transform.localPosition = _ghast.transform.localPosition.AddX(Time.deltaTime * SPEED);
        }

        if (_ghast.transform.localPosition.x < -3.4f || _ghast.transform.localPosition.x > 3.4f)
        {
            if (_createdShells.Count > 0)
            {
                foreach (var shell in _createdShells)
                {
                    shell.GetComponent<MoveShell>().SetSpeed(5);
                }

                _createdShells = new List<GameObject>();
            }
            
            return;
        }
            
        if (_timerCreateShell > 0)
        {
            _timerCreateShell -= Time.deltaTime;
        }
        else
        {
            var randomCount = Random.Range(0, 7);

            if (_isSpawnedMoney)
            {
                if (randomCount == 5)
                    randomCount = 0;
            }
            else if (_spawnedCounter > 7)
            {
                randomCount = 5;
            }

            if (randomCount == 5)
            {
                _isSpawnedMoney = true;
            }
            
            var prefab = randomCount == 5 ? _moneyPrefab : _fireballPrefab;

            if (randomCount != 4 || _isSpawnedEmpty)
            {
                var shell = Instantiate(prefab, _ghast.position, Quaternion.identity, transform);
                shell.GetComponent<MoveShell>().SetSpeed(0);
                _createdShells.Add(shell);
                shell.SetActive(true);
            }
            else
            {
                _isSpawnedEmpty = true;
            }
            
            _spawnedCounter++;
            _timerCreateShell = CREATE_SHELL_DURATION;
        }
    }
}
