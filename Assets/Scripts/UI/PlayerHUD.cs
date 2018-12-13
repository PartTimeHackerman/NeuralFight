
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public Text Name;
    public HpBar HpBar;
    public SpBar SpBar;

    public Image Star1;
    public Image Star2;
    public Image Star3;
    
    public void SetFighter(string name, Fighter fighter)
    {
        Name.text = name;
        HpBar.SetPlayer(fighter.Player);
        SpBar.SetPlayer(fighter.Player);
    }

    public void ResetWins()
    {
        Star1.enabled = false;
        Star2.enabled = false;
        Star3.enabled = false;
    }
}