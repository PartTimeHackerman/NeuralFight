using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSettings : MonoBehaviour
{

    private Animator animator;
    public bool pause;
    public bool setSpeed;
    public float speed = 1f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        animator.enabled = !pause;

        animator.speed = speed;
    }
}
