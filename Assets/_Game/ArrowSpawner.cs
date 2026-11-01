using UnityEngine;

public sealed class ArrowSpawner : MonoBehaviour
{
    [SerializeField]
    private Arrow _arrowPrefab;

    private float _timer;
    
    private void Update()
    {
        if (_timer <= 0f)
        {
            _timer = 1.5f;
            var arow = Instantiate(_arrowPrefab, transform.position, transform.rotation, transform);
            arow.transform.localPosition = Vector3.zero;
            arow.transform.eulerAngles = arow.transform.eulerAngles.AddZ(-90);
        }
        else
        {
            _timer -= Time.deltaTime;
        }
    }
}
