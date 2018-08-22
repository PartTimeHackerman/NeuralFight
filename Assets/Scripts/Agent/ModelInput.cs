using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ModelInput : MonoBehaviour
{
    public int decFreq = 10;
    public int step = 0;
    private InternalModel model;
    public bool run = true;

    void Start()
    {
        model = new InternalModel("Models/ppo_WalkFW_5_nss", GetComponent<IObservations>(), GetComponent<IActions>());
    }


    void FixedUpdate()
    {
        if (run)
        {

            if (step >= decFreq)
            {
                model.act();
                step = 0;
            }

            step++;

        }
    }
}
