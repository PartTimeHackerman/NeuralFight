using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class CannonBall : Poolable
{
    public Rigidbody rigidbody;
    public Rigidbody target;

    void OnEnable()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        StartCoroutine(update(1f));
    }

    public void setSize(float size)
    {
        GetComponent<SphereCollider>().radius = size;
        size *= 2f;
        GetComponent<SpriteRenderer>().size = new Vector2(size, size);
        var trails = GetComponent<ParticleSystem>().trails;
        trails.widthOverTrailMultiplier = size;
        setOutline();
    }

    private void setOutline()
    {
        GetComponent<SpriteShadows>().resetShadows();
    }

    IEnumerator update(float wait)
    {
        while (true)
        {
            if (enabled && Vector3.Distance(transform.position, target.position) > 100f)
            {
                this.push();
            }

            yield return new WaitForSeconds(wait);
        }
    }

}

