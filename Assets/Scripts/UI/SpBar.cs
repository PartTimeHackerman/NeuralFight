using UnityEngine;

public class SpBar : ValBar
{
    
    public void SetPlayer(Player player)
    {
        if (Player != null)
        {
            Player.OnChangeStamina -= SetVals;
        }

        Player = player;
        SetVals(Player.SP, Player.MaxSP);
        Vector2 barSize = LerpBar.rectTransform.sizeDelta;
        barSize.x = NewCurrVal;
        LerpBar.rectTransform.sizeDelta = barSize;
        Player.OnChangeStamina += SetVals;
    }
}