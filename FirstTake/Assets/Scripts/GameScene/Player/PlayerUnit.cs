using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class PlayerUnit : MonoBehaviour, IDamageable
{

    public float maxHealth;
    public float currentHealth;
    public GameObject attackCollision1;
    public Transform target;
    public NavMeshAgent nav;
    public Animator anim;
    public SkinnedMeshRenderer[] meshes;

    public bool bAttack;

    public float distanceToEnemy;

    [SerializeField]
    private float range = 7.5f;

    [SerializeField]
    private string targetTag = "Enemy";

    [SerializeField]
    private float attackRange = 1.0f;

    [SerializeField]
    private float attackRate = 1.0f;

    [SerializeField]
    private float attackCoolDown = 0.0f;


    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        meshes = GetComponentsInChildren<SkinnedMeshRenderer>();
        nav.updateRotation = false;
        bAttack = false;
    }

    private void Start()
    {
        maxHealth = 25;
        currentHealth = maxHealth;
    }

    private void Update()
    {

        UpdateTarget();

        if (target == null)
        {
            return;
        }
        else
        {
            anim.SetBool("bMove", false);


            distanceToEnemy = Vector3.Distance(transform.position,
            target.transform.position);

            if (distanceToEnemy < attackRange)
            { //목표 위치
                Vector3 to = new Vector3(target.position.x, 0, target.position.z);
                //내 위치
                Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);

                //곧바로 목표를 향해 돌기
                transform.rotation = Quaternion.LookRotation(to - from);

                ////천천히 목표를 향해 돌기
                //Quaternion rotation = Quaternion.LookRotation(to - from);
                //transform.rotation = Quaternion.Slerp(transform.rotation, RotationDriveMode, 0.01f);

                nav.isStopped = true;
                Debug.Log("적이 공격 범위 내에 왔습니다");

                if (attackCoolDown <= 0.0f)
                {
                    //anim.SetTrigger("Attack1");
                    anim.SetBool("bAttack", true);
                    attackCoolDown = 1.0f / attackRate;
                }

                attackCoolDown -= Time.deltaTime;


            }
            else
            {
                anim.SetBool("bAttack", false);
                nav.isStopped = false;
                nav.SetDestination(target.position);
                anim.SetBool("bMove", true);
            }



        }

    }

    private void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag);

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

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }

    }

    public void ActiveAttackCollision()
    {
        attackCollision1.SetActive(true);
    }

    public void DeActiveAttackCollision()
    {
        attackCollision1.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void Damage(float Damage)
    {
        currentHealth -= Damage;

        if(currentHealth <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        this.gameObject.SetActive(false);
    }
}
