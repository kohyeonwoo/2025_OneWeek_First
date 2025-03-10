using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent agent;
    private Animator anim;
   // private Rigidbody rigid;

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
     //   rigid = GetComponent<Rigidbody>();

        bMove = true;
        bAttack = false;

        //target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {

        if (bAttack == false)
        {
            RandomMove();
        }

        //if (bMove == false)
        //{
        //    Attack();
        //}


        //if (target != null)
        //{

        //}

    }

    private void Attack()
    {
        this.transform.LookAt(target);
    }

    private void RandomMove()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 point;

            if (RandomPoint(centerPoint.position, range, out point))
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                agent.SetDestination(point);
                anim.SetBool("bMove", true);
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

}
