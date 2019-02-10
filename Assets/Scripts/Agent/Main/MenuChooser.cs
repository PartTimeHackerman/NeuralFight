using System.Linq;
using DG.Tweening;
using UnityEngine;

public class MenuChooser : MonoBehaviour
{
    public CurrentMenu CurrentMenu = CurrentMenu.NONE;
    public MainUI MainUI;
    public Transform CameraTransform;
    public MainMenu MainMenu;
    public EditorMenu Editor;
    public Arena Arena;
    public float MenuTransitionSpeed = 1f;

    private bool CanGoToTreadmill = false;
    private bool firstTime = true;

    private void Start()
    {
        MainUI.MainButton.onClick.AddListener(SetMain);
        MainUI.EditorButton.onClick.AddListener(SetEditor);
        //MainUI.ArenaButton.onClick.AddListener(SetArena);
        Waiter.Get().WaitForSecondsC(1, () => { }, SetMain);
        PlayerCollections.OnCreated += () =>
        {
            CanGoToTreadmill = true;
            SetTreadmillFighter();
        };

        EnemyCollections.OnCreated += enemy =>
        {
            SetArena(PlayerCollections.UserPlayer, enemy);
        };

        Arena.OnArenaExit += () =>
        {
            SetMain();
        };
    }

    private void SetMain()
    {
        if (CurrentMenu == CurrentMenu.MAIN) return;
        CurrentMenu = CurrentMenu.MAIN;
        UnSetArena();
        UnSetEditor();
        if (CanGoToTreadmill)
        {
            SetTreadmillFighter();
        }

        CameraTransform.DOKill();
        MainMenu.Leaderboard.GetLeaderboard();
        MainMenu.RankTransform.DOLocalMove(ObjectsPositions.UIMainRankPos, MenuTransitionSpeed).SetEase(Ease.InQuad);
        CameraTransform.DOMove(ObjectsPositions.CamMainPos, MenuTransitionSpeed).SetEase(Ease.OutQuad);
    }

    private void SetTreadmillFighter()
    {
        MainMenu.Treadmill.SetFighter(
            Editor.FighterChooser.FightersCollection.Fighters.Values.ToList()[Random.Range(0, 2)]);
    }

    private void SetEditor()
    {
        if (CurrentMenu == CurrentMenu.EDITOR) return;
        CurrentMenu = CurrentMenu.EDITOR;
        UnSetArena();
        UnSetMain();

        CameraTransform.DOKill();
        Editor.FighterChooser.SetFighter(FighterNum.F1);
        CameraTransform.DOMove(ObjectsPositions.CamEditorPos, MenuTransitionSpeed).SetEase(Ease.OutQuad);
        Editor.transform.DOMove(ObjectsPositions.UIEditorPos, MenuTransitionSpeed).SetEase(Ease.OutQuad);
    }

    private void SetArena(UserPlayer player, UserPlayer enemy)
    {
        if (CurrentMenu == CurrentMenu.ARENA) return;
        CurrentMenu = CurrentMenu.ARENA;
        UnSetEditor();
        UnSetMain();

        CameraTransform.DOKill();
        CameraTransform.DOMove(ObjectsPositions.CamArenaPos, MenuTransitionSpeed).SetEase(Ease.OutQuad);
        MainMenu.MenuTransform.DOLocalMove(ObjectsPositions.UIMainMenuPosHidden, MenuTransitionSpeed).SetEase(Ease.OutQuad);
        Arena.ArenaUICanvas.transform.DOLocalMove(ObjectsPositions.UIArenaPos, MenuTransitionSpeed)
            .SetEase(Ease.InQuad);
        Arena.SetUpArena(player, enemy);
    }

    private void UnSetMain()
    {
        MainMenu.Treadmill.Stop();
        MainMenu.RankTransform.DOLocalMove(ObjectsPositions.UIMainRankPosHidden, MenuTransitionSpeed)
            .SetEase(Ease.OutQuad);
    }

    private void UnSetEditor()
    {
        
        Editor.FighterEditor.UnSetFighter();
        Editor.transform.DOMove(ObjectsPositions.UIEditorPosHidden, MenuTransitionSpeed).SetEase(Ease.InQuad);
    }

    private void UnSetArena()
    {
        MainMenu.MenuTransform.DOLocalMove(ObjectsPositions.UIMainMenuPos, MenuTransitionSpeed).SetEase(Ease.InQuad);
        Arena.ArenaUICanvas.transform.DOLocalMove(ObjectsPositions.UIArenaPosHidden, MenuTransitionSpeed)
            .SetEase(Ease.OutQuad);
    }
}