using DG.Tweening;
using Invector.vCharacterController;
using UnityEngine;

public class EnemyBackSteb : MonoBehaviour
{
    private Vector3 targetForward;
    private Vector3 moveToPos;
    private Animator ani;
    private Collider col;
    [SerializeField]
    private vThirdPersonController vThirdPersonController;
    private EnemyStats enemyStats;


    private void Awake()
    {
        enemyStats = GetComponent<EnemyStats>();
        targetForward = transform.forward;
        moveToPos = transform.position;
    }

    private void Start()
    {
        ani = GetComponent<Animator>();
        col = GetComponent<Collider>();
    }




    public void PlayBackstabAnimation()
    {
        ani.applyRootMotion = true;
        ani.CrossFadeInFixedTime("backStabHit", 0.1f);
        enemyStats.TakeDamage(500, true);
        col.enabled = false;
    }
    public void PlayFrontstabAnimation()
    {
        ani.applyRootMotion = true;
        ani.CrossFadeInFixedTime("frontStabHit", 0.1f);
        enemyStats.TakeDamage(500, true);
        col.enabled = false;
    }

    public void SetTargetForward(Vector3 TargetForward)
    {
        this.targetForward = TargetForward;
    }
    public void SetTargetBack(Vector3 TargetForward)
    {
        this.targetForward = -TargetForward;
    }

    public void ForceMoveToPos(Vector3 pos)
    {
        this.moveToPos = pos;
    }
    
    

    public void HandleSmoothForwardRotation()
    {
        // transform.DOMove(targetForward, 10);
        DOTween.To(() => transform.forward, x => transform.forward = x, targetForward, .3f);
    }

    public void HandleSmoothForwardMovement()
    {
        transform.DOMove(moveToPos, .3f);
    }

    public void EndBackAttack()
    {
        ani.enabled = false;
    }
}