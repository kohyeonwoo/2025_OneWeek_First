using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public enum UnitType { Basic, Dash, LongDistance, Boss }

public class PlayerUnit : MonoBehaviour, IDamageable
{

    public UnitType unitType;

    public Rigidbody rigid;
    public Animator anim;
    public SkinnedMeshRenderer[] meshes;
    public NavMeshAgent nav;
    public BoxCollider meleeArea;
    private CapsuleCollider capsuleCollider;

    public int maxHealth;
    public int currentHealth;
    public int attackPoint;

    float targetRadius = 0.0f;
    float targetRange = 0.0f;

    public bool bChase;
    public bool bAttack;

    public Transform target;
    public GameObject particleEffect;
    public GameObject bullet;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        meshes = GetComponentsInChildren<SkinnedMeshRenderer>();

        currentHealth = maxHealth;

        if (unitType != UnitType.Boss)
        {
            Invoke("ChaseStart", 1.0f);
        }


    }

    private void Update()
    {
        UpdateTarget();
        ChaseTarget();
    }

    private void FixedUpdate()
    { 
        Targeting();
        FreezeVelocity();
    }

    public void ChaseStart()
    {
        bChase = true;
        anim.SetBool("bMove", true);
    }

    public void ChaseTarget()
    {
        if (nav.enabled && unitType != UnitType.Boss)
        {
            nav.SetDestination(target.position);
            nav.isStopped = !bChase;
        }
    }

    protected void FreezeVelocity()
    {
        if (bChase)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    protected void Targeting()
    {
        //float targetRadius = 0.0f;
        //float targetRange = 0.0f;

        if (unitType != UnitType.Boss)
        {
            switch (unitType)
            {
                case UnitType.Basic:
                    targetRadius = 1.5f;
                    targetRange = 3.0f;
                    break;
                case UnitType.Dash:
                    targetRadius = 0.5f;
                    targetRange = 12.0f;
                    break;
                case UnitType.LongDistance:
                    targetRadius = 0.5f;
                    targetRange = 25.0f;
                    break;
            }

            RaycastHit[] rayHits = Physics.SphereCastAll(transform.position,
                targetRadius,
                transform.forward,
                targetRange,
                LayerMask.GetMask("Enemy"));

            if (rayHits.Length > 0 && !bAttack)
            {
                StartCoroutine(Attack());
            }
        }


    }

    private void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float shortestDistance = Mathf.Infinity; //�� ���� �����Ǿ� ���� ���� ���

        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position,
                enemy.transform.position);

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;

                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= targetRange)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }

    }

    private IEnumerator Attack()
    {
        bChase = false;
        bAttack = true;
        anim.SetBool("bAttack", true);
      
        switch (unitType)
        {
            case UnitType.Basic:

                yield return new WaitForSeconds(0.2f);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(1.0f);
                meleeArea.enabled = false;

                yield return new WaitForSeconds(1.0f);

                break;

            case UnitType.Dash:

                yield return new WaitForSeconds(0.1f);
                rigid.AddForce(this.transform.forward * 20, ForceMode.Impulse);

                yield return new WaitForSeconds(0.5f);
                rigid.velocity = Vector3.zero;

                yield return new WaitForSeconds(2.0f);

                break;

            case UnitType.LongDistance:

                yield return new WaitForSeconds(0.5f);
                //총알 생성 부분 

                break;
        }



        bChase = true;
        bAttack = false;
        anim.SetBool("bAttack", false);
    }

    IEnumerator ChangeColor()
    {

        foreach (SkinnedMeshRenderer mesh in meshes)
        {
            mesh.material.color = Color.red;
        }

        yield return new WaitForSeconds(0.3f);

        foreach (SkinnedMeshRenderer mesh in meshes)
        {
            mesh.material.color = Color.white;
        }
    }

    private void Dead()
    {
        bChase = false;
        nav.enabled = false;
        anim.SetTrigger("Die");
        Invoke("EraseBody", 2.0f);
        //this.gameObject.SetActive(false);
    }

    private void EraseBody()
    {
        this.gameObject.SetActive(false);
    }

    public void Damage(float Damage)
    {
        currentHealth -= (int)Damage;
        StartCoroutine(ChangeColor());
        AudioManager.Instance.PlaySFX("PlayerHitSound");
        GameObject obj = Instantiate(particleEffect, transform.position, Quaternion.identity);
        Destroy(obj, 2.0f);

        if (currentHealth <= 0 && unitType != UnitType.Boss)
        {
            Dead();
        }
    }
}
