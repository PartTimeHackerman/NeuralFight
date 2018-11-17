public class SpBar : ValBar
{
    protected override void Start()
    {
        MaxVal.text = ThousandsConverter.ToKs(Player.MaxSP);
        CurrVal.text = ThousandsConverter.ToKs(Player.MaxSP);
        Player.OnChangeStamina += SetVals;
        base.Start();
    }
}