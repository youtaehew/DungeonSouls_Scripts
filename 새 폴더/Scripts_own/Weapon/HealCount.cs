using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealCount : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI count;

    private void Start()
    {
        count = GetComponent<TextMeshProUGUI>();
    }

    public void HealControll(int heal)
    {
        count.text = "X" + heal;
    }
}
