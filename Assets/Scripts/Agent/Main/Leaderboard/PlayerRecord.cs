using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRecord : MonoBehaviour
{
    public long Rank;
    public string PlayerName;
    public string PlayerID;
    public long Score;
    public UserPlayer UserPlayer;

    public Image RecordImage;
    public Image PlaceImage;
    public Text PlaceText;
    public Image NameImage;
    public Text NameText;
    public Image ScoreImage;
    public Text ScoreText;

    public Button AttackButton;

    
    
    private void Awake()
    {
        AttackButton.onClick.AddListener(() => FightPlayer());
    }

    public void SetPlayer(UserPlayer userPlayer)
    {
        UserPlayer = userPlayer;
        SetPlayer(UserPlayer.Rank, UserPlayer.UserName, UserPlayer.PlayerID, UserPlayer.Score);
    }

    public void SetPlayer(long rank, string playerName, string playerId, long score)
    {
        Rank = rank;
        PlayerName = playerName;
        PlayerID = playerId;
        Score = score;

        PlaceText.text = Rank.ToString();
        NameText.text = PlayerName;
        ScoreText.text = Score.ToString();

        switch (Rank)
        {
            case 1:
                PlaceImage.color = new Color(1f, 0.97f, 0f);
                break;
            case 2:
                PlaceImage.color = new Color(0.8f, 0.8f, 0.8f);
                break;
            case 3:
                PlaceImage.color = new Color(1f, 0.67f, 0.2f);
                break;
        }

        if (PlayerID.Equals(Auth.UserId))
        {
            RecordImage.color = new Color(1f, 1f, 1f, 0.44f);
            AttackButton.GetComponentInChildren<Text>().text = "You";
            AttackButton.GetComponent<Image>().color = Color.green;
        }
        else
        {
            RecordImage.color = new Color(1f, 1f, 1f, 0.11f);
            AttackButton.GetComponentInChildren<Text>().text = "Attack";
            AttackButton.GetComponent<Image>().color = new Color(1f, 0.3f, 0.31f);
        }
    }

    private async Task FightPlayer()
    {
        await EnemyCollections.Get().LoadEnemy(UserPlayer);
    }
}