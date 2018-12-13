public class BodyPartModificationType : ModificationType
{
    public static readonly ModificationType BONUS_HP = new ModificationType("Bonus HP");
    public static readonly ModificationType BONUS_SP = new ModificationType("Bonus SP");
    public static readonly ModificationType BONUS_HP_REGEN = new ModificationType("Bonus HP Regen.");
    public static readonly ModificationType BONUS_SP_REGEN = new ModificationType("Bonus SP Regen.");
    
    public BodyPartModificationType(string name) : base(name)
    {
    }
}