using System;
using System.Collections;
using UnityEngine;

public sealed class BonesShell : MonoBehaviour
{
    [SerializeField]
    private GameObject _frame, _bones;

    private IEnumerator Start()
    {
        _bones.SetActive(false);
        _frame.SetActive(true);
        yield return new WaitForSeconds(1);
        _frame.SetActive(false);

        _bones.SetActive(true);
        _bones.transform.localPosition = _bones.transform.localPosition.SetY(-1.05f);
        
        while (_bones.transform.localPosition.y < 0)
        {
            _bones.transform.localPosition = _bones.transform.localPosition.AddY(Time.deltaTime);
            yield return null;
        }
    }
}
