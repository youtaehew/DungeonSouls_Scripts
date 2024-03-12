using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Hellmade.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingScene : MonoBehaviour
{
    public static EndingScene instance;
    [SerializeField] private TextMeshProUGUI EndingFont;
    [SerializeField] private Image image;


    [SerializeField] private AudioClip ClickAudio;
    [SerializeField] private AudioClip ClearAudio;
    [SerializeField] private GameObject buttons;

    [SerializeField] private GameObject PlayerUi;

    private void Awake()
    {
        instance = this;
    }


    public void DoEnding()
    {
        EazySoundManager.PlaySound(ClearAudio);
        PlayerUi.SetActive(false);
        Cursor.lockState = CursorLockMode.None;

        Cursor.visible = true;
        image.DOFade(1f, 3f).OnComplete(()=>EndingFont.DOFade(1f, 1f).OnComplete(() => EndingButton()));
    }

    public void EndingButton()
    {
        buttons.SetActive(true);
    }

    public void ButtonExit()
    {
        EazySoundManager.PlaySound(ClickAudio);
        Application.Quit();
    }

    public void ButtonRetry()
    {
        EazySoundManager.PlaySound(ClickAudio);
        SceneManager.LoadScene("CharacterCustom");
    }
}
