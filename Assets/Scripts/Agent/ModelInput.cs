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
    private InternalModel modelWalk;
    private InternalModel modelStand;
    public bool run = true;
    private Observations obs;
    public bool right = false;

    void Start()
    {
        // standObs = new Observations(GetComponent<BodyParts>());
        //walkObs = new Observations(GetComponent<BodyParts>());
        //walkObs.addToRemove(new[] { "root_pos_x" });
        obs = GetComponent<Observations>();
        modelWalk = new InternalModel("Models/ppo_WalkFW_1_nss", GetComponent<IObservations>(), GetComponent<IActions>());
        modelStand = new InternalModel("Models/ppo_Standing_1_nss", GetComponent<IObservations>(), GetComponent<IActions>());
    }


    void FixedUpdate()
    {
        right = Input.GetKey("right");
        if (run)
        {

            if (step >= decFreq)
            {
                Dictionary<string, float> observationsNamed = obs.getObservationsNamed();
                if (right)
                {
                    observationsNamed.Remove("root_pos_x");
                    modelWalk.act(observationsNamed.Select(kv => kv.Value).ToList());
                    Debug.Log("r");
                }
                else
                {
                    modelStand.act(observationsNamed.Select(kv => kv.Value).ToList());
                }
                step = 0;
            }

            step++;

        }
    }
}
