using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Result : MonoBehaviour
{
    // Result 결과창
    [SerializeField] private GameObject goUI;
    [SerializeField] private TextMeshProUGUI[] textCount;
    [SerializeField] private TextMeshProUGUI textCoin;
    [SerializeField] private TextMeshProUGUI textScore;
    [SerializeField] private TextMeshProUGUI textMaxCombo;
    [SerializeField] private StageMenu stageMenu;
    
    private ScoreManager _theScoreManager;
    private ComboManager _theComboManager;
    private TimingManager _theTimingManager;
    private DatabaseManager _theDatabaseManager;

    private int _currentSongIndex = 0;
    
    private void Start()
    {
        _theScoreManager = FindObjectOfType<ScoreManager>();
        _theComboManager = FindObjectOfType<ComboManager>();
        _theTimingManager = FindObjectOfType<TimingManager>();
        _theDatabaseManager = FindObjectOfType<DatabaseManager>();
    }

    public void ShowResult()
    {
        // BGM Stop
        AudioManager.Instance.StopBGM();
        
        // BGM flag reset
        FindObjectOfType<CenterFrame>().ResetMusic();
        
        goUI.SetActive(true);
        foreach (var text in textCount)
            text.text = "0";

        textCoin.text = "0";
        textScore.text = "0";
        textMaxCombo.text = "0";

        // Set Judgement Text
        int[] tJudgement = _theTimingManager.GetJudgementRecord();
        for (int i = 0; i < tJudgement.Length; i++)
            textCount[i].text = $"{tJudgement[i]:#,##0}";
        
        // Set Score Text
        int tCurrentScore = _theScoreManager.GetCurrentScore();
        textScore.text = $"{tCurrentScore:#,##0}";
        
        // Set MaxCombo Text
        int tMaxCombo = _theComboManager.GetMaxCombo();
        textMaxCombo.text = $"{tMaxCombo:#,##0}";
        
        // Set Coin Text
        int tCoin = tCurrentScore / 50;
        textCoin.text = $"{tCoin:#,##0}";

        // 최고 점수 갱신
        if(tCurrentScore > _theDatabaseManager.score[_currentSongIndex])
        {
            _theDatabaseManager.score[_currentSongIndex] = tCurrentScore;
            _theDatabaseManager.SaveScore();
        }
    }

    public void BtnMainMenu()
    {
        goUI.SetActive(false);
        GameManager.Instance.MainMenu();
        _theComboManager.ResetCombo();
    }

    public void BtnRetryMenu()
    {
        goUI.SetActive(false);
        _theComboManager.ResetCombo();
        
        stageMenu.BtnPlay();
    }
    
    public void SetCurrentSong(int pSongNum)
    {
        _currentSongIndex = pSongNum;
    }
}
