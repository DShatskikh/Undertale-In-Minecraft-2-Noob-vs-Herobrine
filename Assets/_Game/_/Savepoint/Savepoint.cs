using System.Collections;
using UnityEngine;

public class Savepoint : Usable
{
    [SerializeField]
    private int _locationIndex;
    
    public override IEnumerator AwaitUse()
    {
        Player.Instance.enabled = false;

        var saveWindow = Instantiate(Resources.Load<SaveWindow>("Save Window"),
            new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + 0f),
            Camera.main.transform.rotation);

        saveWindow.Init(_locationIndex);
        yield break;
    }
}
