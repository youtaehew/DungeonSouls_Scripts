using System;
using UnityEngine;

public class EnemyAnimatonManager : AnimatorManager
{
    private EnemyManager enemyManager;
    private void Awake()
    {
        ani = GetComponentInParent<Animator>();
        enemyManager = GetComponentInParent<EnemyManager>();
    }
    
    
    
    
    
}
