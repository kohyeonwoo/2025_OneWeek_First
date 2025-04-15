using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{

    private Transform enemy;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        enemy = GameObject.FindGameObjectWithTag("Enemy").transform;

        animator.transform.LookAt(enemy);

        float distance = Vector3.Distance(enemy.position, animator.transform.position);

        if (distance > 3.5f || enemy == null)
        {
            animator.SetBool("bAttack", false);
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}
