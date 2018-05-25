using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperLegsCollidingWithSurface : MonoBehaviour
{

    public AntReward antReward;
    public int id;

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.name == "Plane")
        {
            antReward.upperLegsSurfaceCols++;
        }
    }
}
