using System;
using System.Collections.Generic;
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
    private static List<Parts> AllParts = new List<Parts>();
    private static List<Parts> RightParts = new List<Parts>();
    private static List<Parts> LeftParts = new List<Parts>();
    private static List<Parts> MiddleParts = new List<Parts>();

    static PartsMethods()
    {
        foreach (Parts part in (Parts[]) Enum.GetValues(typeof(Parts)))
        {
            switch (part)
            {
                case Parts.BUTT:
                    AllParts.Add(part);
                    RightParts.Add(part);
                    LeftParts.Add(part);
                    MiddleParts.Add(part);
                    break;
                case Parts.L_WAIST:
                    AllParts.Add(part);
                    RightParts.Add(part);
                    LeftParts.Add(part);
                    MiddleParts.Add(part);
                    break;
                case Parts.U_WAIST:
                    AllParts.Add(part);
                    RightParts.Add(part);
                    LeftParts.Add(part);
                    MiddleParts.Add(part);
                    break;
                case Parts.TORSO:
                    AllParts.Add(part);
                    RightParts.Add(part);
                    LeftParts.Add(part);
                    MiddleParts.Add(part);
                    break;
                case Parts.HEAD:
                    AllParts.Add(part);
                    RightParts.Add(part);
                    LeftParts.Add(part);
                    MiddleParts.Add(part);
                    break;
                case Parts.R_UPPER_ARM:
                    AllParts.Add(part);
                    RightParts.Add(part);
                    break;
                case Parts.R_LOWER_ARM:
                    AllParts.Add(part);
                    RightParts.Add(part);
                    break;
                case Parts.L_UPPER_ARM:
                    AllParts.Add(part);
                    LeftParts.Add(part);
                    break;
                case Parts.L_LOWER_ARM:
                    AllParts.Add(part);
                    LeftParts.Add(part);
                    break;
                case Parts.R_THIGH:
                    AllParts.Add(part);
                    RightParts.Add(part);
                    MiddleParts.Add(part);
                    break;
                case Parts.R_SHIN:
                    AllParts.Add(part);
                    RightParts.Add(part);
                    MiddleParts.Add(part);
                    break;
                case Parts.L_THIGH:
                    AllParts.Add(part);
                    LeftParts.Add(part);
                    MiddleParts.Add(part);
                    break;
                case Parts.L_SHIN:
                    AllParts.Add(part);
                    LeftParts.Add(part);
                    MiddleParts.Add(part);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public static Transform GetTransformFromAction(float value, BodyParts bodyParts, WeaponHand weaponHand)
    {
        Parts part = getPartFromAction(value, weaponHand);
        return GetTransformFromPart(part, bodyParts);
    }


    public static Parts getPartFromAction(float value, WeaponHand weaponHand)
    {
        float normVal = (value + 1f) * .5f;
        Parts part;
        switch (weaponHand)
        {
            case WeaponHand.RIGHT:
                part = LeftParts[Mathf.RoundToInt(Mathf.Lerp(0, LeftParts.Count - 1, normVal))];
                break;
            case WeaponHand.LEFT:
                part = RightParts[Mathf.RoundToInt(Mathf.Lerp(0, RightParts.Count - 1, normVal))];
                break;
            case WeaponHand.BOTH:
                part = MiddleParts[Mathf.RoundToInt(Mathf.Lerp(0, MiddleParts.Count - 1, normVal))];
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(weaponHand), weaponHand, null);
        }
        return part;
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