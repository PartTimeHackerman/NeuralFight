using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class LocalRot : MonoBehaviour
{


    #if (UNITY_EDITOR)
    public DictionaryStringFloat rots = new DictionaryStringFloat();
    #endif
    private BodyParts bodyParts;

    void Start()
    {
        bodyParts = GetComponent<BodyParts>();
    }

    void LateFixedUpdate()
    {
        foreach (JointInfo jointInfo in bodyParts.jointsInfos)
        {
            float rot = jointInfo.transform.localRotation.eulerAngles.x;
            rot = rot < 180 ? -rot : (360 - rot);
            #if (UNITY_EDITOR)
            rots[jointInfo.name] = rot;
            #endif
        }
    }

}
