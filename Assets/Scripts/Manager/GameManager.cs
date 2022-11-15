using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private ComboManager _theComboManager;
    private ScoreManager _theScoreManager;
    private TimingManager _theTimingManager;
    private StatusManager _theStatusManager;
    private PlayerController _thePlayerController;
    private StageManager _theStageManager;
    private NoteManager _theNoteManager;
    private Result _theResult;
    
    // In Game UI (HUD)
    [SerializeField] private GameObject[] goGameUI;
    [SerializeField] private GameObject goTitleUI;
    
    // Center Frame
    [SerializeField] private CenterFrame goCenterFrame;

    public static GameManager Instance;

    public bool isStartGame = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        _theComboManager = FindObjectOfType<ComboManager>();
        _theScoreManager = FindObjectOfType<ScoreManager>();
        _theTimingManager = FindObjectOfType<TimingManager>();
        _theStatusManager = FindObjectOfType<StatusManager>();
        _thePlayerController = FindObjectOfType<PlayerController>();
        _theStageManager = FindObjectOfType<StageManager>();
        _theNoteManager = FindObjectOfType<NoteManager>();
        _theResult = FindObjectOfType<Result>();
    }

    public void GameStart(int pSongIndex, int pSongBpm)
    {
        for (int i = 0; i < goGameUI.Length; i++)
            goGameUI[i].SetActive(true);
        
        _theStageManager.RemoveStage();
        _theStageManager.SettingStage(pSongIndex);
        
        _theComboManager.ResetCombo();
        _theScoreManager.Initialized();
        _theTimingManager.Initialized();
        _theStatusManager.Initialized();
        _thePlayerController.Initialized();
        _theResult.SetCurrentSong(pSongIndex);
        
        // Song ready
        goCenterFrame.bgmName = "BGM" + pSongIndex;
        _theNoteManager.bpm = pSongBpm;
        
        AudioManager.Instance.StopBGM();
        isStartGame = true;
    }

    public void MainMenu()
    {
        for (int i = 0; i < goGameUI.Length; i++)
            goGameUI[i].SetActive(false);

        goTitleUI.SetActive(true);
    }
}
