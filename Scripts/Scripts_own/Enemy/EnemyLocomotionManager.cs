using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class EnemyLocomotionManager : MonoBehaviour
{ 
    
    
    
    private EnemyManager enemyManger;
    private EnemyAnimatonManager enemyAnimatonManager;

    
    public LayerMask detectionLayer;
    public Rigidbody enemyRigid;

    

    
    private void Awake()
    {
        enemyManger = GetComponent<EnemyManager>();
        enemyAnimatonManager = GetComponentInChildren<EnemyAnimatonManager>();
      
        enemyRigid = GetComponent<Rigidbody>();
    }


    
}
