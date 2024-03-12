using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickStart : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private StartScene startScene;
    

    public void OnPointerClick(PointerEventData eventData)
    {
        if (startScene.canStart)
        {
            Debug.Log("Clixk");
        }
    }
}
