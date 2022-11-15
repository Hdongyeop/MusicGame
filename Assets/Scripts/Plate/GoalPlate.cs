using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPlate : MonoBehaviour
{
    private AudioSource _audio;
    private NoteManager _theNoteManager;
    private Result _theResult;
    
    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        _theNoteManager = FindObjectOfType<NoteManager>();
        _theResult = FindObjectOfType<Result>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _audio.Play();
            PlayerController.SCanPressKey = false;
            _theNoteManager.RemoveNote();
            
            _theResult.ShowResult();
        }
    }
}
