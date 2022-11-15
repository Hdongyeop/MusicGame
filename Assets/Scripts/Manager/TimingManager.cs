using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    private Vector2[] _timingBoxs;
    private EffectManager _theEffectManager;
    private ScoreManager _theScoreManager;
    private ComboManager _theComboManager;
    private StageManager _theStageManager;
    private PlayerController _thePlayerController;
    private StatusManager _theStatusManager;
    private AudioManager _theAudioManager;

    private int[] _judgementRecord = new int[5];

    [SerializeField] private Transform center;
    [SerializeField] private RectTransform[] timingRect;

    public List<GameObject> boxNoteList = new();

    private void Start()
    {
        _theEffectManager = FindObjectOfType<EffectManager>();
        _theScoreManager = FindObjectOfType<ScoreManager>();
        _theComboManager = FindObjectOfType<ComboManager>();
        _theStageManager = FindObjectOfType<StageManager>();
        _thePlayerController = FindObjectOfType<PlayerController>();
        _theStatusManager = FindObjectOfType<StatusManager>();
        _theAudioManager = AudioManager.Instance;

        _timingBoxs = new Vector2[timingRect.Length];
        for (int i = 0; i < timingRect.Length; i++)
        {
            _timingBoxs[i].Set(center.localPosition.x - timingRect[i].rect.width / 2,
                center.localPosition.x + timingRect[i].rect.width / 2);
        }
    }

    public void Initialized()
    {
        for (int i = 0; i < _judgementRecord.Length; i++)
            _judgementRecord[i] = 0;
        
    }

    public bool CheckTiming()
    {
        for (int i = 0; i < boxNoteList.Count; i++)
        {
            float tNotePosX = boxNoteList[i].transform.localPosition.x;

            // 0 : Perfect, 1 : Cool, 2 : Good, 3 : Bad
            for (int x = 0; x < _timingBoxs.Length; x++)
            {
                if (_timingBoxs[x].x <= tNotePosX && tNotePosX <= _timingBoxs[x].y)
                {
                    // Hide note
                    boxNoteList[i].GetComponent<Note>().HideNote();
                    boxNoteList.RemoveAt(i);
                    
                    // Hit Effects
                    if(x < _timingBoxs.Length - 1) // No Hit Effect on Bad
                        _theEffectManager.NoteHitEffect();
                    
                    // Check player's destPos can be reached
                    if (CheckCanNextPlate())
                    {
                        // Get Score
                        _theScoreManager.IncreaseScore(x);
                        
                        // Show next plate
                        _theStageManager.ShowNextPlate();
                        
                        // Judgement Effect
                        _theEffectManager.JudgementEffect(x);
                        
                        // Record judgement
                        _judgementRecord[x]++;
                        
                        // Check Shield
                        _theStatusManager.CheckShield();
                    }
                    else
                    {
                        _theEffectManager.JudgementEffect(5); // 5 : Normal
                    }
                    
                    // Play clap sound when right judgement
                    _theAudioManager.PlaySFX("Clap");
                    
                    return true;
                }
            }
        }
        
        // When Miss
        _theComboManager.ResetCombo();
        _theEffectManager.JudgementEffect(_timingBoxs.Length);
        MissRecord();
        return false;
    }

    bool CheckCanNextPlate()
    {
        if (Physics.Raycast(_thePlayerController.destPos, Vector3.down, out var rayhit, 1.1f))
        {
            if (rayhit.transform.CompareTag("BasicPlate"))
            {
                BasicPlate tPlate = rayhit.transform.GetComponent<BasicPlate>();
                if (tPlate.flag)
                {
                    tPlate.flag = false;
                    return true;
                }
            }
        }

        return false;
    }

    public int[] GetJudgementRecord()
    {
        return _judgementRecord;
    }

    public void MissRecord()
    {
        _judgementRecord[4]++;
    }
}
