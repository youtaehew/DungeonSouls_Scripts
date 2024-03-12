using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
using Invector.vCharacterController;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class EnemyStats : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    private Animator ani;
    [SerializeField] private HpBar hp;
    public bool isHit = false;
    private EnemyManager enemyManager;
    private vThirdPersonController controller;
    private Rigidbody rigidbody;

    [SerializeField] private GameObject BloodFX;
    [SerializeField] private Collider[] dead;
    [SerializeField] private List<Behaviour> DeadComponent;
    [SerializeField] private GameObject bloodPos;
    [SerializeField] private SkinnedMeshRenderer mat;

    private float nockBack = 0.6f;

    public bool isDead = false;
    public bool enemyGather = false;

    [SerializeField] private bool isWizard = false;
    [SerializeField] private AudioClip EnemeyDieClip;

    private RaycastHit raycastHit;
    private float rayDistance = 0.6f;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        enemyManager = GetComponent<EnemyManager>();
        currentHealth = maxHealth;
        mat = GetComponentInChildren<SkinnedMeshRenderer>();
        controller = GameObject.Find("Player").GetComponent<vThirdPersonController>();
    }
    

    private void FixedUpdate()
    {
        Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), -transform.forward,
            out raycastHit, rayDistance);
    }


    public void TakeDamage(int damage, bool isBack)
    {
        StartCoroutine(StopTime(0.01f));
        isHit = true;
        if (isWizard == false)
        {
            enemyManager.EnableCol_weapon(0);
        }

        currentHealth -= damage;
        hp.updateHp(currentHealth, maxHealth);

        if (isBack == false)
        {
            StartCoroutine(HitRed());
            ani.SetTrigger("Hit");
            GameManager.instance.EnemyHit();
            if (controller.isPursure && isWizard == false)
            {
                if (raycastHit.transform == null || !raycastHit.transform.CompareTag("Environment"))
                {
                    transform.DOMove(transform.position - transform.forward * nockBack, 0.1f);
                }
            }
        }

        if (currentHealth <= 0)
        {
            if (!isWizard && isBack == false)
            { 
                EnemySound.instance.EnemyDieSound();
            }
            StartCoroutine(StopTime(0.1f));
            isDead = true;
            currentHealth = 0;
            enemyManager.isPurse(false);
            enemyManager.navmeshAgent.enabled = false;
            for (int i = 0; i < dead.Length; i++)
            {
                dead[i].enabled = false;
            }

            for (int i = 0; i < DeadComponent.Count; i++)
            {
                DeadComponent[i].enabled = false;
            }

            if (isBack == false)
            {
                Blood();
                GameManager.instance.EnemyKillSound();
                ani.SetBool("isDead", true);
            }

            enemyManager.enabled = false;
            GameManager.instance.EnemyCheck();
        }

        StartCoroutine("EnemyHelp");
    }


    private void Blood()
    {
        Instantiate(BloodFX, bloodPos.transform.position, Quaternion.identity);
    }

    IEnumerator HitRed()
    {
        mat.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        mat.material.color = Color.white;
    }


    IEnumerator EnemyHelp()
    {
        yield return new WaitForSeconds(1f);
        enemyGather = true;
    }

    IEnumerator StopTime(float time)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1;
    }

    public void HitEnd()
    {
        isHit = false;
    }
}