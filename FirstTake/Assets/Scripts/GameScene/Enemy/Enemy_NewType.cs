using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Enemy_NewType : MonoBehaviour, IDamageable
{
    //FSM 타입, enum 세팅값
    public enum State
    {
        Idle, //서있음
        Run, //이동
        Attack, //공격
        OnDamage,//맞았을 경우
        Die //죽음
    }

    public static List<Enemy_NewType> enemyNewTypeList = new List<Enemy_NewType>();
    
    private Coroutine idleCoroutine;

    private Coroutine runCoroutine;

    private Coroutine attackCoroutine;

    private Coroutine onDamageCoroutine;

    private Coroutine dieCoroutine;

    private Animator animator;

    public float maxHealth = 100;

    public float currentHealth;

    State theState;

    State state
    {
        get { return theState; }

        set
        {

            ExitState(theState);

            switch (value)
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

                case State.OnDamage:
                    onDamageCoroutine = StartCoroutine(OnDamageCoroutine());
                    break;

                case State.Die:
                    dieCoroutine = StartCoroutine(DieCoroutine());
                    break;
            }
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();

        enemyNewTypeList.Add(this);
    }

    private void Start()
    {
        currentHealth = maxHealth;    
    }

    private void PlayAnimation(string clipName)
    {
        animator.Play(clipName, 0, 0);
    }

    private void ExitState(State State)
    {
        switch (State)
        {
            case State.Idle:
                if (idleCoroutine != null)
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

            case State.OnDamage:
                if (onDamageCoroutine != null)
                {
                    StopCoroutine(OnDamageCoroutine());
                }
                break;

            case State.Die:
                if (dieCoroutine != null)
                {
                    StopCoroutine(DieCoroutine());
                }
                break;
        }
    }

    private IEnumerator IdleCoroutine()
    {

        //현재 Idle 애니메이션 작동
        animator.Play("Idle");

        //목표를 찾을 때까지 대기

        while (true)
        {

            //목표를 찾는다

            //목표를 찾았다면 적을 추적한다

            yield return null;
        }

    }

    //목표를 추적한다
    private IEnumerator RunCoroutine()
    {

        //움직임 애니메이션 작동
        animator.Play("Run");

        //목표에게 이동하기 전까진 대기
        yield return null;

        //현재 상태값을 공격값으로 변경
        state = State.Attack;
    }

    //공격 부분 
    private IEnumerator AttackCoroutine()
    {
        //공격 모션 작동
        animator.Play("Attack",0,0);

        yield return null;

        //목표에게 피해 주기

        //해당 목표가 사망했다면, 다음 목표를 찾는다

        //목표가 없다면 상태값을 --> Idle로 바꿔준다
    }

    //데미지 입을 경우의 Coroutine
    private IEnumerator OnDamageCoroutine()
    {
        //피해 입을 경우 해당 애니메이션 출력 
        //animator.Play("OnDamage");
        PlayAnimation("OnDamage");

        //해당 애니메이션 실행될 동안 대기
        yield return new WaitForSeconds(1.0f);

        //상태 변경 --> 플레이어 공격 
        
    }

    //사망할 경우의 Coroutine
    private IEnumerator DieCoroutine()
    {
        enemyNewTypeList.Remove(this);
        animator.Play("Die");
        yield return new WaitForSeconds(1.0f);

        //Destroy(this.gameObject);
        this.gameObject.SetActive(false);
    }

    public void Damage(float Damage)
    {
        currentHealth -= Damage;

        //현재 HP가 0보다 크다면 
        if(currentHealth > 0)
        {
            state = State.OnDamage;
        }
        else
        {
            state = State.Die;
        }
    }
}
