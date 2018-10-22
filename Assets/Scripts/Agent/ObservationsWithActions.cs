using System.Collections.Generic;
using System.Linq;

public class ObservationsWithActions : Observations
{
    private List<float> lastActions;
    private ActionsAngPosStrength ActionsAngPos;
    
    protected override void Start()
    {
        ActionsAngPos = GetComponent<ActionsAngPosStrength>();
        lastActions = new List<float>();
        for (int i = 0; i < ActionsAngPos.actionsSpace; i++)
        {
            lastActions.Add(0f);
        }
        
        base.Start();
    }

    public override List<float> getObservations()
    {
        getObservationsNamed();
        List<float> observations = observationsNamed.Select(kv => kv.Value).ToList();
        observations.AddRange(lastActions);
        return observations;
    }

    public void setLastActions(List<float> lastActions)
    {
        this.lastActions = lastActions;
    }
}