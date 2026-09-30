using TMPro;
using UnityEngine;

public sealed class SlotsLabel : MonoBehaviour
{
    private TextMeshPro _label;
    
    private void Start()
    {
        _label = GetComponent<TextMeshPro>();
    }
    
    private void Update()
    {
        _label.text = $"{Stats.Instance.GetItemsCount()}/{Stats.Instance.Items.Length}$";
    }
}
