using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;

    private void Start()
    {
        cinemachineBasicMultiChannelPerlin = GetComponent<CinemachineVirtualCamera>()
            .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeStart()
    {
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 2;
    }
    
    public void ShakeEnd()
    {
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
        
    }

    public void TimeReset()
    {
        Time.timeScale = 1; 
    }
}
