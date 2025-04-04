using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState_wizard : State
{
    public IdleState_wizard idleStateWizard;
    public AttackState_wizard attackStateWizard;
    public HitState_wizard hitState;
    
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats,EnemyAnimatonManager enemyAni)
    {
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position,
            enemyManager.transform.position);

        HandleRotateTowardsTarget(enemyManager);

        if (enemyStats.isHit) return hitState;
        if (enemyManager.currentRecoveryTime <= 0 && distanceFromTarget <= 10f)//enemyManager.maximumAttacRange)
        {
            return attackStateWizard;
        }
        else if (distanceFromTarget > 10f)//enemyManager.maximumAttacRange)
        {
            return idleStateWizard;
        }
        else
        {
            return this;
        }
    }
    
    private void HandleRotateTowardsTarget(EnemyManager enemyManager)
    {
        Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
        direction.y = 0;
        direction.Normalize();

        if (direction == Vector3.zero)
        {
            direction = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
            enemyManager.rotationSpeed / Time.deltaTime);
        

    }
}
