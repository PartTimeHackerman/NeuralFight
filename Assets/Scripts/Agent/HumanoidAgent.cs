using System.Collections.Generic;
using System.Linq;
using MLAgents;
using UnityEngine;

internal class HumanoidAgent : Agent
{
    private ApplicationSettings applicationSettings;
    private IActions actions;

    public int episodes;
    private readonly Epoch epoch = Epoch.get();

    private Observations observations;
    private PausePos pausePos;
    private ResetPosOLD _resetPosOld;
    private StandingRewardOld _standingRewardOld;
    private long startTime;

    public int resetWaitSteps = 5;
    private int resetStepsElapsed = 0;
    private bool velReset = false;
    public int steps = 0;
    public int maxSteps = 0;

    public override void InitializeAgent()
    {
        observations = GetComponent<Observations>();
        _standingRewardOld = GetComponent<StandingRewardOld>();
        actions = GetComponent<ActionsAngPosOLD>();
        _resetPosOld = GetComponent<ResetPosOLD>();
        pausePos = GetComponent<PausePos>();
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
            _resetPosOld.resetVel();
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
        if (steps > maxSteps)
            maxSteps = steps;

        List<float> actions = vectorAction.ToList();
        List<float> actionsClamped = new List<float>();
        foreach (var var in actions)
            actionsClamped.Add(Mathf.Clamp(var, -1f, 1f));

        this.actions.applyActions(actionsClamped);

        SetReward(_standingRewardOld.getReward());

        if (getTerminated(stepCount))
        {
            Done();
            SetReward(-1);
            steps = 0;
        }
    }

    public override void AgentReset()
    {
        _resetPosOld.ResetPosition();
        resetStepsElapsed = 0;
        velReset = false;
    }

    private bool getTerminated(int step)
    {
        var terminated = _standingRewardOld.terminated(step);
        if (terminated)
        {
            startTime = epoch.getEpoch();
            episodes++;
        }

        return terminated;
    }
}