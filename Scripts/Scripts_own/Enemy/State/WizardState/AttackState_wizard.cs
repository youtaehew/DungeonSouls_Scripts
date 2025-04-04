using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState_wizard : State
{
    public EnemyAttackAction[] enemyAttacks;
    public EnemyAttackAction currentAttack;
    public CombatState_wizard combatStanceState;
    [SerializeField] private float AttackTime = 10;

    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatonManager enemyAni)
    {
        enemyManager.isPurse(true);
        Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

        // if (enemyStats.isHit) return hitState;
        if (enemyManager.isPreformingAction) return combatStanceState;
        if (currentAttack != null)
        {
            if (distanceFromTarget < currentAttack.minimumDistanceNeededToAttack)
            {
                return this;
            }
            else if (distanceFromTarget < currentAttack.maximumDistanceNeededToAttack)
            {
                if(viewableAngle <= currentAttack.maximumAttackRange && viewableAngle>= currentAttack.minimumAttackRange)
                    if (enemyManager.currentRecoveryTime <= 0 && enemyManager.isPreformingAction == false)
                    {
                        enemyAni.PlayTargetAnimation(currentAttack.actionAnimation, true);
                        enemyManager.isPreformingAction = true;
                        enemyManager.currentRecoveryTime = AttackTime;
                        currentAttack = null;
                        return combatStanceState;
                    }
            }
        }
        else
        {
            GetNewAttack(enemyManager);
        }

        return combatStanceState;
    }

    

    private void GetNewAttack(EnemyManager enemyManager)
    {
        Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
        float distanceFromTarget =
            Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);
        int maxScore = 0;
        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAtion = enemyAttacks[i];

            if (distanceFromTarget >= enemyAttackAtion.minimumDistanceNeededToAttack &&
                distanceFromTarget <= enemyAttackAtion.maximumDistanceNeededToAttack)
            {
                if (viewableAngle <= enemyAttackAtion.maximumAttackRange &&
                    viewableAngle >= enemyAttackAtion.minimumAttackRange)
                {
                    maxScore += enemyAttackAtion.attackScore;
                }
            }
        }

        int randomValue = Random.Range(0, maxScore);
        int temporaryScore = 0;

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAtion = enemyAttacks[i];

            if (distanceFromTarget >= enemyAttackAtion.minimumDistanceNeededToAttack &&
                distanceFromTarget <= enemyAttackAtion.maximumDistanceNeededToAttack)
            {
                if (viewableAngle <= enemyAttackAtion.maximumAttackRange &&
                    viewableAngle >= enemyAttackAtion.minimumAttackRange)
                {
                    if (currentAttack != null) return;

                    temporaryScore += enemyAttackAtion.attackScore;

                    if (temporaryScore > randomValue)
                    {
                        currentAttack = enemyAttackAtion;
                    }
                }
            }
        }
    }
}
