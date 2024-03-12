using System;
using System.Collections;
using System.Collections.Generic;
using Hellmade.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public CharacterData data = new CharacterData();
    public int BossCutScene = 0;
    public int PlayCutScene = 0;

    public int EnemyCount = 24;

    private CreateBridge createBridge;
    [SerializeField] private TextMeshProUGUI EnemyCountText;
    private CreatePortal createPortal;

    [SerializeField] private AudioClip EnemeyKill;
    [SerializeField] private AudioClip EnemyHitSound;
    [SerializeField] private AudioClip PlayerHitSound;
    [SerializeField] private AudioClip TouchClip;
    [SerializeField] private AudioClip GroundClip;
    [SerializeField] private AudioClip portalSoundClip;
    [SerializeField] private AudioClip BossAttack;
    [SerializeField] private AudioClip FireBall;
    [SerializeField] private AudioClip HealSound;
    private bool isBoss = false;

    private void Awake()
    {
        GameManager[] manager = FindObjectsByType<GameManager>(FindObjectsSortMode.None);
        if (manager.Length > 1) Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            for (int i = 0; i < 13; i++)
                data.costume.Add((Human)i, 0);
        }

        SceneManager.sceneLoaded += OnSceneLoad;
    }


    private void OnSceneLoad(Scene arg0, LoadSceneMode arg1)
    {
        createBridge = FindObjectOfType<CreateBridge>();
        createPortal = FindObjectOfType<CreatePortal>();
        if (isBoss == false)
        {
            EnemyCountText = GameObject.Find("Player").GetComponentInChildren<TextMeshProUGUI>();
            EnemyCountText.text = "남은 적: " + EnemyCount + "/24";
        }
    }


    public void EnemyCheck()
    {
        EnemyCount--;
        EnemyCountText.text = "남은 적: " + EnemyCount + "/24";
        if (EnemyCount == 12)
        {
            createBridge.ChangeCameara();
        }
        else if (EnemyCount <= 0)
        {
            createPortal.ChangePortalCam();
            isBoss = true;
        }
    }


    public static void PlaySound(AudioClip clip, float volume)
    {
        EazySoundManager.PlaySound(clip, volume);
    }

    public static void PlaySound(AudioClip clip, float sound, bool loop, Transform sourceTransform)
    {
        EazySoundManager.PlaySound(clip, sound, loop, sourceTransform);
    }

    public void EnemyKillSound()
    {
        EazySoundManager.PlaySound(EnemeyKill);
    }

    public void EnemyHit()
    {
        EazySoundManager.PlaySound(EnemyHitSound);
    }

    public void PlayerHit()
    {
        EazySoundManager.PlaySound(PlayerHitSound);
    }
    public void BossAttackSound()
    {
        EazySoundManager.PlaySound(BossAttack);
    }
    public void FireAttackSound()
    {
        EazySoundManager.PlaySound(FireBall);
    }
    
    public void PotionDrinkSound()
    {
        EazySoundManager.PlaySound(HealSound);
    }

    public void TouchSound(Transform transform)
    {
        EazySoundManager.PlaySound(TouchClip, 0.5f, true, transform);
    }

    public void GroundSound()
    {
        EazySoundManager.PlaySound(GroundClip, 2f);
    }

    public void PortalSound()
    {
        EazySoundManager.PlaySound(portalSoundClip, 1f);
    }

    // public void EnemySoundHit(int name, Transform transform)
    // {
    //     if (name == 0)
    //     {
    //         EazySoundManager.PlaySound(SkeltonSoundClips[0], 1f, false, transform);
    //     }
    //     else if (name == 1)
    //     {
    //         EazySoundManager.PlaySound(GoblinSoundClips[0], 1f, false, transform);
    //     }
    //     else if(name == 2)
    //     {
    //         EazySoundManager.PlaySound(GoblinKingSoundClips[0], 1f, false, transform);
    //     }
    //     else if (name == 3)
    //     {
    //         EazySoundManager.PlaySound(GoblinGeneralSoundClips[0], 1f, false, transform);
    //     }
    //     else if (name == 4)
    //     {
    //         EazySoundManager.PlaySound(WizardSoundClips[0], 1f, false, transform);
    //     }
    // }
    // public void EnemySoundDie(int name, Transform transform)
    // {
    //     if (name == 0)
    //     {
    //         EazySoundManager.PlaySound(SkeltonSoundClips[1], 1f, false, transform);
    //     }
    //     else if (name == 1)
    //     {
    //         EazySoundManager.PlaySound(GoblinSoundClips[1], 1f, false, transform);
    //     }
    //     else if(name == 2)
    //     {
    //         EazySoundManager.PlaySound(GoblinKingSoundClips[1], 1f, false, transform);
    //     }
    //     else if (name == 3)
    //     {
    //         EazySoundManager.PlaySound(GoblinGeneralSoundClips[1], 1f, false, transform);
    //     }
    //     else if (name == 4)
    //     {
    //         EazySoundManager.PlaySound(WizardSoundClips[1], 1f, false, transform);
    //     }
    // }
}