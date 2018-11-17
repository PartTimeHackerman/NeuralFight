using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class HitNumber : Poolable
{
    public bool play;
    public int damage;
    
    public Animator Animator;
    public Text Text;
    public bool Player1 = true;

    private bool Animating = false;
    private float FirstDamage = 0f;
    private float CurrentDamage = 0f;
    private List<AnimatorResetBoolAtEnd> AnimatorResetBoolAtEnd;
    private Quaternion zeroRot = Quaternion.Euler(Vector3.zero);
    private Transform parent;
    public float followSpeed = 1f;
    private bool newDamage = false; 
    private void Start()
    {
        AnimatorResetBoolAtEnd = Animator.GetBehaviours<AnimatorResetBoolAtEnd>().ToList();
        foreach (var animatorResetBoolAtEnd in AnimatorResetBoolAtEnd)
        {
            
            animatorResetBoolAtEnd.OnAnimationEnd += Disable;
        }
        //gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (newDamage)
        {
            Text.color = Color.white;
            newDamage = false;
        }
    }

    private void Update()
    {
        transform.rotation = zeroRot;
        if (parent != null && Animating)
        {
            Vector3 parentPos = parent.position;
            parentPos.y += .2f;
            transform.position = Vector3.Lerp(transform.position, parentPos, Time.deltaTime * followSpeed);
        }
    }

    private void FixedUpdate()
    {
        if (play)
        {
            Text.text = damage.ToString();
            Animator.SetBool("MoveUp", true);
            play = false;
        }

    }

    public void setParent(Transform parent)
    {
        this.parent = parent;
    }

    public void Animate(float damage, Vector3 pos, bool P1)
    {
        Text.text = ThousandsConverter.ToKs(damage);
        transform.position = pos;

        if (P1)
            Animator.SetBool("MoveUpP1", true);
        else
            Animator.SetBool("MoveUpP2", true);

    }

    public void Anim(float damage)
    {
        if (!Animating)
        {
            Vector3 parentPos = parent.position;
            parentPos.y += .2f;
            transform.position = parentPos;
            FirstDamage = damage;
            if (Player1)
                Animator.SetBool("MoveUpP1", true);
            else
                Animator.SetBool("MoveUpP2", true);
            Animating = true;
        }

        newDamage = true;
        CurrentDamage += damage;
        float scale = Mathf.InverseLerp(FirstDamage, FirstDamage * 100f, CurrentDamage) + 1f;
        Text.transform.localScale = new Vector3(scale, scale, scale);
        Text.text = ThousandsConverter.ToKs(CurrentDamage);


    }
    

    private void Disable()
    {
        transform.position = new Vector3(-100f, -100f, 0f);
        //this.push();
        Animating = false;
        FirstDamage = 0f;
        CurrentDamage = 0f;

        //Debug.Log("Disable" + transform.position+ gameObject.activeSelf);
    }
}