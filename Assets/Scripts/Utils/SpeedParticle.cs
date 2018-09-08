using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class SpeedParticle : MonoBehaviour
{
    public Rigidbody reference;
    private ParticleSystem particleSystem;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        ParticleSystem.ShapeModule shape = particleSystem.shape;
        float vel = reference.velocity.magnitude;
        var emmision = particleSystem.emission;
        emmision.rateOverDistance = vel < 20 ? 0 : vel / 50f;
        //wshape.rotation = reference.transform.rotation.eulerAngles;
    }
}
