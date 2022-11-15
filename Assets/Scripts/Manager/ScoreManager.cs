using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private int _currentScore = 0;

    private ComboManager _theComboManager;

    [SerializeField] private TextMeshProUGUI textScore;
    [SerializeField] private int baseScore = 10;
    [Tooltip("0 : Perfect, 1 : Cool, 2 : Good, 3 : Bad")]
    [SerializeField] private float[] weight;
    [SerializeField] private int comboBonusScore = 10;
    
    private void Start()
    {
        _theComboManager = FindObjectOfType<ComboManager>();
        
        _currentScore = 0;
        textScore.text = "0";
    }

    public void Initialized()
    {
        _currentScore = 0;
        textScore.text = "0";
    }
    
    public void IncreaseScore(int pJudgement)
    {
        // Combo up
        _theComboManager.IncreaseCombo();

        // Calc combo weight
        int tCurrentCombo = _theComboManager.GetCurrentCombo();
        int tComboBonusScore = (tCurrentCombo / 10) * comboBonusScore;
        
        // Calc score weight
        int tScore = (int)(baseScore * weight[pJudgement] + tComboBonusScore);
        _currentScore += tScore;
        
        // Apply Score to text
        textScore.text = string.Format("{0:#,##0}", _currentScore);
    }

    public int GetCurrentScore()
    {
        return _currentScore;
    }
}
