using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using DG.Tweening;

public class CreateBridge : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
    private Transform Bridge;
    
    
    

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        Bridge = GameObject.Find("Environment").transform.Find("Bridge");
        cinemachineBasicMultiChannelPerlin =
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ChangeCameara()
    {
        StartCoroutine("MoveBridge");
    }

    IEnumerator MoveBridge()
    {
        yield return new WaitForSeconds(3.5f);
        virtualCamera.Priority = 20;
        yield return new WaitForSeconds(3f);
        GameManager.instance.GroundSound();
        StartCoroutine("Shake");
        Bridge.DOLocalMoveY(-95.94f, 2f);
        yield return new WaitForSeconds(2.5f);
        virtualCamera.Priority = 5;
    }

    IEnumerator Shake()
    {
        
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 5f;
        yield return new WaitForSeconds(2f);
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
    }
    
    
}
