using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class PatrolState : StateMachineBehaviour
{
    private float timer;

    private float chaseRange = 10.0f;

    List<Transform> wayPoints = new List<Transform>();

    private NavMeshAgent agent;

    private Transform enemy;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        enemy = GameObject.FindGameObjectWithTag("Enemy").transform;

        agent = animator.GetComponent<NavMeshAgent>();

        agent.speed = 1.5f;

        timer = 0.0f;

        GameObject go = GameObject.FindGameObjectWithTag("WayPoints");

        foreach (Transform t in go.transform)
        {
            wayPoints.Add(t);
        }

        agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
        }

        timer += Time.deltaTime;

        if (timer > 10.0f)
        {
            animator.SetBool("bMove", false);
        }

        float distance = Vector3.Distance(enemy.position, animator.transform.position);

        if (distance < chaseRange)
        {
            animator.SetBool("bChase", true);
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        agent.SetDestination(agent.transform.position);

    }
}
