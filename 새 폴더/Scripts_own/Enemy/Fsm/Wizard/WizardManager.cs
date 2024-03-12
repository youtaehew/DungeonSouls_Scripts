using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class WizardManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem FireBall;
    [SerializeField] private ParticleSystem FireWall;
    [SerializeField] private GameObject FireWallPos;
    [SerializeField] private ParticleSystem FireSpell;

    private EnemyManager enemyManager;

    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
    }

    public void Spell()
    {
        FireWall.transform.position = enemyManager.vThirdPersonController.transform.position;
        FireSpell.Play();
    }
    
    public void SpellFireball()
    {
        FireBall.Play();
    }

    public void SpellFireWall()
    {
        FireWall.Play();
    }
}
