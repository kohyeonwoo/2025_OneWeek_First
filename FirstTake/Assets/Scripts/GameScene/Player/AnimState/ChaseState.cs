using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class ChaseState : StateMachineBehaviour
{

    private NavMeshAgent agent;

    private Transform enemy;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();

        enemy = GameObject.FindGameObjectWithTag("Enemy").transform.transform;

        agent.speed = 3.5f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(enemy.position);

        float distance = Vector3.Distance(enemy.position, animator.transform.position);

        if (distance > 15.0f)
        {
            animator.SetBool("bChase", false);
        }

        if (distance < 2.5f)
        {
            animator.SetBool("bAttack", true);
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position);
    }

}
