    using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;


public class HpBar : MonoBehaviour, Interface
{
    [SerializeField] private Image hpBar;
    [SerializeField] private Image hpBar_main;
    
    private Camera mainCam;
    private float target;

    private void Start()
    {
        mainCam = Camera.main;
    }

    public void updateHp(float currentHp, float maxHp)
    {
        target = currentHp / maxHp;
        hpBar_main.fillAmount = target;
        StartCoroutine("reduceHp");
    }

    IEnumerator reduceHp()
    {
        yield return new WaitForSeconds(0.6f);
        hpBar.DOFillAmount(target, 3f);
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - mainCam.transform.position);
        if (hpBar.fillAmount <= 0)
        {
            gameObject.SetActive(false);
        }
    }


    public void UpdateHp(float currentHp, float maxHp)
    {
        throw new NotImplementedException();
    }
}
