using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStanceState : State
{
    public AttackState attackState;
    public PursueTargetState pursueTargetState;
    public HitState hitState;
    public StunState stunState;
    
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats,EnemyAnimatonManager enemyAni)
    {
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position,
            enemyManager.transform.position);

        // if (enemyStats.isHit) return hitState;
        if (enemyManager.isPreformingAction)
        {
            enemyAni.ani.SetFloat("Vertical", 0,0.1f, Time.deltaTime);
        }
        
        
        if (enemyStats.isHit) return hitState;
        if (enemyManager.isStun) return stunState;
        if (enemyManager.currentRecoveryTime <= 0 && distanceFromTarget <= enemyManager.maximumAttacRange)
        {
            return attackState;
        }
        else if (distanceFromTarget > enemyManager.maximumAttacRange)
        {
            return pursueTargetState;
        }
        else
        {
            return this;
        }
    }
}
