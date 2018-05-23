using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class VelDebug : MonoBehaviour
{

    public Vector3 velocity = new Vector3();



    public void Update()
    {
        gameObject.GetComponent<Rigidbody>().velocity = velocity;
    }

}
