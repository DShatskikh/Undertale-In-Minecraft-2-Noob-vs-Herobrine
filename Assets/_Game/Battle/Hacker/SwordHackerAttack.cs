using System.Collections;
using UnityEngine;

public sealed class SwordHackerAttack : MonoBehaviour
{
    [SerializeField]
    private SwordHackerShell _swordPrefab;
    
    [SerializeField]
    private Transform[] _points;

    public IEnumerator AwaitProcess()
    {
        Heart.Instance.transform.position = new Vector3(0, -3.4f);
        SwordHackerShell.CanActive = true;
        
        var timer = 10f; // 10
        
        while (timer > 0)
        {
            timer -= 0.5f;

            yield return new WaitForSeconds(0.5f);

            var index = Random.Range(0, _points.Length);
            var point = _points[index];
            var sword = Instantiate(_swordPrefab, point.position, point.rotation, transform);
            sword.Init(index < 3);
        }
        
        SwordHackerShell.CanActive = false;
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
