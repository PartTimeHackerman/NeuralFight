using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Agent;
using UnityEngine;

internal class Humanoid2DDuelAgent : Agent
{
    private ApplicationSettings applicationSettings;
    private IActions actions;

    public int episodes;
    private readonly Epoch epoch = Epoch.get();

    private Humanoid2DDuelObservations observations;
    private PausePos pausePos;
    private Humanoid2DResetPos resetPos;
    private DuelReward duelReward;
    private DuelEnemyObservations enemyObservations;
    public TerminateDuel terminateDuel;
    private long startTime;

    public int resetWaitSteps = 5;
    private int resetStepsElapsed = 0;
    private bool velReset = false;
    public int steps = 0;

    public bool playerOne = true;
    public Humanoid2DDuelAgent enemyAgent;

    public override void InitializeAgent()
    {
        observations = GetComponent<Humanoid2DDuelObservations>();
        duelReward = GetComponent<DuelReward>();
        actions = GetComponent<Humanoid2DActionsAngPos>();
        resetPos = GetComponent<Humanoid2DResetPos>();
        pausePos = GetComponent<PausePos>();
        enemyObservations = GetComponent<DuelEnemyObservations>();
        terminateDuel = GetComponent<TerminateDuel>();
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
        if (resetStepsElapsed > resetWaitSteps && !velReset)
        {
            resetPos.resetVel();
            resetPos.resetJointForces();
            resetPos.resetJointPositions();
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
        SetReward(duelReward.getReward());

        bool terminateSelf = terminateDuel.isTerminated();
        bool terminateEnemy = enemyAgent.terminateDuel.isTerminated();
        bool terminate = terminateSelf || terminateEnemy;

        float reward = 0f;

        if (terminateSelf)
            reward = -1f;

        if (terminateEnemy)
            reward = 1f;

        if (terminateSelf && terminateEnemy)
            reward = 0;

        if (terminate)
        {
            Done();
            SetReward(reward);
            steps = 0;
        }
    }

    public override void AgentReset()
    {

        //decisionFrequency = (decisionFrequency + 1 > 10) ? 5 : decisionFrequency + 1;
        //agentParameters.numberOfActionsBetweenDecisions = Random.Range(5,10);
        //observations.decisionFrequency = decisionFrequency;
        resetPos.ResetPosition();
        resetStepsElapsed = 0;
        velReset = false;
    }
   
}