public class WeaponModificationType : ModificationType
{
    public static readonly ModificationType BONUS_ATTACK = new ModificationType("Bonus attack");
    public static readonly ModificationType BONUS_SP_REQ = new ModificationType("Bonus SP REQ.");
    
    public WeaponModificationType(string name) : base(name)
    {
    }
}