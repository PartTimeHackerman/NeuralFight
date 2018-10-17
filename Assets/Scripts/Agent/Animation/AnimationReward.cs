using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class AnimationReward : MonoBehaviour
{
    public bool calcAvgReward = false;
    public bool debug = false;
    public BodyParts referenceBodyParts;
    public ReferenceObservations referenceObservations;
    public bool isAnimationRef = true;
    private BodyParts bodyParts;
    private int bodyPartsCount;
    private PhysicsUtils physics;

    private Rigidbody refRoot;
    private Rigidbody root;

    public float poseRew = 0f;
    public float velocityRew = 0f;
    public float endRew = 0f;
    public float COMRew = 0f;
    public float imitationReward = 0f;

    public float posErrSum;
    public float velErrSum;
    public float endPosErrSum;
    public float comPosErr;

    public float posErrExp = -2f;
    public float velErrExp = -.1f;
    public float endPosErrExp = -40f;
    public float comPosExp = -10f;

    void Start()
    {
        bodyParts = GetComponent<BodyParts>();
        bodyPartsCount = bodyParts.getNamedRigids().Count - 1;
        refRoot = referenceBodyParts.root;
        root = bodyParts.root;
        physics = PhysicsUtils.get();
        if (debug) InvokeRepeating("getReward", 0.0f, .1f);
    }


    public float getReward()
    {
        //poseRew = poseReward();
        //velocityRew = velocityReward();
        //endRew = endReward();
        //COMRew = COMReward();
        setRewards();
        imitationReward = poseRew * .65f + velocityRew * .15f + endRew * .2f;
        //imitationReward = poseRew * .65f + velocityRew * .1f + endRew * .15f + COMRew * .1f;
        //imitationReward *= 10f;
        //imitationReward = Mathf.Clamp(imitationReward, -100f, 100f);
        imitationReward = RewardFunctions.toleranceInvNoBounds(imitationReward, 1f, .1f, RewardFunction.LONGTAIL);
        return imitationReward;
    }

    private float getRelativeRot(Rigidbody rigidbody, Rigidbody root, bool isRef)
    {
        float rotAng;
        float rotClamped = 0f;
        Vector3 rot = (Quaternion.Inverse(root.rotation) * rigidbody.transform.rotation).eulerAngles;

        rotAng = isRef ? rot.x : rot.z;

        if (rotAng <= 180f)
            rotClamped = rotAng / 180f;
        else
            rotClamped = ((rotAng - 180f) / 180f) - 1f;

        return isRef ? -rotClamped : rotClamped;
    }

    private void setRewards()
    {
        posErrSum = 0f;
        velErrSum = 0f;
        endPosErrSum = 0f;
        foreach (KeyValuePair<string, Rigidbody> namedRigid in bodyParts.getNamedRigids())
        {
            //poseRew
            if (namedRigid.Key.Equals("butt"))
                continue;

            float modelRbPos = getRelativeRot(namedRigid.Value, root, false);
            float refRbPos = referenceObservations.relativeRots[namedRigid.Key];

            float posDiff = 0f;

            if ((modelRbPos > 0 && refRbPos < 0) || (modelRbPos < 0 && refRbPos > 0))
            {
                posDiff = Mathf.Abs(modelRbPos) + Mathf.Abs(refRbPos);
                if (posDiff > 1)
                {
                    posDiff = 2f - posDiff;
                }
            }
            else
            {
                posDiff = Mathf.Abs(Mathf.Abs(modelRbPos) - Mathf.Abs(refRbPos));
            }

            posErrSum += Mathf.Pow(posDiff, 2f);

            //velRew
            if (namedRigid.Key.Equals("butt"))
                continue;

            float modelRbVel = namedRigid.Value.angularVelocity.z;
            float refRbVel = referenceObservations.angularVelocities[namedRigid.Key].z;

            float velDiff = Mathf.Abs(modelRbVel - refRbVel);

            velErrSum += Mathf.Pow(velDiff, 2f);

            //endPosRew
            if (!namedRigid.Key.Contains("_end"))
                continue;

            Vector3 modelRbEndPos = root.transform.InverseTransformPoint(namedRigid.Value.transform.position);
            Vector3 refRbEndPos = referenceObservations.endPositions[namedRigid.Key];

            Vector2 v2Model = new Vector2(modelRbEndPos.z, modelRbEndPos.y);
            Vector2 v2Ref = new Vector2(isAnimationRef ? refRbEndPos.x : refRbEndPos.z, refRbEndPos.y);

            float dist = Vector2.Distance(v2Model, v2Ref);
            endPosErrSum += Mathf.Pow(dist, 2f);
        }

        poseRew = Mathf.Exp(posErrExp * posErrSum);
        velocityRew = Mathf.Exp(velErrExp * velErrSum);
        endRew = Mathf.Exp(endPosErrExp * endPosErrSum);
    }

    private float poseReward()
    {
        posErrSum = 0f;
        foreach (KeyValuePair<string, Rigidbody> namedRigid in bodyParts.getNamedRigids())
        {
            if (namedRigid.Key.Equals("butt"))
                continue;

            float modelRbPos = getRelativeRot(namedRigid.Value, root, false);
            float refRbPos = referenceObservations.relativeRots[namedRigid.Key];

            float posDiff = 0f;

            if ((modelRbPos > 0 && refRbPos < 0) || (modelRbPos < 0 && refRbPos > 0))
            {
                posDiff = Mathf.Abs(modelRbPos) + Mathf.Abs(refRbPos);
                if (posDiff > 1)
                {
                    posDiff = 2f - posDiff;
                }
            }
            else
            {
                posDiff = Mathf.Abs(Mathf.Abs(modelRbPos) - Mathf.Abs(refRbPos));
            }

            posErrSum += Mathf.Pow(posDiff, 2f);
        }

        return Mathf.Exp(posErrExp * posErrSum);
    }

    private float velocityReward()
    {
        velErrSum = 0f;
        foreach (KeyValuePair<string, Rigidbody> namedRigid in bodyParts.getNamedRigids())
        {
            if (namedRigid.Key.Equals("butt"))
                continue;

            float modelRbVel = namedRigid.Value.angularVelocity.z;
            float refRbVel = referenceObservations.angularVelocities[namedRigid.Key].z;

            float velDiff = Mathf.Abs(modelRbVel - refRbVel);

            velErrSum += Mathf.Pow(velDiff, 2f);
        }

        return Mathf.Exp(velErrExp * velErrSum);
    }

    private float endReward()
    {
        endPosErrSum = 0f;
        foreach (KeyValuePair<string, Rigidbody> namedRigid in bodyParts.getNamedRigids())
        {
            if (!namedRigid.Key.Contains("_end"))
                continue;

            Vector3 modelRbEndPos = root.transform.InverseTransformPoint(namedRigid.Value.transform.position);
            Vector3 refRbEndPos = referenceObservations.endPositions[namedRigid.Key];

            Vector2 v2Model = new Vector2(modelRbEndPos.z, modelRbEndPos.y);
            Vector2 v2Ref = new Vector2(isAnimationRef ? refRbEndPos.x : refRbEndPos.z, refRbEndPos.y);

            float dist = Vector2.Distance(v2Model, v2Ref);
            endPosErrSum += Mathf.Pow(dist, 2f);
        }

        return Mathf.Exp(endPosErrExp * endPosErrSum);
    }

    private float COMReward()
    {
        Vector3 COM = referenceObservations.COM;
        Vector3 refCOM = physics.getCenterOfMass(referenceBodyParts.getRigids()) -
                         referenceBodyParts.root.transform.position;
        COM.z = 0;
        refCOM.z = 0;
        comPosErr = Vector3.Distance(COM, refCOM);

        return Mathf.Exp(comPosExp * Mathf.Pow(comPosErr, 2f));
    }
}