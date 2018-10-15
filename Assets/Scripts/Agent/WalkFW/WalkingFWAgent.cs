﻿using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Agent;
using MLAgents;
using UnityEngine;

internal class WalkingFWAgent : Agent
{
    private ApplicationSettings applicationSettings;
    private IActions actions;
    
    private Observations observations;
    private ResetPos resetPos;
    private WalkFWReward rewards;
    
    public int steps = 0;
    public int maxSteps = 100;
    
    private float agentReward = 0f;

    public override void InitializeAgent()
    {
        observations = GetComponent<Observations>();
        rewards = GetComponent<WalkFWReward>();
        actions = GetComponent<ActionsAngPos>();
        resetPos = GetComponent<ResetPos>();
        observations.addToRemove(new[]{"root_pos_x"});

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
        if (!agentParameters.onDemandDecision)
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
        agentReward = rewards.getReward();
        
        SetReward(agentReward);

        if (steps > maxSteps)
        {
            steps = 0;
            resetPos.ResetPosition();
            Done();
        }
    }

    public override void AgentReset()
    {
        
    }


   
}