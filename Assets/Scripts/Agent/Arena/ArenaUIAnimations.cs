using System.Collections;
using System.Text.RegularExpressions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ArenaUIAnimations : MonoBehaviour
{
    public Button StartButton;
    public Button BackButton;
    public Text RoundText;
    public Text RoundEndText;
    public Text RoundEndPlayerNameText;
    public Text MatchEndText;
    public Text MatchEndPlayerNameText;
    public Text RoundNumber;
    public Text Timer;
    public ItemHighlighter ItemHighlighter;

    private bool CountTimer = false;
    private int CurrentTime = 0;


    private void Start()
    {
        StartCoroutine(TimeSetter());
    }

    public void SetUp()
    {
        StartButton.transform.DOLocalMove(new Vector3(0, 0, 0), 2).SetEase(Ease.OutQuart);
        RoundNumber.text = "0";
        Timer.text = "0";
    }

    public void StartArena()
    {
        StartButton.transform.DOLocalMove(new Vector3(0, 370, 0), 2).SetEase(Ease.OutQuart);
    }

    public void StartRound(int round)
    {
        RoundText.gameObject.SetActive(true);
        RoundNumber.DOText(round.ToString(), 1, false);

        RoundText.transform.localPosition = new Vector3(-550, 0, 0);
        RoundText.text = "Round " + round;

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(RoundText.transform.DOLocalMove(Vector3.zero, 1).SetEase(Ease.OutQuart))
            .Append(RoundText.transform.DOLocalMove(new Vector3(550, 0, 0), 1).SetEase(Ease.InQuart))
            .OnComplete(() => RoundText.gameObject.SetActive(false));
    }

    public void EndRound(int round, string winnerName)
    {
        RoundEndText.gameObject.SetActive(true);
        string endText = "Round $n$ ended\n\nwins";
        string nameText = "\n$p$\n";
        RoundEndText.text = endText.Replace("$n$", round.ToString());
        RoundEndPlayerNameText.text = nameText.Replace("$p$", winnerName);
        RoundEndText.transform.localPosition = new Vector3(-650, 0, 0);

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(RoundEndText.transform.DOLocalMove(Vector3.zero, 1).SetEase(Ease.OutQuart))
            .AppendInterval(1)
            .Append(RoundEndText.transform.DOLocalMove(new Vector3(650, 0, 0), 1).SetEase(Ease.InQuart))
            .OnComplete(() => RoundEndText.gameObject.SetActive(false));
    }

    public void EndMatch(string winnerName)
    {
        MatchEndText.gameObject.SetActive(true);
        string endText = "Match ended\n\nwins";
        string nameText = "\n$p$\n";
        MatchEndPlayerNameText.text = nameText.Replace("$p$", winnerName);
        MatchEndText.transform.localPosition = new Vector3(-650, 0, 0);

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(MatchEndText.transform.DOLocalMove(Vector3.zero, 1).SetEase(Ease.OutQuart))
            .AppendInterval(1)
            .Append(MatchEndText.transform.DOLocalMove(new Vector3(650, 0, 0), 1).SetEase(Ease.InQuart))
            .OnComplete(() =>
                {
                    MatchEndText.gameObject.SetActive(false);
                }
            );
    }

    public void ReceiveItem(Item item)
    {
        
        ItemHighlighter.gameObject.SetActive(true);
        ItemHighlighter.transform.localPosition = new Vector3(-5f, 0, 0);
        ItemHighlighter.ScrollListItem.SetItem(item);

        Sequence mySequence = DOTween.Sequence();
        mySequence
            .PrependInterval(3)
            .OnStart((() =>
            {
                //Vector3 pos = ItemHighlighter.Canvas.transform.position;
                //pos.x = Camera.main.transform.position.x;
                //ItemHighlighter.Canvas.transform.position = pos;
            }))
            .Append(ItemHighlighter.transform.DOLocalMove(Vector3.zero, 1).SetEase(Ease.OutQuart))
            .AppendInterval(1)
            .Append(ItemHighlighter.transform.DOLocalMove(new Vector3(5f, 0, 0), 1).SetEase(Ease.InQuart))
            .OnComplete(() =>
                {
                    ItemHighlighter.gameObject.SetActive(false);
                    if (item.GetType() == typeof(Weapon))
                    {
                        PlayerCollections.Get().PlayerWeaponsCollection.AddToScrollList((Weapon) item);
                    }
                    else
                    {
                        PlayerCollections.Get().PlayerFighterPartsCollection.AddToScrollList((FighterPart) item);
                    }

                    BackButton.transform.DOLocalMove(new Vector3(0, -200, 0), 2).SetEase(Ease.OutQuart);
                    
                }
            );
    }

    public void ExitArena()
    {
        BackButton.transform.DOLocalMove(new Vector3(0, -370, 0), 2).SetEase(Ease.OutQuart);
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
                Timer.transform.DOPunchScale(new Vector3(.2f, .2f, .2f), .5f, 1, .1f);
                Timer.DOText(CurrentTime.ToString(), .2f, false);
            }

            yield return new WaitForSeconds(1f);
        }
    }
}