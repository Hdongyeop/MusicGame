using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterFrame : MonoBehaviour
{
    private bool _musicStart = false;

    public string bgmName;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_musicStart == true) return;
        
        if (col.CompareTag("Note"))
        {
            AudioManager.Instance.PlayBGM(bgmName);
            _musicStart = true;
        }
    }

    public void ResetMusic()
    {
        _musicStart = false;
    }
}
