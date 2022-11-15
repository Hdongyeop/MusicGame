using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectManager : MonoBehaviour
{
    private string hit = "Hit";
    
    [SerializeField] private Animator noteHitAnimator;
    [SerializeField] private Animator judgementAnimator;
    [SerializeField] private Sprite[] judgementSprites;
    [SerializeField] private Image judgementImage;
    
    // 0 : Perfect, 1 : Cool, 2 : Good, 3 : Bad
    public void JudgementEffect(int num)
    {
        judgementImage.sprite = judgementSprites[num];
        judgementAnimator.SetTrigger(hit);
    }
    
    public void NoteHitEffect()
    {
        noteHitAnimator.SetTrigger(hit);
    }
}
