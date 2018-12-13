using System.Collections.Generic;
using System.Linq;

public class NothingLocomoctionAction : LocomotionAction
{

    public BodyParts BodyParts;
    private List<BodyPart> Parts;

    private bool Enabled = true;
    
    protected override void Start()
    {
        Parts = BodyParts.namedObservableBodyParts.Values.ToList();
    }

    protected override void FixedUpdate()
    {
        if (run)
        {
            if (Enabled)
            {
                DisableJoints();
                Enabled = false;
            }
        }
        else
        {
            if (!Enabled)
            {
                EnableJoints();
                Enabled = true;
            }
        }
    }

    private void DisableJoints()
    {
        foreach (BodyPart bodyPart in Parts)
        {
            if (bodyPart.partEnabled)
            {
                if (bodyPart.JointInfo.isEnabled) bodyPart.JointInfo.Disable();
            }
        }
    }
    
    private void EnableJoints()
    {
        foreach (BodyPart bodyPart in Parts)
        {
            if (bodyPart.partEnabled)
            {
                if (!bodyPart.JointInfo.isEnabled) bodyPart.JointInfo.Enable();
            }
        }
    }
}