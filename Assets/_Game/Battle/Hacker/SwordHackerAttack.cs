using System.Collections;
using UnityEngine;

public sealed class SwordHackerAttack : MonoBehaviour
{
    [SerializeField]
    private SwordHackerShell _swordPrefab;
    
    [SerializeField]
    private Transform[] _points;

    public IEnumerator Start()
    {
        Heart.Instance.transform.position = new Vector3(0, -3.4f);
        SwordHackerShell.CanActive = true;
        
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            var index = Random.Range(0, _points.Length);
            var point = _points[index];
            var sword = Instantiate(_swordPrefab, point.position, point.rotation, transform);
            sword.Init(index < 3);
        }
    }
}
