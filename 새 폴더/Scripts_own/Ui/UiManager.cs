using System;using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PsychoticLab;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UiManager : MonoBehaviour
{
    [SerializeField] private CharacterRandomizer custom;

    
    public void pointEnter(CanvasGroup a)
    {
        a.DOFade(1, 0.3f);
    }

    public void pointExit(CanvasGroup a)
    {
        a.DOFade(0.5f, 0.3f);
    }

    public void playerScene()
    {
        SceneManager.LoadScene("GuideScene 1");
    }
    
   
}
