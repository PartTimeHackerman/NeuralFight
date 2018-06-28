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

    public Vector3 getCenterOfMass(List<Rigidbody> rigits)
    {
        var com = new Vector3();
        com.x = getAxisCenterOfMass(rigits, 1);
        com.y = getAxisCenterOfMass(rigits, 2);
        com.z = getAxisCenterOfMass(rigits, 3);
        return com;
    }

    private float getAxisCenterOfMass(List<Rigidbody> rigits, int axis)
    {
        float sumMass = 0;
        float sumMassMulAxis = 0;

        foreach (var rigidBody in rigits) sumMass += rigidBody.mass;

        switch (axis)
        {
            case 1:
                foreach (var rigidBody in rigits) sumMassMulAxis += rigidBody.mass * rigidBody.transform.position.x;
                break;
            case 2:
                foreach (var rigidBody in rigits) sumMassMulAxis += rigidBody.mass * rigidBody.transform.position.y;
                break;
            case 3:
                foreach (var rigidBody in rigits) sumMassMulAxis += rigidBody.mass * rigidBody.transform.position.z;
                break;
        }

        return sumMassMulAxis / sumMass;
    }

    public Vector3 getCenterOfMassVel(List<Rigidbody> rigits)
    {
        var com = new Vector3();
        com.x = getAxisCenterOfMassVel(rigits, 1);
        com.y = getAxisCenterOfMassVel(rigits, 2);
        com.z = getAxisCenterOfMassVel(rigits, 3);
        return com;
    }

    private float getAxisCenterOfMassVel(List<Rigidbody> rigits, int axis)
    {
        float sumMass = 0;
        float sumMassMulAxis = 0;

        foreach (var rigidBody in rigits) sumMass += rigidBody.mass;

        switch (axis)
        {
            case 1:
                foreach (var rigidBody in rigits) sumMassMulAxis += rigidBody.mass * rigidBody.velocity.x;
                break;
            case 2:
                foreach (var rigidBody in rigits) sumMassMulAxis += rigidBody.mass * rigidBody.velocity.y;
                break;
            case 3:
                foreach (var rigidBody in rigits) sumMassMulAxis += rigidBody.mass * rigidBody.velocity.z;
                break;
        }

        return sumMassMulAxis / sumMass;
    }

    public Vector3 getCenterOfMassRotVel(List<Rigidbody> rigits)
    {
        var com = new Vector3();
        com.x = getAxisCenterOfMassRotVel(rigits, 1);
        com.y = getAxisCenterOfMassRotVel(rigits, 2);
        com.z = getAxisCenterOfMassRotVel(rigits, 3);
        return com;
    }

    private float getAxisCenterOfMassRotVel(List<Rigidbody> rigits, int axis)
    {
        float sumMass = 0;
        float sumMassMulAxis = 0;

        foreach (var rigidBody in rigits) sumMass += rigidBody.mass;

        switch (axis)
        {
            case 1:
                foreach (var rigidBody in rigits) sumMassMulAxis += rigidBody.mass * rigidBody.angularVelocity.x;
                break;
            case 2:
                foreach (var rigidBody in rigits) sumMassMulAxis += rigidBody.mass * rigidBody.angularVelocity.y;
                break;
            case 3:
                foreach (var rigidBody in rigits) sumMassMulAxis += rigidBody.mass * rigidBody.angularVelocity.z;
                break;
        }

        return sumMassMulAxis / sumMass;
    }
}