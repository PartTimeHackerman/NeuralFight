internal class BodyPartsSmallXY : BodyParts
{
    private void Awake()
    {
        base.Awake();
        parts.Remove(namedParts["lfoot"]);
        parts.Remove(namedParts["rfoot"]);
    }
}