using System;
using System.Collections;
using System.Collections.Generic;
using EmeraldAI;
using EmeraldAI.Utility;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : MonoBehaviour
{
    [SerializeField]private Image image;
    
    [SerializeField]private EmeraldAISystem emeraldAISystem;
   

    public void UpdateHp()
    {
        image.fillAmount = emeraldAISystem.CurrentHealth / emeraldAISystem.StartingHealth;
    }
    
    
    
    
    
}
