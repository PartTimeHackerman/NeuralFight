using System.Collections.Generic;
using UnityEngine;

public class StandingAgentSmall : MonoBehaviour, IAgent
{
    private readonly float actionMultipler = 100;


    public int actionsTaken;
    private ApplicationSettings applicationSettings;
    private ApplyActionsSmall _applyActionsSmall;
    public int episodes;
    private readonly Epoch epoch = Epoch.get();


    public int hash;
    public float highestReward;

    private Observations observations;
    private PausePos pausePos;
    private ResetPos resetPos;
    private StandingReward standingReward;
    private long startTime;
    public float timeReward;
    private IEnvironment environment { get; set; }

    public ObservationsDTO getObservations()
    {
        var observations = new ObservationsDTO();
        observations.hash = getHash();
        var reward = standingReward.getReward();
        var terminated = getTerminated();
        observations.observations = this.observations.getObservations();
        observations.reward = reward;
        observations.terminated = terminated;
        return observations;
    }

    public void receiveActions(ActionsDTO actionsDTO)
    {
        List<float> actions = actionsDTO.actions;
        List<float> actionsMultiplied = new List<float>();
        foreach (var var in actions)
            actionsMultiplied.Add(var * actionMultipler);

        _applyActionsSmall.applyActions(actionsMultiplied);
        actionsTaken++;
    }

    public int getHash()
    {
        return hash;
    }

    public void pauseAgent(bool pause)
    {
        if (pause)
            pausePos.Pause();
        else
            pausePos.Continue();
    }

    private void Start()
    {
        hash = GetHashCode();
        observations = GetComponent<ObservationsSmallXY>();
        standingReward = GetComponent<StandingReward>();
        _applyActionsSmall = GetComponent<ApplyActionsSmall>();
        resetPos = GetComponent<ResetPos>();
        pausePos = GetComponent<PausePos>();


        //environment = InternalEnv.get(this);
    }

    public float getTimeReward()
    {
        highestReward = timeReward > highestReward ? timeReward : highestReward;
        timeReward = epoch.getEpoch() - startTime;
        timeReward *= applicationSettings.timeScale;
        return timeReward;
    }

    private bool getTerminated()
    {
        var terminated = standingReward.terminated();
        if (terminated)
        {
            startTime = epoch.getEpoch();
            resetPos.ResetPosition();
            episodes++;
        }

        return terminated;
    }
}