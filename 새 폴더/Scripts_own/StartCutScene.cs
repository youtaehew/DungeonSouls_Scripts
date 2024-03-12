using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class StartCutScene : MonoBehaviour
{
    private PlayableDirector pd;
    [SerializeField] private TimelineAsset ta;

    private void Start()
    {
        pd = GetComponent<PlayableDirector>();
        if (GameManager.instance.PlayCutScene == 0)
        {
            pd.Play(ta);
            GameManager.instance.PlayCutScene++;
        }
    }
}
