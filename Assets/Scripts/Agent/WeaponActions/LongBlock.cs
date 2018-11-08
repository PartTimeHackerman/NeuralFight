public class LongBlock : Block
{
    public override void setUpAction(BodyParts bodyParts)
    {
        upperArmAng = -45f;
        lowerArmAng = 70f;
        base.setUpAction(bodyParts);
    }
}