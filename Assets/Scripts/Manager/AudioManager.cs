using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [SerializeField] private Sound[] sfx;
    [SerializeField] private Sound[] bgm;

    [SerializeField] private AudioSource bgmPlayer;
    [SerializeField] private AudioSource[] sfxPlayer;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    public void PlayBGM(string pBgmName)
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            if (pBgmName == bgm[i].name)
            {
                bgmPlayer.clip = bgm[i].clip;
                bgmPlayer.Play();
                return;
            }
        }
        Debug.Log(pBgmName + " 오디오 파일이 없습니다.");
    }

    public void StopBGM()
    {
        bgmPlayer.Stop();
    }
    
    public void PlaySFX(string pSfxName)
    {
        for (int i = 0; i < sfx.Length; i++)
        {
            if (pSfxName == sfx[i].name)
            {
                for (int j = 0; j < sfxPlayer.Length; j++)
                {
                    if (sfxPlayer[j].isPlaying == false)
                    {
                        sfxPlayer[j].clip = sfx[i].clip;
                        sfxPlayer[j].Play();
                        return;
                    }
                }
                
                Debug.Log("모든 SFXPlayer가 재생중입니다. ");
                return;
            }
        }
        
        Debug.Log(pSfxName + " 오디오 파일이 없습니다.");
    }
}
