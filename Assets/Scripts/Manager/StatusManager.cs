using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    private Result _theResult;
    private NoteManager _theNoteManager;
    
    private int _maxHp = 3;
    private int _currentHp = 3;

    private int _maxShield = 3;
    private int _currentShield = 0;

    private bool _isDead = false;
    private bool _isBlink = false;

    [SerializeField] private GameObject[] hpImage;
    [SerializeField] private GameObject[] shieldImage;

    [SerializeField] private float blinkSpeed = 0.1f;
    [SerializeField] private int blinkCount = 10;
    private int _currentBlinkCount = 0;
    [SerializeField] private MeshRenderer playerMesh;

    [SerializeField] private int shieldIncreaseCombo = 5;
    private int _currentShieldCombo = 0;
    [SerializeField] private Image shieldGauge;
    
    private void Start()
    {
        _theResult = FindObjectOfType<Result>();
        _theNoteManager = FindObjectOfType<NoteManager>();
    }

    public void Initialized()
    {
        _currentHp = _maxHp;
        _currentShield = 0;
        _currentShieldCombo = 0;
        shieldGauge.fillAmount = 0;
        _isDead = false;
        
        SettingHpImage();
        SettingShieldImage();
    }
    
    #region Shield
    
    public void IncreaseShield(int pNum = 1)
    {
        _currentShield += pNum;
        if (_currentShield >= _maxShield)
            _currentShield = _maxShield;
        
        SettingShieldImage();
    }

    public void DecreaseShield(int pNum = 1)
    {
        _currentShield -= pNum;
        if (_currentShield <= 0)
            _currentShield = 0;
        
        SettingShieldImage();
    }

    public void CheckShield()
    {
        _currentShieldCombo++;

        if (_currentShieldCombo >= shieldIncreaseCombo)
        {
            IncreaseShield();
            _currentShieldCombo = 0;
        }

        shieldGauge.fillAmount = (float)_currentShieldCombo / shieldIncreaseCombo;
    }

    public void ResetShieldCombo()
    {
        _currentShieldCombo = 0;
        shieldGauge.fillAmount = (float)_currentShieldCombo / shieldIncreaseCombo;
    }
    
    void SettingShieldImage()
    {
        for (int i = 0; i < shieldImage.Length; i++)
        {
            if(i < _currentShield)
                shieldImage[i].SetActive(true);
            else
                shieldImage[i].SetActive(false);
        }
    }
    
    #endregion

    #region Hp

    public void IncreaseHp(int pNum = 1)
    {
        _currentHp += pNum;
        if (_currentHp >= _maxHp)
            _currentHp = _maxHp;
        
        SettingHpImage();
    }
    
    public void DecreaseHp(int pNum = 1)
    {
        // 무적시간이면 체력감소 안함
        if (_isBlink) return;

        if (_currentShield > 0)
        {
            DecreaseShield(pNum);
            return;
        }
        
        _currentHp -= pNum;
        
        if (_currentHp <= 0)
        {
            _isDead = true;
            _theResult.ShowResult();
            _theNoteManager.RemoveNote();
        }
        else
        {
            StartCoroutine(CoBlink());
        }
        
        SettingHpImage();
    }

    void SettingHpImage()
    {
        for (int i = 0; i < hpImage.Length; i++)
        {
            if(i < _currentHp)
                hpImage[i].SetActive(true);
            else
                hpImage[i].SetActive(false);
        }
    }
    
    #endregion

    public bool IsDead() => _isDead;

    IEnumerator CoBlink()
    {
        _isBlink = true;
        
        while (_currentBlinkCount <= blinkCount)
        {
            playerMesh.enabled = !playerMesh.enabled;
            yield return new WaitForSeconds(blinkSpeed);
            _currentBlinkCount++;
        }

        _isBlink = false;
        _currentBlinkCount = 0;
        playerMesh.enabled = true;
    }
}
