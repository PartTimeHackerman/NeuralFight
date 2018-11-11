using System;
using System.Collections.Generic;
using UnityEngine;

public class LocomotionAction : MonoBehaviour
{
    public int decFreq = 5;
    public int step = 0;
    public LocomotyionType LocomotyionType;
    private InternalModel model;
    public bool run = true;
    public Observations Observations;
    public ActionsAngPosStrength ActionsAngPosStrength;
    private VerticalEffector verticalEffector;

    private List<float> observations;
    
    protected virtual void Start()
    {
        SetUpModel();
        verticalEffector = GetComponent<VerticalEffector>();
    }
    
    protected virtual void FixedUpdate()
    {
        verticalEffector.enable = run;
        if (run)
        {
            if (step >= decFreq)
            {
                observations = Observations.getObservations();
                RunModel();
                step = 0;
            }
            step++;
        }
    }

    protected virtual  void RunModel()
    {
        //List<float> observations = Observations.getObservations();
        List<float> actions = model.getActions(observations);
        ActionsAngPosStrength.applyActions(actions);

    }

    protected virtual void SetUpModel()
    {
        string modelPath = "Models/";
        switch (LocomotyionType)
        {
            case LocomotyionType.STAND:
                modelPath += "Stand";
                break;
            case LocomotyionType.CROUCH:
                modelPath += "Crouch";
                break;
            case LocomotyionType.WALK_FORWARD:
                modelPath += "Walk_Forward";
                break;
            case LocomotyionType.WALK_BACKWARD:
                modelPath += "Walk_Backward";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        model = new InternalModel(modelPath, Observations, ActionsAngPosStrength);
    }
}