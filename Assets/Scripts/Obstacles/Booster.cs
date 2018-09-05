using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Booster : MonoBehaviour
{
    public bool right = true;
    public float force = 0f;
    public float width = 0f;
    public float totalWidth = 0f;
    public float friction = 1000f;
    private Vector3 direction;
    public BoxCollider ground;
    private BoxCollider boost;
    public bool set = false;
    private PhysicMaterial groundMaterial;

    void Start()
    {
        boost = GetComponent<BoxCollider>();
        setBooster();
        groundMaterial = new PhysicMaterial("ground");
        groundMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
        ground.material = groundMaterial;
    }

    public void FixedUpdate()
    {
        if (set)
        {
            setBooster();
            set = false;
        }
    }

    private void setBooster()
    {
        totalWidth = width;
        direction = right ? Vector3.right : Vector3.left;
        Vector3 size = ground.size;
        size.x = width;
        boost.size = size;
        ground.size = size;

        Vector3 center = ground.center;
        center.x = width / 2f;
        ground.center = center;
        boost.center = center;

        ground.material.staticFriction = friction;
        ground.material.dynamicFriction = friction;

    }

    void OnTriggerStay(Collider other)
    {
        other.GetComponent<Rigidbody>().AddForce(direction * force, ForceMode.Force);
    }


}
