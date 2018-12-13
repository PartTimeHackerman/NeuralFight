using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Agent;
using MLAgents;
using UnityEditor;
using UnityEngine;

public class FightPlayerAgent : Agent
{
    public Resetter Resetter;
    public Player Player;
    public Fighter PlayerFighter;
    private Fighter EnemyFighter;
    private FightPlayerActions actions;

    private FightPlayerObservations observations;
    public ResetPos resetPos;
    private FightPlayerReward duelReward;
    private FightPlayerObservations enemyObservations;
    private FightObservationsForEnemy observationsForEnemy;
    public RandomWeaponEquipper RandomWeaponEquipper;
    private long startTime;

    public int resetWaitSteps = 10;
    private int resetStepsElapsed = 0;
    public int steps = 0;

    public bool playerOne = true;
    public FightPlayerAgent enemyAgent;

    public float sumRewards = 0f;
    public bool terminateAgent = false;
    public bool ready = true;
    public bool terminateSelf = false;
    private bool newDecisionStep = false;

    public override void InitializeAgent()
    {
        observations = GetComponent<FightPlayerObservations>();
        observationsForEnemy = GetComponent<FightObservationsForEnemy>();
        duelReward = GetComponent<FightPlayerReward>();
        actions = GetComponent<FightPlayerActions>();
        
        enemyObservations = enemyAgent.observations;
        
        observations.SetUp(this, enemyAgent);
        observationsForEnemy.SetUp(this);
        
        duelReward.SetUp(this, enemyAgent);
        StartCoroutine(Waiter.WaitForFrames(2, () => { }, () =>
        {
            actions.SetUp(this,enemyAgent);
            EnemyFighter = enemyAgent.PlayerFighter;
            RandomWeaponEquipper.Equip(PlayerFighter); 
            observations.observationsSpace = observations.GetObservations(playerOne).Count;
            PlayerFighter.SetSide(PlayerFighter.Left);
            PlayerFighter.SetEnemy(EnemyFighter);
            PlayerFighter.StartFight();
            PlayerFighter.FightAction.run = false;
        }));
    }

    public override void CollectObservations()
    {
        List<float> observations = this.observations.GetObservations(playerOne);

        foreach (var observation in observations) AddVectorObs(observation);
        //foreach (var observation in enemyObservations) AddVectorObs(observation);
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
                newDecisionStep = true;
            }
        }
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        /*if (!ready)
        {
            return;
        }*/

        steps++;
        if (newDecisionStep)
        {
            List<float> actions = vectorAction.ToList();
            List<float> actionsClamped = new List<float>();
            foreach (var var in actions)
                actionsClamped.Add(Mathf.Clamp(var, -1f, 1f));

            this.actions.setActions(actionsClamped);
            newDecisionStep = false;
        }

        float reward = duelReward.getReward();

        terminateSelf = Player.died;
        Resetter.AgentsTerminated = terminateSelf;
        //bool terminateEnemy = enemyAgent.terminateSelf;
        //terminateAgent = terminateSelf || terminateEnemy;

        AddReward(reward);
        if (terminateAgent)
        {
            steps = 0;
            terminateAgent = false;
            resetStepsElapsed = 0;
            ready = false;
            Player.ResetPlayer();
            resetPos.ResetPosition();
            RandomWeaponEquipper.Equip(PlayerFighter);
            ready = true;
            Done();
        }
    }

    public override void AgentReset()
    {
    }
}