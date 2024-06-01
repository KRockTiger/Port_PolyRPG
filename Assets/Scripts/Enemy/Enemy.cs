using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyAnimation enemyAnimation;
    private CharacterController controller;

    [Header("보스 확인")]
    [SerializeField, Tooltip("보스 몬스터일 경우 True로 표시")] private bool isBoss;

    [Header("기본 설정")]
    private float turnSpeed; //회전 속도를 담을 변수
    private float gravity = -9.81f; //중력값
    private Vector3 velocity; //중력 적용 변수
    [SerializeField] private bool isChase; //추적 상태
    [SerializeField] private bool isGround; //추적 상태
    [SerializeField] private bool isHitting; //피격 상태 ==> 피격 중일 경우 약간의 그로기 시간을 가짐
    [SerializeField] private bool isAttacking; //전투 중 상태
    [SerializeField] private bool attackAble; //전투 가능 상태
    [SerializeField] private bool isMoving; //이동 상태
    [SerializeField] private Transform groundChecker; //발이 땅에 닿았는 지를 판단할 Transform
    [SerializeField] private float groundDistance; //발과 땅과의 거리
    [SerializeField] private LayerMask groundMask; //바닥판정을 받을 레이어마스크
    [SerializeField] private GameObject prfHPImage; //체력바 이미지 프리팹
    [SerializeField] private GameObject objHPImage; //체력바 이미지 오브젝트

    [Header("캐릭터 설정")]
    [SerializeField] private float setHP; //설정할 체력
    [SerializeField] private float curHP; //현재 체력
    [SerializeField] private float moveSpeed; //이동 속도
    [SerializeField] private float startSearchRange; //탐색 범위
    [SerializeField] private float endSearchRange; //탐색 중단 범위
    [SerializeField] private float attackRange; //공격 범위
    [SerializeField] private float setAttackTimer; //설정할 공격 타이머
    [SerializeField] private float curAttackTimer; //현재 공격 타이머
    [SerializeField] private bool goAttack; //공격 발사 가능 상태
    [SerializeField] private bool isTimer; //타이머 조절용

    [Header("체력바 설정")]
    [SerializeField, Range(0, 5)] private float objHPUIPositionY; //체력바 위치를 정하기 위해 넘길 값

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

    private void Start()
    {
        goAttack = true;
        objHPImage = Instantiate(prfHPImage, gameObject.transform); //현 오브젝트에서 자식으로 생성
        //objHPImage.SetActive(false); //체력바 비활성화
    }

    private void Update()
    {
        Gravity();
        SearchPlayer();
        Attacking();
        HPPassToUI();
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
        if (isHitting || isAttacking) { return; } //피격중 혹은 공격중일 경우 리턴

        GameObject player = GameObject.FindGameObjectWithTag("Player"); //플레이어 탐색
        
        //플레이어와의 거리 측정
        float distance = Vector3.Distance(player.transform.position, transform.position);
    
        if (distance <= startSearchRange) //몬스터의 탐색 범위보다 거리가 가깝다면
        {
            isChase = true; //추적 상태 키기
            objHPImage.SetActive(true); //체력바 활성화

            if (distance <= attackRange) //공격 범위보다 가깝다면
            {
                attackAble = true; //전투 상태 전환
            }

            else //공격 범위보다 멀면
            {
                attackAble = false; //비전투 상태 전환
            }
        }

        else if (distance >= endSearchRange) //몬스터의 탐색 중단 범위 보다 거리가 멀면
        {
            isChase = false; //추적 상태 끄기
            objHPImage.SetActive(false); //체력바 비활성화
            attackAble = false; //비전투 상태 전환,
                                //몬스터의 공격 중 범위에서 벗어났을 때 유지되는 현상을 고침
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

        if (attackAble) { return; } //전투 상태일 경우 멈춤

        //오브젝트 이동
        controller.Move(targetDirection * moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 현재 체력 정보를 생성한 UI오브젝트에 넘겨서 표기
    /// </summary>
    private void HPPassToUI()
    {
        EnemyHP sc = objHPImage.GetComponent<EnemyHP>();
        sc.P_CurrectEnemyInformation(curHP, setHP, objHPUIPositionY, transform);
    }

    /// <summary>
    /// 전투 상태로 전환
    /// 일정 간격으로 평타를 때리는 일반적인 공격 코드
    /// </summary>
    private void Attacking()
    {
        //if (isTimer) //타이머가 작동 가능한 상태일경우
        //{
        //    curAttackTimer -= Time.deltaTime; //공격 쿨타임은 계속 줄어듬
        //}

        //if (curAttackTimer <= 0f)
        //{
        //    goAttack = true; //공격 발사 상태로 전환
        //    isTimer = false; //타이머 중지
        //}

        if (!attackAble) { return; }

        if (goAttack)
        {
            StartCoroutine(C_SetAttackTimer());
            enemyAnimation.P_GoAttack();
        }
    }

    /// <summary>
    /// 피격 판정
    /// </summary>
    private void Hit()
    {
        Debug.Log("맞았습니다!");
        isAttacking = false; //공격 캔슬 판정을 받아야 함

        if (isBoss) { return; } //보스 몬스터일 경우 리턴
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

    private IEnumerator C_SetAttackTimer()
    {
        goAttack = false; //공격 발사 불가능 상태
        yield return new WaitForSeconds(setAttackTimer);
        goAttack = true; //공격 발사 가능 상태
    }

    public void P_SetIsAttacking(bool _isAttacking)
    {
        isAttacking = _isAttacking;
    }

    public bool P_GetIsAttacking()
    {
        return isAttacking;
    }

    public bool P_GetIsChase()
    {
        return isChase;
    }

    public bool P_GetAttackAble()
    {
        return attackAble;
    }
}
