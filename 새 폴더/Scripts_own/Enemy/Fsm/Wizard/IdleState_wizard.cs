using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState_wizard : State
{
    public AttackState_wizard attackStateWizard;

    public LayerMask detectionLayer;
    public HitState_wizard hitState;
    [SerializeField] private EnemyStats _enemyStat;
    [SerializeField] private EnemyStats CheckEnemyStat;
    [SerializeField]private EnemyStats own;
    private float minDistance = 100;

    private CharacterStats characterStats;

    private void Start()
    {
        own = GetComponentInParent<EnemyStats>();
    }

    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatonManager enemyAni)
    {
        #region 적 감지

        Collider[] colliders = Physics.OverlapSphere(transform.position, 10f/*enemyManager.detectionRadius*/, detectionLayer);//dectionradius
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Player"))
                characterStats = colliders[i].transform.GetComponent<CharacterStats>();
            
            if (colliders[i].CompareTag("Enemy"))
            {
                if (colliders[i].transform != own.transform)
                {
                    CheckEnemyStat = colliders[i].transform.GetComponent<EnemyStats>();
                    float distance = Vector3.Distance(CheckEnemyStat.transform.position, transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        _enemyStat = CheckEnemyStat;
                    }   
                }

            }


            if (_enemyStat != null)
            {
                if (_enemyStat.enemyGather)
                {
                    enemyManager.currentTarget = characterStats;
                }
            }


            if (characterStats != null)
            {
                Vector3 targetDirection = characterStats.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
                if (viewableAngle > enemyManager.minimumDetectionAngle &&
                    viewableAngle < enemyManager.maximumDetectionAngle)
                {
                    enemyManager.currentTarget = characterStats;
                }
            }
        }

        #endregion

        #region 상태변화

        if (enemyStats.isHit) return hitState;
        if (enemyManager.currentTarget != null)
        {
            return attackStateWizard;
        }
        else
        {
            return this;
        }

        #endregion
    }
}
