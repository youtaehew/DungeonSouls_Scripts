using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Hellmade.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TitleFont;
    [SerializeField] private TextMeshProUGUI StartFont;
    [SerializeField] private AudioClip StartClip;

    [SerializeField] private AudioClip ClickAudio;
    public bool canStart = false;

    private void Start()
    {
        TitleFont.DOFade(0.9f, 4f).OnComplete(() => StartFontBlink());
    }


    private void StartFontBlink()
    {
        EazySoundManager.PlaySound(StartClip, true);
        canStart = true;
        StartFont.DOFade(1f, 1).SetLoops(-1, LoopType.Yoyo);
    }
    
    public void Click()
    {
        if (canStart)
        {
           StartCoroutine("GameStart");
           canStart = false;
        }
    }
    IEnumerator GameStart()
    {
        EazySoundManager.PlaySound(ClickAudio);
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("CharacterCustom");
    }


    
}