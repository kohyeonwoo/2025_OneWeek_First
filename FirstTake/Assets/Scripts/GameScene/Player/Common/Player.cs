using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public Transform target;
    public NavMeshAgent agent;
    public GameObject attackCollision;
    public GameObject particle;
    public Transform particleLocation;

    protected float maxHealth;
    protected float currentHealth;

    public Animator anim;
    public Rigidbody rigid;

    [SerializeField]
    private bool bMove;
    [SerializeField]
    private bool bAttack;

    private float timer;

    public float range;

    public Transform centerPoint;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();

        bMove = true;
        bAttack = false;

        //target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Attack()
    {
        this.transform.LookAt(target);
    }

    public void RandomMove()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 point;

            if (RandomPoint(centerPoint.position, range, out point))
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                agent.SetDestination(point);
                anim.SetBool("bMove", true);
                attackCollision.SetActive(true);
            }
            else
            {
                attackCollision.SetActive(false);
            }

        }
    }

    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;

    }

    public void Damage(float Damage)
    {
        currentHealth -= Damage;
        
        if(currentHealth <= 0)
        {
            Dead();
        }
    }

    public void Dead()
    {
        this.gameObject.SetActive(false);
    }

}
