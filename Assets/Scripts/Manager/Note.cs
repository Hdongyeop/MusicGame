using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    public float noteSpeed = 400f;

    private Image _noteImage;

    private void OnEnable()
    {
        if(_noteImage == null)
            _noteImage = GetComponent<Image>();

        _noteImage.enabled = true;
    }

    public void HideNote()
    {
        _noteImage.enabled = false;
    }

    private void Update()
    {
        transform.localPosition += Vector3.right * noteSpeed * Time.deltaTime;
    }

    public bool GetNoteFlag()
    {
        return _noteImage.enabled;
    }
}
