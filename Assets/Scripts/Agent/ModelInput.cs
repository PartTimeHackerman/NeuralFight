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
    public SingleJoystick singleJoystick;
    private Vector3 input;
    private Rigidbody headEnd;
    private VerticalEffector verticalEffector;
    private BodyParts bodyParts;
    public float force = 10f;
    public float jumpforce = 10f;
    private bool jumped = false;
    private Humanoid2DResetPos resetPos;
    public ObstaclesGenerator obstaclesGenerator;
    void Start()
    {
        jointInfosManager = new JointInfosManager(GetComponent<BodyParts>());
        obs = GetComponent<Observations>();
        actions = GetComponent<IActions>();
        modelWalkFW = new InternalModel("Models/ppo_WalkFWnew_9_nss", GetComponent<IObservations>(), GetComponent<IActions>());
        modelWalkBW = new InternalModel("Models/ppo_WalkBWnew_1_nss", GetComponent<IObservations>(), GetComponent<IActions>());
        modelStand = new InternalModel("Models/ppo_Standing_1_nss", GetComponent<IObservations>(), GetComponent<IActions>());
        headEnd = GetComponent<BodyParts>().getNamedRigids()["head_end"];
        bodyParts = GetComponent<BodyParts>();
        verticalEffector = GetComponent<VerticalEffector>();
        resetPos = GetComponent<Humanoid2DResetPos>();

        ColorShifter.maxDist = 1000f;
    }


    void FixedUpdate()
    {
        ColorShifter.currentDist = bodyParts.root.transform.position.x;
        if (bodyParts.root.transform.position.y < -2f)
        {
            resetPos.ResetPosition();
            obstaclesGenerator.resetRun();
        }
        if (run)
        {
            if (Input.GetKeyDown("space") && canJump())
            {
                jump();
            }

            if (Input.anyKey)
            {
                input = singleJoystick.GetInputDirection();
                horizontal = input.x;
                vertical = input.y;
                addVel(input, force, ForceMode.Impulse);
            }

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
        //horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        //horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        input = singleJoystick.GetInputDirection();
        horizontal = input.x;
        vertical = input.y;
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
        input = singleJoystick.GetInputDirection();
        horizontal = input.x;
        vertical = input.y;
        List<List<float>> actions = new List<List<float>>();

        if (Input.anyKey)
        {

            verticalEffector.enable = true;
            Dictionary<string, float> observationsNamed = obs.getObservationsNamed();
            jointInfosManager.enableJoints();
            if (horizontal > 0f) //if (Input.GetKey("right"))
            {
                //jointInfosManager.setJointsJointsForces(horizontal);
                Dictionary<string, float> observationsNamedNew = new Dictionary<string, float>(observationsNamed);
                observationsNamedNew.Remove("root_pos_x");
                actions.Add(
                    getScaledActions(modelWalkFW.getActions(observationsNamedNew.Select(kv => kv.Value).ToList()),
                        1f)); // horizontal));
            }
            if (horizontal < 0f)//else if (Input.GetKey("left"))
            {
                //jointInfosManager.setJointsJointsForces(-horizontal);
                Dictionary<string, float> observationsNamedNew = new Dictionary<string, float>(observationsNamed);
                observationsNamedNew.Remove("root_pos_x");
                actions.Add(
                    getScaledActions(modelWalkBW.getActions(observationsNamedNew.Select(kv => kv.Value).ToList()),
                        1f)); //-horizontal));
            }
            /*if (vertical < 0f)
            {
                actions.Add(getScaledActions(modelStand.getActions(observationsNamed.Select(kv => kv.Value).ToList()), -vertical));
            }*/

            if (actions.Count > 0)
            {
                List<float> avgAction = getAvgActions(actions);
                float mag = new Vector2(horizontal, vertical).magnitude;


                verticalEffector.velocity = mag * 1000f;
                jointInfosManager.setJointsJointsForces(mag);
                this.actions.applyActions(avgAction);
            }

        }
        else
        {
            verticalEffector.velocity = 0;
            verticalEffector.enable = false;
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

    private void addVel(Vector3 direction, float force, ForceMode forceMode)
    {
        foreach (Rigidbody rigid in bodyParts.getRigids())
        {
            rigid.AddForce(direction * force, forceMode);
        }
    }

    private void jump()
    {
        addVel(Vector3.up, jumpforce, ForceMode.Impulse);
        /*bodyParts.namedJoints["lthigh"].setConfigurableRotVel(new Vector3(0f, 0f,-25f));
        bodyParts.namedJoints["rthigh"].setConfigurableRotVel(new Vector3(0f, 0f,-25f));
        bodyParts.namedJoints["lshin"].setConfigurableRotVel(new Vector3(0f, 0f,25f));
        bodyParts.namedJoints["rshin"].setConfigurableRotVel(new Vector3(0f, 0f,25f));*/
    }

    public float distToFloor(Rigidbody rigidbody)
    {
        int layerMask = 1 << rigidbody.gameObject.layer;
        layerMask = ~layerMask;
        RaycastHit hit;
        if (Physics.Raycast(rigidbody.transform.position, Vector3.down, out hit, Mathf.Infinity, layerMask,
            QueryTriggerInteraction.Ignore))
        {
            return hit.distance;
        }
        else
            return Mathf.Infinity;
    }

    public bool canJump()
    {
        Rigidbody lfoot = bodyParts.getNamedRigids()["lfoot_end"];
        Rigidbody rfoot = bodyParts.getNamedRigids()["rfoot_end"];

        return distToFloor(lfoot) < .2f || distToFloor(rfoot) < .2f;

    }

    public void JumpSetDownState()
    { 
        if (canJump())
        {
            jump();
        }
    }
}
