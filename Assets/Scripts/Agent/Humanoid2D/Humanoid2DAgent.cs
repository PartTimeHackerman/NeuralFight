using System.Collections.Generic;
using System.Linq;
using UnityEngine;

internal class Humanoid2DAgent : Agent
{
    private ApplicationSettings applicationSettings;
    private IActions actions;

    public int episodes;
    private readonly Epoch epoch = Epoch.get();

    private Humanoid2DObservations observations;
    private PausePos pausePos;
    private Humanoid2DResetPos resetPos;
    private Humanoid2DStandingReward standingReward;
    private long startTime;

    public int resetWaitSteps = 5;
    private int resetStepsElapsed = 0;
    private bool velReset = false;
    public int steps = 0;
    public int maxSteps = 0;
    public int decisionFrequency = 5;

    public override void InitializeAgent()
    {
        observations = GetComponent<Humanoid2DObservations>();
        standingReward = GetComponent<Humanoid2DStandingReward>();
        actions = GetComponent<Humanoid2DActionsAngPos>();
        resetPos = GetComponent<Humanoid2DResetPos>();
        pausePos = GetComponent<PausePos>();
        observations.decisionFrequency = agentParameters.numberOfActionsBetweenDecisions;
    }

    public override void CollectObservations()
    {
        List<float> observations = this.observations.getObservations();
        foreach (var observation in observations) AddVectorObs(observation);
        SetTextObs("Testing " + gameObject.GetInstanceID());
    }

    protected override void MakeRequests(int academyStepCounter)
    {
        if (resetStepsElapsed > resetWaitSteps && !velReset)
        {
            resetPos.resetVel();
            resetPos.resetJointForces();
            velReset = true;
        }

        resetStepsElapsed++;
        agentParameters.numberOfActionsBetweenDecisions =
            Mathf.Max(agentParameters.numberOfActionsBetweenDecisions, 1);
        if (!agentParameters.onDemandDecision && resetStepsElapsed > resetWaitSteps)
        {
            RequestAction();
            if (academyStepCounter %
                agentParameters.numberOfActionsBetweenDecisions == 0)
                RequestDecision();
        }
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        if (resetStepsElapsed < resetWaitSteps)
            return;
        steps++;

        List<float> actions = vectorAction.ToList();
        List<float> actionsClamped = new List<float>();
        foreach (var var in actions)
            actionsClamped.Add(Mathf.Clamp(var, -1f, 1f));

        this.actions.applyActions(actionsClamped);
        SetReward(standingReward.getReward());
        if (getTerminated(stepCount))
        {
            Done();
            SetReward(-1);
            steps = 0;
        }
    }

    public override void AgentReset()
    {
        resetPos.ResetPosition();
        resetStepsElapsed = 0;
        velReset = false;
        decisionFrequency = (decisionFrequency + 1 > 10) ? 1 : decisionFrequency + 1;
        agentParameters.numberOfActionsBetweenDecisions = decisionFrequency;
        observations.decisionFrequency = decisionFrequency;
    }

    private bool getTerminated(int step)
    {
        var terminated = standingReward.terminated(step);
        if (terminated)
        {
            startTime = epoch.getEpoch();
            episodes++;
        }

        return terminated;
    }
}