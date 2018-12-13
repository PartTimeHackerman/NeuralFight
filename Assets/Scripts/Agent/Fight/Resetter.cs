using UnityEngine;

public class Resetter : MonoBehaviour
{
    public float Elapsed;
    public Walls Walls;

    public FightPlayerAgent LeftAgent;
    public FightPlayerAgent RightAgent;

    public bool agentsTerminated;
    public bool AgentsTerminated
    {
        get { return agentsTerminated; }
        set
        {
            agentsTerminated = value;
            if (agentsTerminated)
            {
                ResetEnv();
            }
        }
    }

    private void FixedUpdate()
    {
        Elapsed = GameTimer.get().Elapsed;
        if (Elapsed > 60)
        {
            ResetEnv();
        }
    }

    public void ResetEnv()
    {
        LeftAgent.terminateAgent = true;
        RightAgent.terminateAgent = true;
        agentsTerminated = false;
        GameTimer.get().Elapsed = 0;
        Walls.ResetWalls();
        Walls.StartWalls();
    }
}