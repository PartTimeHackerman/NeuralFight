using System;
using System.Security.Cryptography;
using UnityEngine;

public class SizeChanger : MonoBehaviour
{
    public float size;

    public float Size
    {
        get { return size; }
        set
        {
            if (Math.Abs(size * value - size) > .001f)
            {
                ChangeSize(value);   
            }
        }
    }

    public BoxCollider Collider;
    public Transform Child;
    public float PrecSize = 1f;
    private float baseSize;
    private float minSize;
    private float maxSize;

    private void Start()
    {
        Collider = GetComponent<BoxCollider>();
        //Child = GetComponentsInChildren<Transform>()[1];
        size = Collider.size.y;
        baseSize = size;
        minSize = size * .6f;
        maxSize = size * 1.5f;
    }
    private void FixedUpdate()
    {
        PrecSize = Mathf.Clamp(PrecSize, .6f, 1.5f);
        Size = PrecSize;
    }

    private void ChangeSize(float precSize)
    {
        precSize = Mathf.Clamp(precSize, .6f, 1.5f);
        size = baseSize * precSize; 
        Vector3 newSize = Collider.size;
        newSize.y = size;
        Collider.size = newSize;
        Vector3 newCenter = Collider.center;
        newCenter.y = -size / 2f;
        Collider.center = newCenter;
        Vector3 childLocPos = Child.localPosition;
        childLocPos.y = -size;
        Child.localPosition = childLocPos;

        ConfigurableJoint cj = Child.GetComponent<ConfigurableJoint>();
        if (cj != null)
        {
            cj.connectedAnchor = childLocPos;
        }
    }
}