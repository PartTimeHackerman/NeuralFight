public class HpBar : ValBar
{
    protected override void Start()
    {
        MaxVal.text = ThousandsConverter.ToKs(Player.MaxHP);
        CurrVal.text = ThousandsConverter.ToKs(Player.MaxHP);
        Player.OnChangeHealth += SetVals;
        base.Start();
    }
}