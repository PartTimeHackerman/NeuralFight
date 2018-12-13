using System.Collections;
using System.Text.RegularExpressions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ArenaUIAnimations : MonoBehaviour
{
    public Button StartButton;
    public Text RoundText;
    public Text RoundEndText;
    public Text RoundEndPlayerNameText;
    public Text MatchEndText;
    public Text MatchEndPlayerNameText;
    public Text RoundNumber;
    public Text Timer;

    private bool CountTimer = false;
    private int CurrentTime = 0;


    private void Start()
    {
        StartCoroutine(TimeSetter());
    }

    public void SetUp()
    {
        StartButton.transform.DOLocalMove(new Vector3(0, -300, 0), 2).From().SetEase(Ease.OutQuart);
    }

    public void StartArena()
    {
        StartButton.transform.DOLocalMove(new Vector3(0, 300, 0), 2).SetEase(Ease.OutQuart);
    }

    public void StartRound(int round)
    {
        RoundNumber.DOText(round.ToString(), 1, false);
        
        RoundText.transform.localPosition = new Vector3(-550, 0, 0);
        RoundText.text = "Round " + round;

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(RoundText.transform.DOLocalMove(Vector3.zero, 1).SetEase(Ease.OutQuart))
            .Append(RoundText.transform.DOLocalMove(new Vector3(550, 0, 0), 1).SetEase(Ease.InQuart));
        //RoundText.transform.DOLocalMove(Vector3.zero, 2).SetEase(Ease.OutQuart);
        //RoundText.transform.DOLocalMove(new Vector3(550, 0,0), 2).SetEase(Ease.OutQuart);
    }

    public void EndRound(int round, string winnerName)
    {
        string endText = "Round $n$ ended\n\nwins";
        string nameText = "\n$p$\n";
        RoundEndText.text = endText.Replace("$n$", round.ToString());
        RoundEndPlayerNameText.text = nameText.Replace("$p$", winnerName);
        RoundEndText.transform.localPosition = new Vector3(-650, 0, 0);

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(RoundEndText.transform.DOLocalMove(Vector3.zero, 1).SetEase(Ease.OutQuart))
            .Append(RoundEndText.transform.DOLocalMove(new Vector3(650, 0, 0), 1).SetEase(Ease.InQuart));
        //RoundText.transform.DOLocalMove(Vector3.zero, 2).SetEase(Ease.OutQuart);
        //RoundText.transform.DOLocalMove(new Vector3(550, 0,0), 2).SetEase(Ease.OutQuart);
    }

    public void EndMatch(string winnerName)
    {
        string endText = "Match ended\n\nwins";
        string nameText = "\n$p$\n";
        MatchEndPlayerNameText.text = nameText.Replace("$p$", winnerName);
        MatchEndText.transform.localPosition = new Vector3(-650, 0, 0);

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(MatchEndText.transform.DOLocalMove(Vector3.zero, 1).SetEase(Ease.OutQuart))
            .Append(MatchEndText.transform.DOLocalMove(new Vector3(650, 0, 0), 1).SetEase(Ease.InQuart));
        //RoundText.transform.DOLocalMove(Vector3.zero, 2).SetEase(Ease.OutQuart);
        //RoundText.transform.DOLocalMove(new Vector3(550, 0,0), 2).SetEase(Ease.OutQuart);
    }

    public void EnableTimer(bool enable)
    {
        CurrentTime = enable ? 0 : CurrentTime;
        CountTimer = enable;
    }

    public IEnumerator TimeSetter()
    {
        while (true)
        {
            if (CountTimer)
            {
                CurrentTime++;
                Timer.transform.DOPunchScale(new Vector3(.2f,.2f,.2f), .5f, 1, .1f);
                Timer.DOText(CurrentTime.ToString(), .2f, false);
            }

            yield return new WaitForSeconds(1f);
        }
    }
}