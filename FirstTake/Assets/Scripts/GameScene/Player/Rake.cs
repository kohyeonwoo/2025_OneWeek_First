using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using TMPro;

public class Rake : MonoBehaviour
{

    public Transform target;

    public NavMeshAgent nav;
    public Animator anim;
    public Rigidbody rigid;
    public SkinnedMeshRenderer[] meshes;

    public int maxHealth;
    private int currentHealth;

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

    public TextMeshProUGUI rakeHealth;

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        meshes = GetComponentsInChildren<SkinnedMeshRenderer>();
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

            float distanceToEnemy = Vector3.Distance(transform.position,
                target.transform.position);

            if (distanceToEnemy < attackRange)
            { //목표 위치
                Vector3 to = new Vector3(target.position.x, 0, target.position.z);
                //내 위치
                Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);

                //바로 돌기
                transform.rotation = Quaternion.LookRotation(to - from);
                nav.isStopped = true;
                Debug.Log("적이 공격 범위 내에 왔습니다");
                anim.SetTrigger("Attack1");
            }
            else
            {
               // anim.SetBool("bAttack", false);
                nav.isStopped = false;
                nav.SetDestination(target.position);
            }


        }

        rakeHealth.text = currentHealth.ToString();

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, attackRange);       
    }

}
