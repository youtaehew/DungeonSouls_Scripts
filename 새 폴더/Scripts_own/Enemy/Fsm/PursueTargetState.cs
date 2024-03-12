using UnityEngine;

public class PursueTargetState : State
{
    public CombatStanceState combatStanceState;
    public HitState hitState;
    
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats,EnemyAnimatonManager enemyAni)
    {
        enemyManager.isPurse(true);
        if (enemyManager.isPreformingAction && enemyManager.navmeshAgent.enabled)
        {
            enemyAni.ani.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            enemyManager.navmeshAgent.isStopped = true;
            enemyManager.navmeshAgent.velocity = Vector3.zero;
            return this;
        }

        Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        // float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

        if (distanceFromTarget > enemyManager.maximumAttacRange && enemyManager.navmeshAgent.enabled)
        {
            enemyAni.ani.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            enemyManager.navmeshAgent.isStopped = false;
            enemyManager.navmeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
            enemyAni.ani.applyRootMotion = false;
        }

        HandleRotateTowardsTarget(enemyManager);
        // enemyManager.navmeshAgent.transform.localPosition = Vector3.zero;
        // enemyManager.navmeshAgent.transform.localRotation = Quaternion.identity;

        if (enemyStats.isHit)
        {
            return hitState;
        }
        if (distanceFromTarget <= enemyManager.maximumAttacRange)
        {
            return combatStanceState;
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
