using System;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.AI;


public class EnemyManager : MonoBehaviour // baseEnemy
{
    public bool isPreformingAction;
    private EnemyLocomotionManager enemyLocomotionManager;
    private EnemyAnimatonManager enemyAni;
    private EnemyStats enemyStats;
    public NavMeshAgent navmeshAgent;
    public Rigidbody enemyRigid;
    [SerializeField] private Weapon weapon;


    public vThirdPersonController vThirdPersonController;

    public State currentState;
    public CharacterStats currentTarget;
    
    public float detectionRadius= 0;
    public float minimumDetectionAngle = 50;
    public float maximumDetectionAngle = -50;

    public float currentRecoveryTime = 3;
    
    public float rotationSpeed = 10f;
    public float maximumAttacRange = 1.5f;

    public bool isInteracting;
    public bool isWakeUp = false;
    private Animator ani;

    [SerializeField] private CanvasGroup canvasGroup;

    public bool isStun = false;

    [SerializeField] private bool isGizmo = true;
    
    private bool onDamage;
    
    
    
    private void Awake()
    {
        enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        enemyAni = GetComponent<EnemyAnimatonManager>();
        enemyStats = GetComponent<EnemyStats>();
        enemyRigid = GetComponent<Rigidbody>();
        navmeshAgent = GetComponent<NavMeshAgent>();
        weapon =GetComponentInChildren<Weapon>();
        vThirdPersonController = FindObjectOfType<vThirdPersonController>();
        ani = GetComponent<Animator>();
    }
    
    private void Start()
    {
        navmeshAgent.enabled = true;
        enemyRigid.isKinematic = true;
        canvasGroup =  transform.Find("HpCanvas").GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        isInteracting = enemyAni.ani.GetBool("isInteracting");
    }

    private void Update()
    {
        HandleRecoveryTimer();
        HandleStateMachine();
    }

    private void SwitchToNextState(State state) // 다른 State로 변경한다
    {
        currentState = state;
    }

    private void HandleStateMachine() //상태를 관리한다
    {
        if (enemyStats.isDead == true) return;
        if (currentState != null)
        {
            State nextState = currentState.Tick(this, enemyStats, enemyAni);
            if (nextState != null)
            {
                SwitchToNextState(nextState);
            }
        }
    }

    public void isPurse(bool a)
    {
        canvasGroup.alpha = 1;
        vThirdPersonController.isPursure = a;
    }

    public void EnableCol_weapon(int a)
    {
        weapon.EnableCol_animationEvent(a);
    }

    private void HandleRecoveryTimer()
    {
        if (currentRecoveryTime > 0)
        {
            currentRecoveryTime -= Time.deltaTime;
        }

        if (isPreformingAction)
        {
            if (currentRecoveryTime <= 0)
            {
                isPreformingAction = false;
            }
        }
    }

    public void WakeUp()
    {
        isWakeUp = true;
    }
    
    void OnDrawGizmos()
    {
        if (isGizmo)
        {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
            
        }
    }


    public void Parry()
    {
        isStun = true;  
    }

    // public void CallEnemyHitSound(int name)
    // {
    //     GameManager.instance.EnemySoundHit(name, transform);
    // }
    //
    // public void CallEnemyDieSound(int name)
    // {
    //     GameManager.instance.EnemySoundDie(name, transform);
    // }
    
    
}
