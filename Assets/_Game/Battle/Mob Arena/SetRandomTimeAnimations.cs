using System;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class SetRandomTimeAnimations : MonoBehaviour
{
    private void Start()
    {
        var animator = GetComponent<Animator>();
        var state = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        animator.Play(state.fullPathHash, 0, Random.Range(0f, 1f));
    }
}
