using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class PlayerNewType : MonoBehaviour, IDamageable
{
   
    //FSM 타입, enum 세팅값
    public enum State
    {
        Idle, //서있음
        Run, //이동
        Attack, //공격
        Die //죽음
    }

    private Coroutine idleCoroutine;

    private Coroutine runCoroutine;

    private Coroutine attackCoroutine;

    private Coroutine dieCoroutine;

    private Animator animator;

    private NavMeshAgent nav;

    private Enemy_NewType targetEnemy;

    public float maxHealth = 100.0f;

    public float currentHealth;

    State theState;

    State state
    {
        get { return theState; }

        set
        {

            ExitState(theState);

            switch(value)
            {

                case State.Idle:
                    idleCoroutine = StartCoroutine(IdleCoroutine());
                    break;

                case State.Run:
                    runCoroutine = StartCoroutine(RunCoroutine());
                    break;

                case State.Attack:
                    attackCoroutine = StartCoroutine(AttackCoroutine());
                    break;

                case State.Die:
                    break;

            }
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        state = State.Idle;

        currentHealth = maxHealth;
    }

    private void ExitState(State State)
    {
        switch(State)
        {
            case State.Idle:
                if(idleCoroutine != null)
                {
                    StopCoroutine(IdleCoroutine());
                }
                break;

            case State.Run:
                if (runCoroutine != null)
                {
                    StopCoroutine(RunCoroutine());
                }
                break;

            case State.Attack:
                if (attackCoroutine != null)
                {
                    StopCoroutine(AttackCoroutine());
                }
                break;

            case State.Die:
                break;
        }
    }

    private IEnumerator IdleCoroutine()
    {
        
        //현재 Idle 애니메이션 작동
        animator.Play("Idle");

        //적을 찾을 때까지 대기

        while (true)
        {
            //가장 가까운 거리의 적을 찾는다
          
            Enemy_NewType nearestEnemy = GetNearestEnemy();          

            //적을 찾았다면 적을 추적한다
            if(nearestEnemy)
            {
                targetEnemy = nearestEnemy;
                break;
            }

            yield return null;
        }

        state = State.Run;
    }

    //적을 추적한다
    private IEnumerator RunCoroutine()
    {

        //움직임 애니메이션 작동
        animator.Play("Run");

        //적에게 이동한다
        nav.SetDestination(targetEnemy.transform.position);

        //적과의 거리가 0.1f보다 크다면 대기
        while(Vector3.Distance(this.transform.position,
            targetEnemy.transform.position) > 0.3f)
        {
            yield return null;
        }

        //현재 상태값을 공격값으로 변경
        state = State.Attack;
    }

    //공격 부분 
    private IEnumerator AttackCoroutine()
    {
        //공격 모션 작동
        animator.Play("Attack", 0, 0);

        //선 딜레이
        yield return new WaitForSeconds(0.3f);

        //적에게 피해 주기
        targetEnemy.Damage(40.0f);

        //후 딜레이
        yield return new WaitForSeconds(2.3f);

        //해당 적이 사망했다면
        if(targetEnemy.currentHealth < 0)
        {
            //다음 적을 찾는다
            Enemy_NewType nearestMonster = GetNearestEnemy();

            //찾았다면, 해당 적을 추적한다

            if(nearestMonster)
            {
                targetEnemy = nearestMonster;
                state = State.Run;
            }
            else
            {
                //적이 없다면 상태값을 --> Idle로 바꿔준다
                state = State.Idle;
            }

        }
        else
        {
            //아직 해당 적이 살아있다면 공격
            state = State.Attack;
        } 
    }

    //가장 근처의 적을 찾아주는 함수
    private Enemy_NewType GetNearestEnemy()
    {
        float nearestDistance = float.MaxValue;

        Enemy_NewType nearestEnemy = null;

        foreach (Enemy_NewType enemyNewType in Enemy_NewType.enemyNewTypeList)
        {

            //죽은 적은 제외하고 찾는다
            if(enemyNewType.currentHealth < 0)
            {
                continue;
            }

            float distance = Vector3.Distance(this.transform.position, enemyNewType.transform.position);

            if (distance < nearestDistance)
            {
                nearestEnemy = enemyNewType;
                nearestDistance = distance;
            }
        }

        return nearestEnemy;
    }

    //현재 플레이어와 목표를 그어주는 선 
    private void OnDrawGizmos()
    {
        if(Application.isPlayer)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(this.transform.position, nav.destination);
        }
    }

   
    public void Damage(float Damage)
    {
        currentHealth -= Damage;

        if (currentHealth <= 0)
        {

        }
    }
}
