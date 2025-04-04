using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField] private GameObject LoadingScreen;
    [SerializeField] private Image LoadingBar;

    public void LoadScene(int sceneId)
    {
    LoadingScreen.SetActive(true);
        LoadingBar.DOFillAmount(1, 2).OnComplete(() => { SceneManager.LoadScene(sceneId); });
    }
}