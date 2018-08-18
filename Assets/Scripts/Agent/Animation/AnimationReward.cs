﻿using System;
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
    public bool isAnimationRef = true;
    private BodyParts bodyParts;
    private int bodyPartsCount;

    private Rigidbody refRoot;
    private Rigidbody root;

    public float rotErr = 0f;
    public float posErr = 0f;
    public float reward = 0f;

    private float nextUpdate = .1f;

    public float avgReward = 0f;
    public float avgRewardSum = 0f;
    public float avgCounter = 0f;


    void Start()
    {
        bodyParts = GetComponent<BodyParts>();
        bodyPartsCount = bodyParts.getNamedRigids().Count - 1;
        refRoot = referenceBodyParts.root;
        root = bodyParts.root;
    }

    void LateFixedUpdate()
    {
        if (isAnimationRef)
        {
            Vector3 refRootRot = refRoot.transform.rotation.eulerAngles;
            Vector3 rootRot = root.transform.rotation.eulerAngles;
            rootRot.z = -refRootRot.x;
            root.transform.rotation = Quaternion.Euler(rootRot);
        }

        if (calcAvgReward)
        {
            getReward();

            avgRewardSum += reward;
            avgCounter++;
            avgReward = avgRewardSum / avgCounter;
        }
    }

    void LateUpdate()
    {
        /*if (Time.time >= nextUpdate && debug)
        {
            nextUpdate = Time.time + .1f;
            getReward();

        }
        if (avgCounter > 10 && debug)
        {
            Debug.Log(avgRewardSum);
            float avgRew = getAvgReward();
            Debug.Log(avgRew);
        }
        */
    }

    public float getAvgReward()
    {
        float avgRew = avgRewardSum / (avgCounter == 0f ? 1 :avgCounter );
        avgCounter = 0f;
        avgRewardSum = 0f;
        return avgRew;
    }


    public float getReward()
    {
        rotErr = getRotErr();
        posErr = getPosErr();
        reward = (rotErr + posErr) / 2f;
        reward = RewardFunctions.toleranceInvNoBounds(reward, .4f, .1f, RewardFunction.LONGTAIL);

        return reward;
    }

    private float getPosErr()
    {
        float posErrSum = 0f;
        foreach (KeyValuePair<string, Rigidbody> namedRigid in bodyParts.getNamedRigids())
        {
            if (namedRigid.Key.Equals("butt"))
                continue;

            Vector3 modelRbPos = root.transform.InverseTransformPoint(namedRigid.Value.transform.position);
            Vector3 refRbPos = refRoot.transform.InverseTransformPoint(referenceBodyParts.getNamedRigids()[namedRigid.Key].transform.position);

            Vector2 v2Model = new Vector2(modelRbPos.z, modelRbPos.y);
            Vector2 v2Ref = new Vector2(isAnimationRef ? refRbPos.x : refRbPos.z, refRbPos.y);

            float dist = Vector2.Distance(v2Model, v2Ref) - .1f;
            dist = Mathf.Abs(Mathf.Clamp(dist, 0, 1) - 1f);
            dist = RewardFunctions.toleranceInvNoBounds(dist, .4f, .1f, RewardFunction.LONGTAIL);
            posErrSum += dist;

        }

        return posErrSum / bodyPartsCount;
    }

    private float getRotErr()
    {
        float rotErrSum = 0f;
        foreach (KeyValuePair<string, Rigidbody> namedRigid in bodyParts.getNamedRigids())
        {
            if (namedRigid.Key.Equals("butt"))
                continue;

            float modelRbRot = getRelativeRot(namedRigid.Value, root, false);
            float refRbRot = getRelativeRot(referenceBodyParts.getNamedRigids()[namedRigid.Key], refRoot, isAnimationRef);

            float rotDiff = 0f;
                

            if ((modelRbRot > 0 && refRbRot < 0) || (modelRbRot < 0 && refRbRot > 0))
            {
                rotDiff = Mathf.Abs(modelRbRot) + Mathf.Abs(refRbRot);
                if (rotDiff > 1)
                {
                    rotDiff = 2f - rotDiff;
                }
            }
            else
            {
                rotDiff = Mathf.Abs(Mathf.Abs(modelRbRot) - Mathf.Abs(refRbRot));
            }
            rotDiff = Mathf.Abs(rotDiff - 1);
            rotErrSum += rotDiff;

        }

        return rotErrSum / bodyPartsCount;


    }

    private float getRelativeRot(Rigidbody rigidbody, Rigidbody root, bool isRef)
    {
        float rotAng;
        float rotClamped = 0f;
        Vector3 rot = (Quaternion.Inverse(root.rotation) * rigidbody.transform.rotation).eulerAngles;

        if (isRef)
            rotAng = rot.x;
        else
            rotAng = rot.z;
        
        if (rotAng <= 180f)
            rotClamped = rotAng / 180f;
        else
            rotClamped = ((rotAng - 180f) / 180f) - 1f;

        return isRef ? -rotClamped : rotClamped;
    }
}
