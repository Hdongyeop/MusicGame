using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    private double _currentTime = 0d;
    private TimingManager _theTimingManager;
    private EffectManager _theEffectManager;
    private ComboManager _theComboManager;
    private StatusManager _theStatusManager;
    
    [SerializeField] private Transform tfNoteAppear = null;
    
    // Beat per minute
    public int bpm = 0;
    
    private void Start()
    {
        _theTimingManager = GetComponent<TimingManager>();
        _theEffectManager = FindObjectOfType<EffectManager>();
        _theComboManager = FindObjectOfType<ComboManager>();
        _theStatusManager = FindObjectOfType<StatusManager>();
    }

    private void Update()
    {
        if (GameManager.Instance.isStartGame == false) return;
        
        _currentTime += Time.deltaTime;
        if (_currentTime >= 60d / bpm)
        {
            // Get a note from ObjectPool
            GameObject tNote = ObjectPool.Instance.NoteQueue.Dequeue();
            tNote.transform.position = tfNoteAppear.position;
            tNote.SetActive(true);
            
            _theTimingManager.boxNoteList.Add(tNote);
            _currentTime -= 60d / bpm;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Note"))
        {
            // When Miss
            if (col.GetComponent<Note>().GetNoteFlag())
            {
                _theTimingManager.MissRecord();
                _theEffectManager.JudgementEffect(4);
                _theComboManager.ResetCombo();
                _theStatusManager.ResetShieldCombo();
            }
            
            _theTimingManager.boxNoteList.Remove(col.gameObject);
            
            // Return a object to pool
            col.gameObject.SetActive(false);
            ObjectPool.Instance.NoteQueue.Enqueue(col.gameObject);
        }
    }

    public void RemoveNote()
    {
        GameManager.Instance.isStartGame = false;
        
        for (int i = 0; i < _theTimingManager.boxNoteList.Count; i++)
        {
            _theTimingManager.boxNoteList[i].SetActive(false);
            ObjectPool.Instance.NoteQueue.Enqueue(_theTimingManager.boxNoteList[i]);
        }
        
        _theTimingManager.boxNoteList.Clear();
    }
}
