using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class PlayerPartyMonster : MonoBehaviour
{

    private Animator anim;
    private NavMeshAgent nav;

    public Transform target;

    public bool bMove;
    public bool bAttack;

    public float chaseRange;
    public float attackRange;

    private string targetTag = "Enemy";

    private void Awake()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
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
            if(bMove)
            {
                nav.isStopped = false;
                anim.SetBool("bMove", true);
                nav.SetDestination(target.position);
            }else
            {
                anim.SetBool("bMove", false);
                nav.isStopped = true;
            }
        }

        float distance = Vector3.Distance(transform.position,
                target.transform.position);

        if (distance < attackRange)
        {
            TurnToTarget();
            nav.isStopped = true;
            bMove = false;
            anim.SetBool("bMove", false);
            anim.SetBool("bAttack", true);
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

        if (nearestEnemy != null && shortestDistance <= chaseRange)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }

    }

    public void TurnToTarget()
    {
        Vector3 to = new Vector3(target.position.x, 0, target.position.z);
        //내 위치
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);

        //곧바로 목표를 향해 돌기
        transform.rotation = Quaternion.LookRotation(to - from);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
