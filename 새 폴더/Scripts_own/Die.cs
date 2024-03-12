using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Purchasing.MiniJSON;


public class Data
{
    public String text;
}

public class Die : MonoBehaviour
{
    private string dieText;
    private ColorAdjustments color;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    public string filePath;
    private Data _data = new Data();

    private void Awake()
    {
        #region 저장
        filePath = Application.persistentDataPath + "/";
        _data.text = "YOU DIED";
        string data = JsonUtility.ToJson(_data);
        File.WriteAllText(filePath + "DieText", data);
        #endregion
    
        
        
        FindObjectOfType<Volume>().profile.TryGet(out color);
        Debug.Log(filePath);
    }

    public void GreyFade()
    {
        #region 로드

        string data = File.ReadAllText(filePath + "DieText");
        _data = JsonUtility.FromJson<Data>(data);
        #endregion

        textMeshProUGUI.SetText(_data.text);
        DOTween.To(() => color.saturation.value, x => color.saturation.value = x, -100, 0.5f);
        image.DOFade(0.7f, 1f);
        textMeshProUGUI.DOFade(1f, 1f);
    }
}