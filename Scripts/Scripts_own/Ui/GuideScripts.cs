using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GuideScripts : MonoBehaviour
{
    [SerializeField] private GameObject[] Pages;

    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private GameObject EndButton;
    [SerializeField] private GameObject NextButton;
    private int PageCountText = 1;
    private int PageCount = 0;

    public void Next()
    {
        CustomSound.instance.movePlaySound();
        Pages[PageCount].SetActive(false);
        PageCount++;
        PageCountText++;
        Pages[PageCount].SetActive(true);
        if (PageCount >= 2)
        {
            NextButton.SetActive(false);
            EndButton.SetActive(true);
        }
        textMeshProUGUI.text = PageCountText + "/3";
    }

    public void End()
    {
        SceneManager.LoadScene("PlayScene");
    }
}
