using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Agent;
using MLAgents;
using UnityEngine;

internal class Humanoid2DDuelAgent : Agent
{
    private ApplicationSettings applicationSettings;
    private IActions actions;

    public int episodes;
    private readonly Epoch epoch = Epoch.get();

    private Humanoid2DDuelObservations observations;
    private PausePos pausePos;
    private ResetPos resetPos;
    private DuelReward duelReward;
    private DuelEnemyObservations enemyObservations;
    public Terminator Terminator;
    private long startTime;

    public int resetWaitSteps = 10;
    private int resetStepsElapsed = 0;
    public int steps = 0;

    public bool playerOne = true;
    public Humanoid2DDuelAgent enemyAgent;

    public float sumRewards = 0f;
    private bool terminateAgent = false;
    public bool ready = true;
    public bool terminateSelf = false;

    public override void InitializeAgent()
    {
        observations = GetComponent<Humanoid2DDuelObservations>();
        duelReward = GetComponent<DuelReward>();
        actions = GetComponent<ActionsAngPos>();
        resetPos = GetComponent<ResetPos>();
        pausePos = GetComponent<PausePos>();
        enemyObservations = GetComponent<DuelEnemyObservations>();
        Terminator = GetComponent<Terminator>();
        observations.decisionFrequency = agentParameters.numberOfActionsBetweenDecisions;

        enemyObservations.setPlayerOne(playerOne);
        observations.setPlayerOne(playerOne);
    }

    public override void CollectObservations()
    {

        List<float> observations = this.observations.getObservations();
        List<float> enemyObservations = this.enemyObservations.getEnemyObservations();

        foreach (var observation in observations) AddVectorObs(observation);
        foreach (var observation in enemyObservations) AddVectorObs(observation);
    }

    protected override void MakeRequests(int academyStepCounter)
    {
        agentParameters.numberOfActionsBetweenDecisions =
            Mathf.Max(agentParameters.numberOfActionsBetweenDecisions, 1);
        if (!agentParameters.onDemandDecision && ready)
        {
            RequestAction();
            if (academyStepCounter %
                agentParameters.numberOfActionsBetweenDecisions == 0)
                RequestDecision();
        }
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        /*if (!ready)
        {
            return;
        }*/

        steps++;

        List<float> actions = vectorAction.ToList();
        List<float> actionsClamped = new List<float>();
        foreach (var var in actions)
            actionsClamped.Add(Mathf.Clamp(var, -1f, 1f));

        
        this.actions.applyActions(actionsClamped);
        float reward = duelReward.getReward();
        sumRewards += reward;
        SetReward(reward);

        terminateSelf = false;
        terminateSelf = Terminator.isTerminated();
        bool terminateEnemy = enemyAgent.terminateSelf;
        terminateAgent = terminateSelf || terminateEnemy;

        float terminateReward = 0f;

        if (terminateSelf)
            terminateReward = -sumRewards;

        if (terminateEnemy)
            terminateReward = sumRewards;

        if (terminateSelf && terminateEnemy)
            terminateReward = 0;

        terminateReward /= 10f;

        if (terminateAgent)
        {
            SetReward(terminateReward);
            sumRewards = 0f;
            steps = 0;
            terminateAgent = false;
            resetStepsElapsed = 0;
            ready = false;
            resetPos.ResetPosition();
            ready = true;
            Done();
        }
    }

    public override void AgentReset()
    {
        
    }
   
}