using TMPro;
using UnityEngine;

public sealed class Grox : Enemy
{
    [SerializeField]
    private AudioClip _theme;
    
    [SerializeField]
    private AttacksPool[] _attacks;
    
    [SerializeField]
    private Animator _view;

    [SerializeField]
    private TextMeshPro _nickname;
}
