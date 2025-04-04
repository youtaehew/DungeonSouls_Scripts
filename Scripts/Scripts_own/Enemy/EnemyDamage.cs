using System;
using System.Collections;
using System.Collections.Generic;
using EmeraldAI;
using TMPro;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private EmeraldAISystem emeraldAISystem;
    private EmeraldAIEventsManager emeraldAIEventsManager;
    private TextMeshProUGUI textMeshProUGUI;

    public int RangeAttackDamage = 30;

    private void Start()
    {
        textMeshProUGUI = GameObject.Find("Player").GetComponentInChildren<TextMeshProUGUI>();
        textMeshProUGUI.alpha = 0;
        emeraldAISystem = GetComponent<EmeraldAISystem>();
        emeraldAIEventsManager = emeraldAISystem.EmeraldEventsManagerComponent;

        for (int i = 1; i <= 5; i++)
        {
            emeraldAIEventsManager.UpdateAIMeleeDamage(i, 30,35);
        }
        emeraldAIEventsManager.UpdateAIMeleeDamage(6, 15,25);
    }
}
