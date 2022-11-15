using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    private int _currentCombo = 0;
    private int _maxCombo = 0;
    private Animator _animator;
    private string _animComboUp = "ComboUp";

    [SerializeField] private GameObject goComboImage;
    [SerializeField] private TextMeshProUGUI textCombo;
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
        
        textCombo.gameObject.SetActive(false);
        goComboImage.SetActive(false);
    }

    public void IncreaseCombo(int pNum = 1)
    {
        _currentCombo += pNum;
        textCombo.text = string.Format("{0:#,##0}", _currentCombo);

        if (_maxCombo < _currentCombo)
            _maxCombo = _currentCombo;
        
        if (_currentCombo > 2)
        {
            textCombo.gameObject.SetActive(true);
            goComboImage.SetActive(true);
            
            _animator.SetTrigger(_animComboUp);
        }
    }

    public void ResetCombo()
    {
        _currentCombo = 0;
        textCombo.text = "0";
        textCombo.gameObject.SetActive(false);
        goComboImage.SetActive(false);
    }

    public int GetCurrentCombo()
    {
        return _currentCombo;
    }

    public int GetMaxCombo()
    {
        return _maxCombo;
    }
}
