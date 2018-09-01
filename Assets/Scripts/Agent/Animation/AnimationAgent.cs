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
    private TerminateDuel terminateFn;
    private Humanoid2DResetPos resetPosition;

    public int steps = 0;
    public int maxSteps = 100;
    public bool ready = true;
    private float rewardAnim = 0f;
    private bool first = true;

    public override void InitializeAgent()
    {
        observations = GetComponent<Observations>();
        animationReward = GetComponent<AnimationReward>();
        actions = GetComponent<Humanoid2DActionsAngPos>();
        animationPositioner = GetComponent<AnimationPositioner>();
        bodyParts = GetComponent<BodyParts>();
        observations.addToRemove(new[] { "root_pos_x" });
        terminateFn = GetComponent<TerminateDuel>();
        resetPosition = GetComponent<Humanoid2DResetPos>();
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
        agentParameters.numberOfActionsBetweenDecisions =
            Mathf.Max(agentParameters.numberOfActionsBetweenDecisions, 1);
        if (!agentParameters.onDemandDecision && ready)
        {
            RequestAction();
            if (academyStepCounter % agentParameters.numberOfActionsBetweenDecisions == 0)
            {
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

        bool terminate = terminateFn.isTerminated();

        SetReward(rewardAnim);
        if (steps > maxSteps || terminate)
        {
            steps = 0;
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
        resetPosition.ResetPosition();
        ready = false;
        resetJointVels();
        //animationPositioner.setVelocities();
        animationPositioner.setRotationsRigids();
        ready = true;
    }
}