using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEffector : MonoBehaviour
{
    public BodyParts bodyParts;
    public float waitMin = .1f, waitMax = 5f;
    public Vector2 velsMult = new Vector2(1f, .5f);
    public Vector2 force = new Vector2(-100f, 100f);
    private List<Rigidbody> rigids;
    public ForceMode forceMode = ForceMode.VelocityChange;

    public bool debug = false;

    private void Start()
    {
        rigids = bodyParts.getRigids();
        StartCoroutine(addVelocity());
    }

    IEnumerator addVelocity()
    {
        while (true)
        {
            Rigidbody rigid = rigids[Random.Range(0, rigids.Count)];
            Vector2 randForce = new Vector2(Random.Range(force.x, force.y), Random.Range(force.x, force.y));
            Vector2 randVel = velsMult * randForce;
            rigid.AddForce(randVel, forceMode);
            if (debug)
                Debug.DrawRay(rigid.transform.position, randVel * .1f);
            yield return new WaitForSeconds(Random.Range(waitMin, waitMax));
        }
    }
}