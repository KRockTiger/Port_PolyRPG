using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyAnimation enemyAnimation;
    private CharacterController controller;

    [Header("기본 설정")]
    private float turnSpeed; //회전 속도를 담을 변수
    private float gravity = -9.81f; //중력값
    private Vector3 velocity; //중력 적용 변수
    [SerializeField] private bool isChase; //추적 상태
    [SerializeField] private bool isGround; //추적 상태
    [SerializeField] private bool isHitting; //피격 상태 ==> 피격 중일 경우 약간의 그로기 시간을 가짐
    [SerializeField] private bool isAttacking; //전투 상태
    [SerializeField] private bool isMoving; //이동 상태
    [SerializeField] private Transform groundChecker;
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundMask;

    [Header("캐릭터 설정")]
    [SerializeField] private float setHP; //설정할 체력
    [SerializeField] private float curHP; //현재 체력
    [SerializeField] private float moveSpeed; //이동 속도
    [SerializeField] private float startSearchRange; //탐색 범위
    [SerializeField] private float endSearchRange; //탐색 중단 범위
    [SerializeField] private float attackRange; //공격 범위

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Attack")
        {
            Hit();
            enemyAnimation.P_SetTrigger_Hit();
        }
    }

    private void Awake()
    {
        enemyAnimation = GetComponent<EnemyAnimation>();
        controller = GetComponent<CharacterController>();
        curHP = setHP; //현재 체력 설정
    }

    private void Update()
    {
        Gravity();
        SearchPlayer();
        Attacking();
    }

    private void Gravity()
    {
        isGround = Physics.CheckSphere(groundChecker.position, groundDistance, groundMask);

        if (isGround)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    /// <summary>
    /// 플레이어 탐색
    /// </summary>
    private void SearchPlayer()
    {
        if (isHitting) { return; } //피격중일 경우 리턴

        GameObject player = GameObject.FindGameObjectWithTag("Player"); //플레이어 탐색

        //플레이어와의 거리 측정
        float distance = Vector3.Distance(player.transform.position, transform.position);
    
        if (distance <= startSearchRange) //몬스터의 탐색 범위보다 거리가 가깝다면
        {
            isChase = true; //추적 상태 키기

            if (distance <= attackRange) //공격 범위보다 가깝다면
            {
                isAttacking = true; //전투 상태 전환
            }

            else //공격 범위보다 멀면
            {
                isAttacking = false; //비전투 상태 전환
            }
        }

        else if (distance >= endSearchRange) //몬스터의 탐색 중단 범위 보다 거리가 멀면
        {
            isChase = false; //추적 상태 끄기
        }

        ChasePlayer(player);
    }

    /// <summary>
    /// 플레이어 추적
    /// </summary>
    private void ChasePlayer(GameObject _objPlayer)
    {
        if (!isChase) { return; } //추적 상태가 아닐경우 리턴

        //방향 설정
        Vector3 direction = (_objPlayer.transform.position - transform.position).normalized;

        //설정한 방향을 토대로의 회전값 설정
        Quaternion angle = Quaternion.LookRotation(direction);

        //위 설정한 회전값의 y값만 가져와서 회전을 부드럽게 할 수 있도록 설정
        float smoothTurnY = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, angle.eulerAngles.y, ref turnSpeed, 0.1f);

        //회전 실행
        transform.rotation = Quaternion.Euler(0f, smoothTurnY, 0f);

        //오브젝트의 정방향 설정
        Vector3 targetDirection = Quaternion.Euler(0f, angle.eulerAngles.y, 0f) * Vector3.forward;

        if (isAttacking) { return; } //전투 상태일 경우 멈춤

        //오브젝트 이동
        controller.Move(targetDirection * moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 전투 상태로 전환
    /// </summary>
    private void Attacking()
    {
        if (!isAttacking) { return; }


    }

    /// <summary>
    /// 피격 판정
    /// </summary>
    private void Hit()
    {
        Debug.Log("맞았습니다!");
        StartCoroutine(C_SetIsHitting());
    }

    /// <summary>
    /// 피격 후 1초동안 그로기 상태를 가짐
    /// </summary>
    /// <returns></returns>
    private IEnumerator C_SetIsHitting()
    {
        isHitting = true;
        yield return new WaitForSeconds(1f);
        isHitting = false;
    }

    public bool P_GetIsChase()
    {
        return isChase;
    }

    public bool P_GetIsAttacking()
    {
        return isAttacking;
    }
}
