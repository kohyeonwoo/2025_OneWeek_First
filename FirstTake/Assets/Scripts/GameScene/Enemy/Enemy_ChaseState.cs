using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Enemy_ChaseState : StateMachineBehaviour
{

    private NavMeshAgent agent;

    private Transform player;
    

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player").transform.transform;

        agent.speed = 3.5f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(player.position);

        float distance = Vector3.Distance(player.position, animator.transform.position);

        if (distance > 15.0f)
        {
            animator.SetBool("bChase", false);
        }

        if (distance < 1.0f)
        {
            animator.SetBool("bAttack", true);
            Debug.Log("적이 플레이어 공격 범위에 들어섰습니다.");
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position);
    }

 
}
