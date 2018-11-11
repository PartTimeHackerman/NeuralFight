public class MovableLocomotionAction : LocomotionAction
{
    private VelocityEffector VelocityEffector;

    protected override void Start()
    {
        base.Start();
        VelocityEffector = GetComponent<VelocityEffector>();
    }

    protected override void FixedUpdate()
    {
        VelocityEffector.enable = run;
        base.FixedUpdate();
    }
}