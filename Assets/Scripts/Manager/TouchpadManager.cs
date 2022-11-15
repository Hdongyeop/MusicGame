using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchpadManager : MonoBehaviour
{
    public bool isTouched = false;
    public Vector3 dir = Vector3.zero;
    
    public void PressW()
    {
        dir = new Vector3(1, 0, 0);
        isTouched = true;
        StartCoroutine(CoTouchable());
    }
    
    public void PressS()
    {
        dir = new Vector3(-1, 0, 0);
        isTouched = true;
        StartCoroutine(CoTouchable());
    }
    
    public void PressD()
    {
        dir = new Vector3(0, 0, 1);
        isTouched = true;
        StartCoroutine(CoTouchable());
    }
    
    public void PressA()
    {
        dir = new Vector3(0, 0, -1);
        isTouched = true;
        StartCoroutine(CoTouchable());
    }

    IEnumerator CoTouchable()
    {
        yield return new WaitForSeconds(0.1f);
        isTouched = false;
    }
}
