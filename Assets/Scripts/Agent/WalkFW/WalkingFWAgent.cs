using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Agent;
using UnityEngine;

internal class WalkingFWAgent : Agent
{
    private ApplicationSettings applicationSettings;
    private IActions actions;
    
    private WalkFWObservations observations;
    private Humanoid2DResetPos resetPos;
    private WalkFWReward rewards;
    private TerminateDuel terminateFn;
    
    public int steps = 0;
    public int maxSteps = 100;
    
    public bool ready = true;
    private float rewardAnim = 0f;
    public float sumRewards = 0f;

    public override void InitializeAgent()
    {
        observations = GetComponent<WalkFWObservations>();
        rewards = GetComponent<WalkFWReward>();
        actions = GetComponent<Humanoid2DActionsAngPos>();
        resetPos = GetComponent<Humanoid2DResetPos>();
        terminateFn = GetComponent<TerminateDuel>();
        
        observations.decisionFrequency = agentParameters.numberOfActionsBetweenDecisions;
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
        rewardAnim = rewards.getReward();

        bool terminateAgent = terminateFn.isTerminated();

        //sumRewards += rewardAnim;
        SetReward(rewardAnim);

        if (steps > maxSteps || terminateAgent)
        {
            //sumRewards = sumRewards > 0 ? sumRewards : 0;
            SetReward(rewardAnim);
            //sumRewards = 0f;
            steps = 0;
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