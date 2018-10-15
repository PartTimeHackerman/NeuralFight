using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Agent;
using MLAgents;
using UnityEngine;

internal class PunchAgent : Agent
{
    private IActions actions;
    private PunchObservations observations;
    private ResetPos resetPos;
    private PunchReward rewards;
    private PunchTerminator terminateFn;
    private PunchPointSpawner punchPointSpawner;

    private Vector3 pointPos;
    
    public int steps = 0;
    public int maxSteps = 100;
    
    public bool ready = true;
    private float agentReward = 0f;
    public float sumRewards = 0f;

    public override void InitializeAgent()
    {
        observations = GetComponent<PunchObservations>();
        rewards = GetComponent<PunchReward>();
        actions = GetComponent<ActionsAngPos>();
        resetPos = GetComponent<ResetPos>();
        terminateFn = GetComponent<PunchTerminator>();
        punchPointSpawner = GetComponent<PunchPointSpawner>();
        pointPos = punchPointSpawner.spawnRandom();
        observations.punchTarget = punchPointSpawner.point.transform;
        observations.addToRemove(new[]{"root_pos_x", "root_pos_y"});
        
    }

    public override void CollectObservations()
    {

        List<float> observations = this.observations.getObservations();

        foreach (var observation in observations) AddVectorObs(observation);
    }

    protected override void MakeRequests(int academyStepCounter)
    {
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

        steps++;
        List<float> actions = vectorAction.ToList();
        List<float> actionsClamped = new List<float>();
        foreach (var var in actions)
            actionsClamped.Add(Mathf.Clamp(var, -1f, 1f));

        this.actions.applyActions(actionsClamped);

        agentReward = rewards.getReward(pointPos);
        bool terminateAgent = terminateFn.isTerminated(pointPos);

        SetReward(agentReward);

        if (steps > maxSteps || terminateAgent)
        {
            SetReward(agentReward);
            if (terminateAgent)
            {
                SetReward(agentReward * 10f);
            }
            steps = 0;
            ready = false;
            resetPos.ResetPosition();
            pointPos = punchPointSpawner.spawnRandom();
            ready = true;
            Done();
        }
    }

    public override void AgentReset()
    {
        
    }


   
}