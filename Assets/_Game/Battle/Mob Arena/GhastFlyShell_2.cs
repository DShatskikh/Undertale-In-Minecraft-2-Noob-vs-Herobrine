using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class GhastFlyShell_2 : MonoBehaviour
{
    private const float CREATE_SHELL_DURATION = 0.5f;
    private const float SPEED = 3;

    [SerializeField]
    private Transform _ghast;

    [SerializeField]
    private GameObject _moneyPrefab, _golemPrefab;

    private float _timerCreateShell;

    private IEnumerator Start()
    {
        while (true)
        {
            while (_ghast.localPosition.x < 3f)
            {
                yield return null;
                _ghast.transform.position = _ghast.transform.position.AddX(Time.deltaTime * SPEED);
            }

            _ghast.GetComponent<SpriteRenderer>().flipX = true;
        
            while (_ghast.localPosition.x > -3f)
            {
                yield return null;
                _ghast.transform.position = _ghast.transform.position.AddX(-Time.deltaTime * SPEED);
            }
        
            _ghast.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    private void Update()
    {
        if (_ghast.localPosition.x > 2.5f || _ghast.localPosition.x < -2.5f)
            return;
        
        if (_timerCreateShell > 0)
        {
            _timerCreateShell -= Time.deltaTime;
        }
        else
        {
            var randomCount = Random.Range(0, 7);
            var prefab = randomCount == 5 ? _moneyPrefab : _golemPrefab;
            var shell = Instantiate(prefab, _ghast.position, Quaternion.identity, transform);
            shell.SetActive(true);
            _timerCreateShell = CREATE_SHELL_DURATION;
        }
    }
}
