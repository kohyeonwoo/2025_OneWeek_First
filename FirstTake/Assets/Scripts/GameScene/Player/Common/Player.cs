using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public enum CurrentState
{
    None = -1, 
    Idle = 0,
    Wander,
    Pursuit,
    Attack
}

public class Player : MonoBehaviour, IDamageable
{
    //현재 움직임 상태값 관련 
    public CurrentState currentState = CurrentState.None;

    //타겟 관련 
    public Transform target;

    // 컴포넌트 관련
    public NavMeshAgent nav;
    public Animator anim;
    public Rigidbody rigid;
    public SkinnedMeshRenderer[] meshes;

    //공격 충돌체 관련 
    public GameObject attackCollision;
    public GameObject attackCollision2;
    public GameObject attackCollision3;

    //체력 관련
    protected float maxHealth;
    protected float currentHealth;

    //움직임 관련 
    protected float walkSpeed;
    protected float runSpeed;

    //추격 관련 
    [SerializeField]
    private float targetRange = 5.0f; //타겟 인식 범위
    [SerializeField]
    private float chaseLimitRange = 7.0f; //추격 범위

    //공격 수치 관련 
    [SerializeField]
    private float attackRange = 3.0f; //==> 공격 범위
    [SerializeField]
    private float attackSpeed = 1.0f; //==> 공격 속도
    public float lastAttackTime = 2.0f; // 공격 주기 계산용 변수

    //플레이어 생사 유무 관련 
    private bool bDead;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {

    }


    //컴포넌트 / 기타 설정 관련 초기화 부분 
    public void Init()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        meshes = GetComponentsInChildren<SkinnedMeshRenderer>();
        nav.updateRotation = false;
    }

    //움직임 상태값 변경 관련 
    public void ChangeState(CurrentState NewState)
    {
        if(currentState == NewState)
        {
            return;
        }

        StopCoroutine(currentState.ToString());

        currentState = NewState;

        StartCoroutine(currentState.ToString());
    }

    //대기 --> 배회로 자동 상태 변환
    private IEnumerator AutoChangeFromIdleToWander()
    {
        //1~4초 사이에서 무작위로 대기
        int changeTime = Random.Range(1, 5);

        yield return new WaitForSeconds(changeTime);

        //상태를 배회로 변경
        ChangeState(CurrentState.Wander);
    }

    //배회 위치값 계산 
    private Vector3 CalculateWanderPosition()
    {
        float wanderRadius = 10.0f; //현재 위치를 원점으로 하는 원의 반지름
        int wanderJitter = 0; //선택된 각도 (wanderJitterMin ~ wanderJitterMax)
        int wanderJitterMin = 0;
        int wanderJitterMax = 360;

        //현재 캐릭터가 있는 월드의 중심 위치와 크기 (구역을 벗어난 행동을 하지 말도록 하기 위해서)
        Vector3 rangePosition = Vector3.zero;
        Vector3 rangeScale = Vector3.one * 100.0f;

        //자신의 위치를 중심으로 반지름(wanderRadius) 거리, 선택된 각도(wanderJitter)에 위치한 좌표를 목표 지점으로 설정
        wanderJitter = Random.Range(wanderJitterMin, wanderJitterMax);
        Vector3 targetPosition = transform.position + SetAngle(wanderRadius, wanderJitter);

        //생성된 목표위치가 자신의 구역을 벗어나지 않도록 조절 
        targetPosition.x = Mathf.Clamp(targetPosition.x, rangePosition.x - rangeScale.x * 0.5f, rangePosition.x + rangeScale.x * 0.5f);
        targetPosition.y = 0.0f;
        targetPosition.z = Mathf.Clamp(targetPosition.z, rangePosition.z - rangeScale.z * 0.5f, rangePosition.z + rangeScale.z * 0.5f);

        return targetPosition;
    }


    //각도값 설정 
    private Vector3 SetAngle(float radius, int angle)
    {
        Vector3 position = Vector3.zero;

        position.x = Mathf.Cos(angle) * radius;
        position.z = Mathf.Sin(angle) * radius;

        return position;
    }

    //기본 서있는 상태

    private IEnumerator Idle()
    {
        //일정 초가 지난 후 배회 상태로 변환되는 코루틴 실행
        StartCoroutine("AutoChangeFromIdleToWander");

        while(true)
        {
            //대기 상황인 경우, 타겟과의 거리에 따라 행동 선택(배회, 추격, 공격)
           

            yield return null;
        }
    }

   
    private IEnumerator Wander()
    {
        float currentTime = 0.0f;
        float maxTime = 10.0f;

        //이동 속도 설정, 배회 --> 걷는 속도, 추격 --> 뛰는 속도
        nav.speed = walkSpeed;
        
        //목표 위치 설정
        nav.SetDestination(target.position);

        //목표 위치로 회전
        Vector3 to = new Vector3(nav.destination.x, 0, nav.destination.z);
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);
        transform.rotation = Quaternion.LookRotation(to - from);

        anim.SetBool("bMove", true);
        anim.SetBool("bAttack", false);

        while(true)
        {
            currentTime += Time.deltaTime;

            //목표 위치에 근접하거나 배회하는 시간이 많이 길어진다면 
            to = new Vector3(nav.destination.x, 0, nav.destination.z);
            from = new Vector3(transform.position.x, 0, transform.position.z);

            if((to - from).sqrMagnitude < 0.01f || currentTime >= maxTime)
            {
                //상태 --> 대기로 변경
                ChangeState(CurrentState.Idle);
            }

            //타겟과의 거리에 따라 행동을 선택한다(배회, 추격, 공격 )

            yield return null;

        }
    }


    ////////////////////////////////////
    ///// 공격 충돌체 활성화/비활성화 부분
    public void ActiveAttackCollision()
    {
        attackCollision.SetActive(true);
    }

    public void DeActiveAttackCollision()
    {
        attackCollision.SetActive(false);
    }

    public void ActiveAttackCollision2()
    {
        attackCollision2.SetActive(true);
    }

    public void DeActiveAttackCollision2()
    {
        attackCollision2.SetActive(false);
    }

    public void ActiveAttackCollision3()
    {
        attackCollision3.SetActive(true);
    }

    public void DeActiveAttackCollision3()
    {
        attackCollision3.SetActive(false);
    }
    ////////////////////////////////////
    ////////////////////////////////////

    public void Damage(float Damage)
    {

        currentHealth -= Damage;

        StartCoroutine(ChangeColor());

        AudioManager.Instance.PlaySFX("PlayerHitSound");
     
        GameManager.Instance.PlayHitEffect();

        if (currentHealth <= 0)
        {
            Dead();
        }

    }

    IEnumerator ChangeColor()
    {

        foreach (SkinnedMeshRenderer mesh in meshes)
        {
            mesh.material.color = Color.red;
        }

        yield return new WaitForSeconds(0.3f);

        foreach (SkinnedMeshRenderer mesh in meshes)
        {
            mesh.material.color = Color.white;
        }

    }

    public void Dead()
    {
         this.gameObject.SetActive(false);
       
        GameManager.Instance.SetActiveEndPanel();
    }

}
