using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RandomColorText : MonoBehaviour
{
    private Image sprite;
    private TextMeshProUGUI text;
    void Start()
    {
        sprite = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        StartCoroutine("Range");
    }

    IEnumerator Range()
    {
        Color color;
        while (true)
        {
            color = new Color(Random.value, Random.value, Random.value, 1f);
            sprite.DOColor(color, 1.1f);
            text.DOColor(color, 1f);
            yield return new WaitForSeconds(1.5f);
        }
    }

    
}
