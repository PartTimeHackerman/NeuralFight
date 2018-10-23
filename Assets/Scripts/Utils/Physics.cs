using System.Collections.Generic;
using UnityEngine;

public class PhysicsUtils
{
    private static readonly object syncLock = new object();
    private static PhysicsUtils physics;


    public static PhysicsUtils get()
    {
        if (physics == null)
            lock (syncLock)
            {
                physics = new PhysicsUtils();
            }

        return physics;
    }
    
    public void getCenterOfMassAll(List<Rigidbody> rigids, out Vector3 com,out Vector3 comVel, out Vector3 comAngVel)
    {
        com = Vector2.zero;
        comVel = Vector2.zero;
        comAngVel = Vector2.zero;
        float massSum = 0f;

        for (int i = rigids.Count - 1; i >= 0; i--)
        {
            Rigidbody rb = rigids[i];
            com += rb.transform.TransformPoint(rb.centerOfMass) * rb.mass;
            comVel += rb.velocity * rb.mass;
            comAngVel += rb.angularVelocity * rb.mass;
            massSum += rb.mass;
        }
        
        com = com / massSum;
        comVel = comVel / massSum;
        comAngVel = comAngVel / massSum;
    }

    public Vector3 getCenterOfMass(List<Rigidbody> rigids)
    {
        Vector3 com = Vector2.zero;
        float massSum = 0f;

        for (int i = rigids.Count - 1; i >= 0; i--)
        {
            Rigidbody rb = rigids[i];
            com += rb.transform.TransformPoint(rb.centerOfMass) * rb.mass;
            massSum += rb.mass;
        }
        
        com = com / massSum;

        return com;
    }

    public Vector2 getCoMAxis(List<Rigidbody> rigids)
    {
        var com = new Vector3();
        com.x = getAxisCenterOfMass(rigids, 1);
        com.y = getAxisCenterOfMass(rigids, 2);
        com.z = getAxisCenterOfMass(rigids, 3);
        return com;
    }

    private float getAxisCenterOfMass(List<Rigidbody> rigids, int axis)
    {
        float sumMass = 0;
        float sumMassMulAxis = 0;

        foreach (var rigidBody in rigids) sumMass += rigidBody.mass;

        switch (axis)
        {
            case 1:
                foreach (var rigidBody in rigids) sumMassMulAxis += rigidBody.mass * rigidBody.transform.position.x;
                break;
            case 2:
                foreach (var rigidBody in rigids) sumMassMulAxis += rigidBody.mass * rigidBody.transform.position.y;
                break;
            case 3:
                foreach (var rigidBody in rigids) sumMassMulAxis += rigidBody.mass * rigidBody.transform.position.z;
                break;
        }

        return sumMassMulAxis / sumMass;
    }

    public Vector3 getCenterOfMassVel(List<Rigidbody> rigids)
    {
        Vector3 com = Vector2.zero;
        float massSum = 0f;

        for (int i = rigids.Count - 1; i >= 0; i--)
        {
            Rigidbody rb = rigids[i];
            com += rb.velocity * rb.mass;
            massSum += rb.mass;
        }
        
        com = com / massSum;

        return com;
    }

    public Vector3 getCenterOfMassVelAxis(List<Rigidbody> rigids)
    {
        var com = new Vector3();
        com.x = getAxisCenterOfMassVel(rigids, 1);
        com.y = getAxisCenterOfMassVel(rigids, 2);
        com.z = getAxisCenterOfMassVel(rigids, 3);
        return com;
    }

    private float getAxisCenterOfMassVel(List<Rigidbody> rigids, int axis)
    {
        float sumMass = 0;
        float sumMassMulAxis = 0;

        foreach (var rigidBody in rigids) sumMass += rigidBody.mass;

        switch (axis)
        {
            case 1:
                foreach (var rigidBody in rigids) sumMassMulAxis += rigidBody.mass * rigidBody.velocity.x;
                break;
            case 2:
                foreach (var rigidBody in rigids) sumMassMulAxis += rigidBody.mass * rigidBody.velocity.y;
                break;
            case 3:
                foreach (var rigidBody in rigids) sumMassMulAxis += rigidBody.mass * rigidBody.velocity.z;
                break;
        }

        return sumMassMulAxis / sumMass;
    }

    public Vector3 getCenterOfMassRotVel(List<Rigidbody> rigids)
    {
        Vector3 com = Vector2.zero;
        float massSum = 0f;

        for (int i = rigids.Count - 1; i >= 0; i--)
        {
            Rigidbody rb = rigids[i];
            com += rb.angularVelocity * rb.mass;
            massSum += rb.mass;
        }
        
        com = com / massSum;

        return com;
    }

    public Vector3 getCenterOfMassRotVelAxis(List<Rigidbody> rigids)
    {
        var com = new Vector3();
        com.x = getAxisCenterOfMassRotVel(rigids, 1);
        com.y = getAxisCenterOfMassRotVel(rigids, 2);
        com.z = getAxisCenterOfMassRotVel(rigids, 3);
        return com;
    }

    private float getAxisCenterOfMassRotVel(List<Rigidbody> rigids, int axis)
    {
        float sumMass = 0;
        float sumMassMulAxis = 0;

        foreach (var rigidBody in rigids) sumMass += rigidBody.mass;

        switch (axis)
        {
            case 1:
                foreach (var rigidBody in rigids) sumMassMulAxis += rigidBody.mass * rigidBody.angularVelocity.x;
                break;
            case 2:
                foreach (var rigidBody in rigids) sumMassMulAxis += rigidBody.mass * rigidBody.angularVelocity.y;
                break;
            case 3:
                foreach (var rigidBody in rigids) sumMassMulAxis += rigidBody.mass * rigidBody.angularVelocity.z;
                break;
        }

        return sumMassMulAxis / sumMass;
    }
}