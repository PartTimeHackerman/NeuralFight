using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Part
{
    public static readonly Part BUTT = new Part("butt", .2f, 15);
    public static readonly Part L_WAIST = new Part("lwaist", .14f, 10);
    public static readonly Part U_WAIST = new Part("uwaist", .16f, 10);
    public static readonly Part TORSO = new Part("torso", .18f, 12);
    public static readonly Part HEAD = new Part("head", .21f, 2);
    public static readonly Part R_UPPER_ARM = new Part("rupperarm", .4f, 5);
    public static readonly Part R_LOWER_ARM = new Part("rlowerarm", .34f, 2);
    public static readonly Part L_UPPER_ARM = new Part("lupperarm", .4f, 5);
    public static readonly Part L_LOWER_ARM = new Part("llowerarm", .34f, 2);
    public static readonly Part R_THIGH = new Part("rthigh", .5f, 10);
    public static readonly Part R_SHIN = new Part("rshin", .48f, 6);
    public static readonly Part L_THIGH = new Part("lthigh", .5f, 6);
    public static readonly Part L_SHIN = new Part("lshin", .48f, 6);

    public static IEnumerable<Part> Values
    {
        get
        {
            yield return BUTT;
            yield return L_WAIST;
            yield return U_WAIST;
            yield return TORSO;
            yield return HEAD;
            yield return R_UPPER_ARM;
            yield return R_LOWER_ARM;
            yield return L_UPPER_ARM;
            yield return L_LOWER_ARM;
            yield return R_THIGH;
            yield return R_SHIN;
            yield return L_THIGH;
            yield return L_SHIN;
        }
    }

    public string Name { get; private set; }
    public float BaseSize { get; private set; }
    public float Mass { get; private set; }

    public Part(string name, float baseSize, float mass)
    {
        Name = name;
        BaseSize = baseSize;
        Mass = mass;
    }

    public static Transform GetTransformFromPart(Part part, BodyParts bodyParts)
    {
        return bodyParts.getNamedRigids()[part.Name].transform;
    }

    public static Part GetPartByName(string name)
    {
        foreach (Part part in Values)
        {
            if (part.Name.Equals(name))
            {
                return part;
            }
        }

        return null;
    }

    public override string ToString()
    {
        return Name;
    }
}