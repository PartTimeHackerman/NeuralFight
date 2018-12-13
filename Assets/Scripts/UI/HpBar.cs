using UnityEngine;

public class HpBar : ValBar
{
    public void SetPlayer(Player player)
    {
        if (Player != null)
        {
            Player.OnChangeHealth -= SetVals;
        }

        Player = player;
        SetVals(Player.HP, player.MaxHP);
        Player.OnChangeHealth += SetVals;
        Vector2 barSize = LerpBar.rectTransform.sizeDelta;
        barSize.x = NewCurrVal;
        LerpBar.rectTransform.sizeDelta = barSize;
    }
}