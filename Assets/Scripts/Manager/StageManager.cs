using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private Transform[] _stagePlates;
    private int _stepCount = 0;
    private int _totalPlateCount;
    private GameObject _currentStage;

    [SerializeField] private GameObject[] stageArray;
    [SerializeField] private float offsetY = -3f;
    [SerializeField] private float plateSpeed = 10f;

    public void SettingStage(int pSongNum)
    {
        _stepCount = 0;
        
        _currentStage = Instantiate(stageArray[pSongNum], Vector3.zero, Quaternion.identity, transform);
        
        _stagePlates = _currentStage.GetComponent<Stage>().plates;
        _totalPlateCount = _stagePlates.Length;

        for (int i = 0; i < _totalPlateCount; i++)
        {
            _stagePlates[i].position = new Vector3(
                _stagePlates[i].position.x,
                _stagePlates[i].position.y + offsetY,
                _stagePlates[i].position.z
                );
        }
    }

    public void RemoveStage()
    {
        if (_currentStage == null) return;

        Destroy(_currentStage);
    }
    
    public void ShowNextPlate()
    {
        if (_stepCount < _totalPlateCount)
            StartCoroutine(CoMovePlate(_stepCount++));
    }

    IEnumerator CoMovePlate(int pNum)
    {
        _stagePlates[pNum].gameObject.SetActive(true);
        Vector3 tDestPos = new Vector3(
            _stagePlates[pNum].position.x,
            _stagePlates[pNum].position.y - offsetY,
            _stagePlates[pNum].position.z
        );

        while (Vector3.SqrMagnitude(_stagePlates[pNum].position - tDestPos) >= 0.001f)
        {
            _stagePlates[pNum].position = Vector3.Lerp(_stagePlates[pNum].position, tDestPos, plateSpeed * Time.deltaTime);
            yield return null;
        }

        _stagePlates[pNum].position = tDestPos;
    }
}
