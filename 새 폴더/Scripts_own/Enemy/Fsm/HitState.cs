using UnityEngine;

public class HitState : State
{
    public LayerMask detectionLayer;
    public PursueTargetState pursueTargetState;

    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatonManager enemyAni)
    {

        if (enemyStats.isHit) return this;
        else 
        {
            Collider[] colliders =
                Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);
            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();
                if (characterStats != null)
                {
                    enemyManager.currentTarget = characterStats;
                }
            }
            return pursueTargetState;
        }
        
    }
}