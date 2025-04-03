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
        ChangeState(CurrentState.Idle);   
    }

    private void OnDisable()
    {
        StopCoroutine(currentState.ToString());
        currentState = CurrentState.None;
    }


    private void Awake()
    {
        Init();
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
            CalculateDistanceToTargetAndSelectState();

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
            CalculateDistanceToTargetAndSelectState();

            yield return null;

        }
    }

    //추격 부분 
    private IEnumerator Pursuit()
    {
        while (true)
        {
            //이동 속도 설정(배회할 때는 걷는 속도로 이동, 추적할 때는 뛰는 속도로 이동)
            nav.speed = runSpeed;

            //목표 위치를 현재 플레이어의 위치로 설정
            nav.SetDestination(target.position);

            //타겟 방향을 계속 주시하도록 함
            LookRotationToTarget();

            anim.SetBool("bAttack", false);
            anim.SetBool("bMove", true);

            //타겟과의 거리에 따라 행동 선택(배회, 추격, 원거리 혹은 근거리 공격)
            CalculateDistanceToTargetAndSelectState();

            yield return null;
        }
    }

    //공격
    private IEnumerator Attack()
    {
        //공격 시 이동을 멈추도록 설정
        nav.ResetPath();

        while (true)
        {
            //타겟 방향 주시
            LookRotationToTarget();

            //타겟과의 거리에 따라 행동 선택(배회, 추격, 원거리 혹은 근거리 공격)
            CalculateDistanceToTargetAndSelectState();

            if (Time.time - lastAttackTime > attackRange)
            {
                //공격 주기가 되어야 공격할 수 있도록 하기 위해 현재 시간 저장
                lastAttackTime = Time.time;
                anim.SetBool("bAttack", true);
                anim.SetBool("bMove", false);
                //발사체 생성
                // GameObject clone = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
                // clone.GetComponent<EnemyProjectile>().SetUp(target.position);
            }
            yield return null;
        }
    }

    //목표를 바라보게 하는 부분 
    private void LookRotationToTarget()
    {
        //목표 위치
        Vector3 to = new Vector3(target.position.x, 0, target.position.z);
        //내 위치
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);

        //바로 돌기
        transform.rotation = Quaternion.LookRotation(to - from);
        //서서히 돌기
        // Quaternion rotation = Quaternion.LookRotation(to - from);
        //transform.rotation = Quaternion.Slerp(transform.rotation, RotationDriveMode, 0.01f); 
    }

    //타겟과의 거리 계산해 행동 바꿔주는 부분 
    private void CalculateDistanceToTargetAndSelectState()
    {
        if (target == null) { return; }

        //플레이어(Target)와 적의 거리 계산 후 거리에 따라 행동 선택
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= attackRange)
        {
            ChangeState(CurrentState.Attack);
        }
        else if (distance <= targetRange)
        {
            ChangeState(CurrentState.Pursuit);
        }
        else if (distance >= chaseLimitRange)
        {
            ChangeState(CurrentState.Wander);
        }
    }

    //움직임 관련 범위를 그려주는 기즈모 

    private void OnDrawGizmos()
    {
        //"배회" 상태일 때 이동할 경로 표시
        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, nav.destination - transform.position);

        //목표 인식 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetRange);

        //추적 범위
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseLimitRange);

        //공격 범위
        Gizmos.color = new Color(0.39f, 0.04f, 0.04f);
        Gizmos.DrawWireSphere(transform.position, attackRange);
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
