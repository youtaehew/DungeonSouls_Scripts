using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutScene : MonoBehaviour
{
    private PlayableDirector pd;
    [SerializeField] private TimelineAsset ta;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pd = FindObjectOfType<PlayableDirector>();
            if (GameManager.instance.BossCutScene == 0)
            {
                Time.timeScale = .8f;
                pd.Play(ta);
                GameManager.instance.BossCutScene++;
            }
        }
    }
    
    
}