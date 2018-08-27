using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


class ModelInput : MonoBehaviour
{
    public int decFreq = 10;
    public int step = 0;
    private InternalModel modelWalkFW;
    private InternalModel modelWalkBW;
    private InternalModel modelStand;
    public bool run = true;
    private Observations obs;
    private JointInfosManager jointInfosManager;
    private IActions actions;
    public float horizontal, vertical;


    void Start()
    {
        jointInfosManager = new JointInfosManager(GetComponent<BodyParts>());
        obs = GetComponent<Observations>();
        actions = GetComponent<IActions>();
        modelWalkFW = new InternalModel("Models/ppo_WalkFW_1_nss", GetComponent<IObservations>(), GetComponent<IActions>());
        modelWalkBW = new InternalModel("Models/ppo_WalkBW_1_nss", GetComponent<IObservations>(), GetComponent<IActions>());
        modelStand = new InternalModel("Models/ppo_Standing_1_nss", GetComponent<IObservations>(), GetComponent<IActions>());
    }


    void FixedUpdate()
    {
        if (run)
        {
            if (step >= decFreq)
            {
                runElastic();
                step = 0;
            }
            step++;

        }
    }

    private void runRigid()
    {
        horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        vertical = CrossPlatformInputManager.GetAxis("Vertical");
        if (Input.anyKey)
        {
            Dictionary<string, float> observationsNamed = obs.getObservationsNamed();
            jointInfosManager.enableJoints();
            if (horizontal > 0f) //if (Input.GetKey("right"))
            {
                jointInfosManager.setJointsJointsForces(horizontal);
                observationsNamed.Remove("root_pos_x");
                modelWalkFW.act(observationsNamed.Select(kv => kv.Value).ToList());
            }
            else if (horizontal < 0f)//else if (Input.GetKey("left"))
            {
                jointInfosManager.setJointsJointsForces(-horizontal);
                observationsNamed.Remove("root_pos_x");
                modelWalkBW.act(observationsNamed.Select(kv => kv.Value).ToList());
            }
            else if (vertical < 0f)
            {
                modelStand.act(observationsNamed.Select(kv => kv.Value).ToList());
            }

        }
        else
        {
            jointInfosManager.disableJoints();
        }

    }

    private void runElastic()
    {
        horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        vertical = CrossPlatformInputManager.GetAxis("Vertical");
        List<List<float>> actions = new List<List<float>>();
        if (Input.anyKey)
        {
            Dictionary<string, float> observationsNamed = obs.getObservationsNamed();
            jointInfosManager.enableJoints();
            if (horizontal > 0f) //if (Input.GetKey("right"))
            {
                //jointInfosManager.setJointsJointsForces(horizontal);
                Dictionary<string, float> observationsNamedNew = new Dictionary<string, float>(observationsNamed);
                observationsNamedNew.Remove("root_pos_x");
                actions.Add(getScaledActions(modelWalkFW.getActions(observationsNamedNew.Select(kv => kv.Value).ToList()), horizontal));
            }
            if (horizontal < 0f)//else if (Input.GetKey("left"))
            {
                //jointInfosManager.setJointsJointsForces(-horizontal);
                Dictionary<string, float> observationsNamedNew = new Dictionary<string, float>(observationsNamed);
                observationsNamedNew.Remove("root_pos_x");
                actions.Add(getScaledActions(modelWalkBW.getActions(observationsNamedNew.Select(kv => kv.Value).ToList()), -horizontal));
            }
            if (vertical < 0f)
            {
                actions.Add(getScaledActions(modelStand.getActions(observationsNamed.Select(kv => kv.Value).ToList()), -vertical));
            }

            if (actions.Count > 0)
            {
                List<float> avgAction = getAvgActions(actions);
                float mag = new Vector2(horizontal, vertical).magnitude;
                jointInfosManager.setJointsJointsForces(mag);
                this.actions.applyActions(avgAction);
            }

        }
        else
        {
            jointInfosManager.disableJoints();
        }
    }

    private List<float> getScaledActions(List<float> actions, float scale)
    {
        for (int i = 0; i < actions.Count; i++)
        {
            actions[i] *= scale;
        }

        return actions;
    }

    private List<float> getAvgActions(List<List<float>> actions)
    {
        List<float> averaged = new List<float>(new float[actions[0].Count]);
        int actionsCount = actions.Count;

        foreach (List<float> action in actions)
        {
            for (int i = 0; i < action.Count; i++)
            {
                averaged[i] += action[i];
            }
        }
/*

        for (int i = 0; i < averaged.Count; i++)
        {
            averaged[i] /= actionsCount;
        }
*/

        return averaged;
    }
}
