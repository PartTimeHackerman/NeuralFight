using System.Collections.Generic;
using System.Linq;
using UnityEngine;

internal class HumanoidSAgent : Agent
{
    private readonly float actionMultipler = 1000;
    private ApplicationSettings applicationSettings;
    private ApplyActionsSmall _applyActionsSmall;

    public int episodes;
    private readonly Epoch epoch = Epoch.get();

    private Observations observations;
    private PausePos pausePos;
    private ResetPos resetPos;
    private StandingReward standingReward;
    private long startTime;

    public override void InitializeAgent()
    {
        observations = GetComponent<ObservationsSmallXY>();
        standingReward = GetComponent<StandingReward>();
        _applyActionsSmall = GetComponent<ApplyActionsSmall>();
        resetPos = GetComponent<ResetPos>();
        pausePos = GetComponent<PausePos>();
    }

    public override void CollectObservations()
    {
        List<float> observations = this.observations.getObservations();
        foreach (var observation in observations) AddVectorObs(observation);
        SetTextObs("Testing " + gameObject.GetInstanceID());
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        List<float> actions = vectorAction.ToList();
        List<float> actionsMultiplied = new List<float>();
        foreach (var var in actions)
            actionsMultiplied.Add(Mathf.Clamp(var * actionMultipler, 0-actionMultipler, actionMultipler));

        _applyActionsSmall.applyActions(actionsMultiplied);

        SetReward(standingReward.getReward());

        if (getTerminated())
        {
            Done();
            SetReward(0);
        }
    }

    public override void AgentReset()
    {
        resetPos.ResetPosition();
    }

    private bool getTerminated()
    {
        var terminated = standingReward.terminated();
        if (terminated)
        {
            startTime = epoch.getEpoch();
            episodes++;
        }

        return terminated;
    }
}