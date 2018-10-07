using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

class SwipeInput : InputController
{

    public Vector2 down;
    public Vector2 up;
    public Vector2 dir;
    public float distance;
    public ParticleSystem swipeParticles;
    public ModelInput ModelInput;
    public float force = 100f;

    public bool firstTouch = true;

    void Start()
    {
    }

    public override void Moved(Touch touch)
    {
        Vector3 pos = touch.position;
        pos = Camera.main.ScreenToWorldPoint(pos);
        pos.z = swipeParticles.transform.position.z;
        swipeParticles.transform.position = pos;
        
    }

    public override void Began(Touch touch)
    {
        Vector3 pos = touch.position;
        down = pos;

        Vector3 parPos = Camera.main.ScreenToWorldPoint(pos);
        parPos.z = swipeParticles.transform.position.z;
        swipeParticles.transform.position = parPos;
        swipeParticles.Play();
    }

    public override void Ended(Touch touch)
    {
        Vector3 pos = touch.position;
        pos.z = 1f;
        up = pos;
        Vector2 screen = new Vector2(Screen.width, Screen.width);
        distance = Vector2.Distance(down / screen, up / screen);

        swipeParticles.Stop();
        dir = up - down;
        dir = dir.normalized;
        ModelInput.addVel(dir, distance * force, ForceMode.Impulse);
    }
}
