using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState_wizard : State
{
    public LayerMask detectionLayer;
    public CombatState_wizard combatStateWizard;

    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatonManager enemyAni)
    {

        if (enemyStats.isHit) return this;
        else 
        {
            Collider[] colliders =
                Physics.OverlapSphere(transform.position, 10f, detectionLayer);
            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();
                if (characterStats != null)
                {
                    enemyManager.currentTarget = characterStats;
                }
            }
            return combatStateWizard;
        }
        
    }
}
