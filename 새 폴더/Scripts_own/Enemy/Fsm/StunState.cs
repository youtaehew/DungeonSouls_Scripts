using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : State
{
    private float StunTime = 0;
    public CombatStanceState combatStanceState;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatonManager enemyAni)
    {
        StunTime += Time.deltaTime;
        if (StunTime >= 3f)
        {
            StunTime = 0;
            enemyManager.isStun = false;
            enemyAni.ani.SetBool("isParring", false);
            return combatStanceState;
        }
        else
        {
            return this;
        }
    }
}
