using System.Collections.Generic;
using UnityEngine;

public class PausePos : MonoBehaviour
{
    private BodyParts bodyParts;

    private readonly Dictionary<GameObject, Vector3> defaultPos = new Dictionary<GameObject, Vector3>();
    private readonly Dictionary<GameObject, Quaternion> defaultRot = new Dictionary<GameObject, Quaternion>();
    private readonly Dictionary<Rigidbody, Vector3> defaultVel = new Dictionary<Rigidbody, Vector3>();

    private List<GameObject> parts;

    public bool pause = false;
    public bool paused;
    private List<Rigidbody> rigids;

    private void Start()
    {
        bodyParts = GetComponent<BodyParts>();
        parts = bodyParts.getParts();
        rigids = bodyParts.getRigids();
    }

    /*
    void Update()
    {
        if (pause && !paused)
        {
            Pause();
        }
        else if (!pause && paused)
        {
            Continue();
        }

    }
    */
    public void Pause()
    {
        pausePosRot();
        pauseRigid();
        paused = true;
    }

    public void Continue()
    {
        continuePosRot();
        continueRigid();
        paused = false;
    }

    private void pausePosRot()
    {
        foreach (var gameObject in parts)
        {
            var gameObjectTransform = gameObject.transform;
            defaultPos[gameObject] = gameObjectTransform.position;
            defaultRot[gameObject] = gameObjectTransform.rotation;
        }
    }

    private void pauseRigid()
    {
        foreach (var rigidBody in rigids)
        {
            defaultVel[rigidBody] = rigidBody.velocity;
            rigidBody.isKinematic = true;
        }
    }

    private void continuePosRot()
    {
        foreach (KeyValuePair<GameObject, Vector3> objPos in defaultPos) objPos.Key.transform.position = objPos.Value;
        foreach (KeyValuePair<GameObject, Quaternion> objRot in defaultRot)
            objRot.Key.transform.rotation = objRot.Value;
    }

    private void continueRigid()
    {
        foreach (KeyValuePair<Rigidbody, Vector3> objVel in defaultVel)
        {
            objVel.Key.WakeUp();
            objVel.Key.isKinematic = false;
            objVel.Key.velocity = objVel.Value;
        }
    }
}