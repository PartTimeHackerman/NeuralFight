using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

class Booster : Obstacle
{
    public bool right = true;
    public float force = 0f;
    public float width = 0f;
    //public float totalWidth = 0f;
    public float friction = 1000f;
    private Vector3 direction;
    public BoxCollider ground;
    private BoxCollider boost;
    public bool set = false;
    private PhysicMaterial groundMaterial;

    void OnEnable()
    {
        boost = GetComponent<BoxCollider>();
        setRandom();
        groundMaterial = new PhysicMaterial("ground");
        groundMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
        ground.material = groundMaterial;
    }

    public void FixedUpdate()
    {
        if (set)
        {
            setRandom();
            set = false;
        }
    }

    public override void setRandom()
    {
        force = Random.Range(0f, 100f);
        width = Random.Range(2f, 15f);
        friction = Random.Range(0f, 1000f);
        setBooster();
    }

    private void setBooster()
    {
        totalWidth = width;
        direction = right ? Vector3.right : Vector3.left;
        Vector3 size = ground.size;
        size.x = width;
        Vector3 boostSize = boost.size;
        boostSize.x = width;
        boost.size = boostSize;
        ground.size = size;
        ground.GetComponent<SpriteRenderer>().size = size;
        Vector3 boosterSpriteSize = GetComponent<SpriteRenderer>().size;
        boosterSpriteSize.x = width;
        GetComponent<SpriteRenderer>().size = boosterSpriteSize;
        Vector3 center = ground.center;
        center.x = width / 2f;
        Vector3 boostCenter = boost.center;
        boostCenter.x = width / 2f;
        ground.center = center;
        boost.center = boostCenter;
        Color groundColor = ground.GetComponent<SpriteRenderer>().color;
        groundColor.r = friction / 1000f;
        ground.GetComponent<SpriteRenderer>().color = groundColor;

        Color boosterColor = GetComponent<SpriteRenderer>().color;
        boosterColor.a = force / 100f;
        GetComponent<SpriteRenderer>().color = boosterColor;

        ground.material.staticFriction = friction;
        ground.material.dynamicFriction = friction;

    }

    void OnTriggerStay(Collider other)
    {
        other.GetComponent<Rigidbody>().AddForce(direction * force, ForceMode.Force);
    }


}
