using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Song
{
    public string name;
    public string composer;
    public int bpm;
    public Sprite sprite;
}

public class StageMenu : MonoBehaviour
{
    private int _currentSongIndex = 0;
    private DatabaseManager _theDatabaseManager;
    
    [SerializeField] private Song[] songList;
    [SerializeField] private TextMeshProUGUI textSongName;
    [SerializeField] private TextMeshProUGUI textSongComposer;
    [SerializeField] private TextMeshProUGUI textSongScore;
    [SerializeField] private Image imgDisk;
    
    [SerializeField] private GameObject titleMenu;

    private void OnEnable()
    {
        if(_theDatabaseManager == null)
            _theDatabaseManager = FindObjectOfType<DatabaseManager>();
        SettingSong();
    }

    public void BtnNext()
    {
        AudioManager.Instance.PlaySFX("Touch");
        
        // _currentSongIndex = (_currentSongIndex + 1) % songList.Length;
        if (++_currentSongIndex > songList.Length - 1)
            _currentSongIndex = 0;
        // Debug.Log("BtnNext");
        SettingSong();
    }

    public void BtnPrev()
    {
        AudioManager.Instance.PlaySFX("Touch");
        
        if (--_currentSongIndex < 0)
            _currentSongIndex = songList.Length - 1;
        // Debug.Log("BtnPrev");
        SettingSong();
    }

    public void SettingSong()
    {
        textSongName.text = songList[_currentSongIndex].name;
        textSongComposer.text = songList[_currentSongIndex].composer;
        textSongScore.text = $"{_theDatabaseManager.score[_currentSongIndex]:#,##0}";
        imgDisk.sprite = songList[_currentSongIndex].sprite;
        
        AudioManager.Instance.PlayBGM("BGM" + _currentSongIndex);
    }
    
    public void BtnBack()
    {
        titleMenu.SetActive(true);
        AudioManager.Instance.StopBGM();
        this.gameObject.SetActive(false);
    }

    public void BtnPlay()
    {
        int tBpm = songList[_currentSongIndex].bpm;
        
        GameManager.Instance.GameStart(_currentSongIndex, tBpm);
        this.gameObject.SetActive(false);
    }
}
