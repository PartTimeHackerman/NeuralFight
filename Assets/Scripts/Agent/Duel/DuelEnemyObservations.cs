using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class DuelEnemyObservations : MonoBehaviour
{
    public bool debug = false;
    public int observationsSpace;
    public bool playerOne = true;
    public BodyParts enemyBodyParts;
    private PhysicsUtils physicsUtils;
    private List<float> enemyObservations = new List<float>();
    private Rigidbody enemyRoot;
    private List<Rigidbody> enemyRigids;

    public Vector3 maxAxis = new Vector3();
    public Vector3 minAxis = new Vector3();
    public Vector3 COM = new Vector3();
    public Vector3 COMVel = new Vector3();
    public Vector3 COMRotVel = new Vector3();
    public Vector3 rootPos = new Vector3();
    public float rootRot = 0f;

    private readonly float maxPos = 10;
    private readonly float minPos = -10;

    private void Start()
    {
        enemyRoot = enemyBodyParts.root;
        enemyRigids = enemyBodyParts.getRigids();
        physicsUtils = PhysicsUtils.get();
        observationsSpace = getEnemyObservations().Count;

        if (debug)
            InvokeRepeating("getEnemyObservations", 0.0f, .1f);
    }

    private void setMinMaxAxis()
    {
        maxAxis.x = -999;
        maxAxis.y = -999;

        minAxis.x = 999;
        minAxis.y = 999;

        foreach (Rigidbody rigid in enemyRigids)
        {
            if (!gameObject.Equals(enemyRoot))
            {
                Vector3 rbPos = enemyRoot.transform.InverseTransformPoint(rigid.transform.position);
                maxAxis.x = rbPos.x > maxAxis.x ? rbPos.x : maxAxis.x;
                maxAxis.y = rbPos.y > maxAxis.y ? rbPos.y : maxAxis.y;

                minAxis.x = rbPos.x < minAxis.x ? rbPos.x : minAxis.x;
                minAxis.y = rbPos.y < minAxis.y ? rbPos.y : minAxis.y;

            }
        }

    }

    private void setCOMs()
    {
        COM = physicsUtils.getCenterOfMass(enemyRigids) - enemyRoot.transform.position;
        COMVel = enemyRoot.transform.InverseTransformPoint(physicsUtils.getCenterOfMassVel(enemyRigids));
        COMRotVel = Quaternion.Inverse(enemyRoot.rotation) * physicsUtils.getCenterOfMassRotVel(enemyRigids);
        
    }

    public float normPos(float pos)
    {
        return (Mathf.Clamp(pos, minPos, maxPos) - minPos) / (maxPos - minPos);
    }

    public List<float> getEnemyObservations()
    {
        enemyObservations.Clear();

        setMinMaxAxis();
        setCOMs();

        enemyObservations.Add(maxAxis.x);
        enemyObservations.Add(maxAxis.y);
        enemyObservations.Add(minAxis.x);
        enemyObservations.Add(minAxis.y);
        enemyObservations.Add(COM.x);
        enemyObservations.Add(COM.y);
        enemyObservations.Add(COMVel.x);
        enemyObservations.Add(COMVel.y);
        enemyObservations.Add(COMRotVel.z);

        rootPos = enemyRoot.transform.position;
        rootPos.x = playerOne ? rootPos.x : -rootPos.x;
        rootRot = enemyRoot.transform.rotation.eulerAngles.z;

        enemyObservations.Add(normPos(rootPos.x));
        enemyObservations.Add(normPos(rootPos.y));
        enemyObservations.Add(rootRot);

        return enemyObservations;
    }

    public void setPlayerOne(bool playerOne)
    {
        this.playerOne = playerOne;
    }

}
