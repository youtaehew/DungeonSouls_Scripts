using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CreatePortal : MonoBehaviour
{
    private ParticleSystem ps;
    private CinemachineVirtualCamera PortalCam;
    

    private void Start()
    {
        PortalCam = GetComponent<CinemachineVirtualCamera>();
        ps = GetComponentInChildren<ParticleSystem>();
    }

    public void ChangePortalCam()
    {
        StartCoroutine("MovePortal");
    }

    IEnumerator MovePortal()
    {
        yield return new WaitForSeconds(3.5f);
        PortalCam.Priority = 20;
        yield return new WaitForSeconds(3f);
        ps.Play();
        GameManager.instance.PortalSound();
        yield return new WaitForSeconds(2.5f);
        PortalCam.Priority = 3;

    }
    
}
