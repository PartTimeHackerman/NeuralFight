using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Step : MonoBehaviour
{
    public SphereCollider front;
    public SphereCollider back;

    public float frontRadius = 0f;
    public float backRadius = 0f;

    public float width = 0f;
    public bool set = false;

    public void Start()
    {
    }

    public void FixedUpdate()
    {
        if (set)
        {
            setStep();
            set = false;
        }
    }

    public void setStep()
    {
        setUpStep(front, frontRadius, true);
        setUpStep(back, backRadius, false);

        Vector3 backPos = back.transform.localPosition;
        backPos.x = width + frontRadius + backRadius;
        back.transform.localPosition = backPos;
    }

    public void setUpStep(SphereCollider step, float radius, bool front)
    {
        Vector3 frontPos = step.transform.position;
        frontPos.y = -radius;
        step.transform.position = frontPos;

        step.radius = radius;

        setHorVer(step.transform.Find("horizontal").GetComponent<BoxCollider>(), radius, true, front);
        setHorVer(step.transform.Find("vertical").GetComponent<BoxCollider>(), radius, false, front);
    }

    public void setHorVer(BoxCollider boxCollider, float radius, bool hor, bool frnt)
    {
        Vector3 boxColliderSize = boxCollider.size;
        boxColliderSize.x = radius;
        boxColliderSize.y = radius;
        boxCollider.size = boxColliderSize;

        Vector3 boxColliderCenter = boxCollider.center;
        boxColliderCenter.x = -radius / 2f;
        boxColliderCenter.y = -radius / 2f;
        boxCollider.center = boxColliderCenter;
        

        Vector3 pos = boxCollider.transform.localPosition;
        pos.y = hor ? radius : 0;
        pos.x = !hor ? ( frnt ? radius : 0) : (frnt ? 0 : radius);
        boxCollider.transform.localPosition = pos;
    }

}
