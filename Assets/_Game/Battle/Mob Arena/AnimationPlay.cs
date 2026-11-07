using System;
using UnityEngine;

public sealed class AnimationPlay : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private string _name;
    
    private void Start()
    {
        _animator.Play(_name);
    }
}
