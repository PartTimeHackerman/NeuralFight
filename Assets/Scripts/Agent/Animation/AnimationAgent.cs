using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Agent;
using MLAgents;
using UnityEngine;

internal class AnimationAgent : Agent
{
    private ApplicationSettings applicationSettings;
    private IActions actions;

    private Observations observations;
    private AnimationReward animationReward;
    private AnimationPositioner animationPositioner;
    public AnimationSettings animationSettings;
    private BodyParts bodyParts;

    public int steps = 0;
    public int maxSteps = 100;
    public int actionSteps = 0;
    public int resetPosCounter = 0;
    public int resetPosLimit = 2;
    public bool ready = true;
    private float rewardAnim = 0f;

    public override void InitializeAgent()
    {
        observations = GetComponent<Observations>();
        animationReward = GetComponent<AnimationReward>();
        actions = GetComponent<Humanoid2DActionsAngPos>();
        animationPositioner = GetComponent<AnimationPositioner>();
        bodyParts = GetComponent<BodyParts>();
        observations.addToRemove(new[] { "root_pos_x" });
        //animationReward.getAvgReward();
    }

    public override void CollectObservations()
    {

        List<float> observations = this.observations.getObservations();

        foreach (var observation in observations) AddVectorObs(observation);
    }

    protected override void MakeRequests(int academyStepCounter)
    {
        steps++;
        if (resetPosCounter > resetPosLimit)
        {
            animationSettings.speed = 1;
            ready = true;
        }
        else
        {
            animationPositioner.setRotationsRigids();
            resetPosCounter++;
        }

        agentParameters.numberOfActionsBetweenDecisions =
            Mathf.Max(agentParameters.numberOfActionsBetweenDecisions, 1);
        if (!agentParameters.onDemandDecision && ready)
        {
            RequestAction();
            if (academyStepCounter % agentParameters.numberOfActionsBetweenDecisions == 0)
            {
                actionSteps++;
                RequestDecision();

            }
        }
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        if (!ready)
            return;

        List<float> actions = vectorAction.ToList();
        List<float> actionsClamped = new List<float>();
        foreach (var var in actions)
            actionsClamped.Add(Mathf.Clamp(var, -1f, 1f));


        this.actions.applyActions(actionsClamped);
        rewardAnim = animationReward.getReward();
        SetReward(rewardAnim);
        if (rewardAnim < 2f)
        {
            resetPos();
        }

        if (steps > maxSteps)
        {
            steps = 0;
            actionSteps = 0;
            resetPos();
            Done();
        }
    }

    public override void AgentReset()
    {

    }

    private void resetJointVels()
    {
        foreach (JointInfo jointInfo in bodyParts.jointsInfos)
        {
            jointInfo.setConfigurableRotVel(Vector3.zero);
        }
    }

    private void resetPos()
    {
        resetPosCounter = 0;
        ready = false;
        animationSettings.speed = 0;
        resetJointVels();
        animationPositioner.setVelocities();
        animationPositioner.setRotationsRigids();
    }
}