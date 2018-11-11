using System;
using UnityEngine;

public enum Parts
{
    BUTT,
    L_WAIST,
    U_WAIST,
    TORSO,
    HEAD,
    R_UPPER_ARM,
    R_LOWER_ARM,
    L_UPPER_ARM,
    L_LOWER_ARM,
    R_THIGH,
    R_SHIN,
    L_THIGH,
    L_SHIN
}

public static class PartsMethods
{
    public static Transform GetTransformFromAction(float value, BodyParts bodyParts)
    {
        Parts part = getPartFromAction(value);
        return GetTransformFromPart(part, bodyParts);
    }

    
    public static Parts getPartFromAction(float value)
    {
        float normVal = (value + 1f) * .5f;
        normVal *= 12f;

        int intVal = (int)Mathf.Round(normVal);
    
        return (Parts)intVal;
    }

    public static Transform GetTransformFromPart(Parts part, BodyParts bodyParts)
    {
        Transform bodyPart;
        switch (part)
        {
            case Parts.BUTT:
                bodyPart = bodyParts.getNamedRigids()["butt"].transform;
                break;
            case Parts.L_WAIST:
                bodyPart = bodyParts.getNamedRigids()["lwaist"].transform;
                break;
            case Parts.U_WAIST:
                bodyPart = bodyParts.getNamedRigids()["uwaist"].transform;
                break;
            case Parts.TORSO:
                bodyPart = bodyParts.getNamedRigids()["torso"].transform;
                break;
            case Parts.HEAD:
                bodyPart = bodyParts.getNamedRigids()["head"].transform;
                break;
            case Parts.R_UPPER_ARM:
                bodyPart = bodyParts.getNamedRigids()["rupperarm"].transform;
                break;
            case Parts.R_LOWER_ARM:
                bodyPart = bodyParts.getNamedRigids()["rlowerarm"].transform;
                break;
            case Parts.L_UPPER_ARM:
                bodyPart = bodyParts.getNamedRigids()["lupperarm"].transform;
                break;
            case Parts.L_LOWER_ARM:
                bodyPart = bodyParts.getNamedRigids()["llowerarm"].transform;
                break;
            case Parts.R_THIGH:
                bodyPart = bodyParts.getNamedRigids()["rthigh"].transform;
                break;
            case Parts.R_SHIN:
                bodyPart = bodyParts.getNamedRigids()["rshin"].transform;
                break;
            case Parts.L_THIGH:
                bodyPart = bodyParts.getNamedRigids()["lthigh"].transform;
                break;
            case Parts.L_SHIN:
                bodyPart = bodyParts.getNamedRigids()["lshin"].transform;
                break;
            default:
                bodyPart = bodyParts.getNamedRigids()["butt"].transform;
                break;
        }
        return bodyPart;
    }
}