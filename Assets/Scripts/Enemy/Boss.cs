using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private BossAnimation bossAnimation;
    private CharacterController controller;

    [SerializeField] private bool isTest = false; //테스트 전용

    [Header("기본 설정")]
    private float turnSpeed; //회전 속도를 담을 변수
    private float gravity = -9.81f; //중력값
    private Vector3 velocity; //중력 적용 변수
    [SerializeField] private bool isCombo; //콤보 공격 상태
    [SerializeField] private bool isChase; //추적 상태
    [SerializeField] private bool isGround; //땅 체크
    [SerializeField] private bool isHitting; //피격 상태 ==> 피격 중일 경우 약간의 그로기 시간을 가짐
    [SerializeField] private bool isAttacking; //전투 중 상태
    [SerializeField] private bool attackAble; //전투 가능 상태
    [SerializeField] private bool isMoving; //이동 상태
    [SerializeField] private Transform groundChecker; //발이 땅에 닿았는 지를 판단할 Transform
    [SerializeField] private float groundDistance; //발과 땅과의 거리
    [SerializeField] private LayerMask groundMask; //바닥판정을 받을 레이어마스크

    [Header("오브젝트 관리")]
    [SerializeField] private GameObject prfHPImage; //체력바 이미지 프리팹
    [SerializeField] private GameObject objHPImage; //체력바 이미지 오브젝트
    [SerializeField] private GameObject objDamageUI; //데미지 UI 프리팹
    [SerializeField] private BoxCollider attackTrigger; //공격 트리거

    [Header("오브젝트 위치 설정")]
    [SerializeField, Range(0, 5)] private float objHPUIPositionY; //체력바 위치를 정하기 위해 넘길 값
    [SerializeField, Range(0, 5)] private float objDamageUIPositionY; //데미지 오브젝트 위치를 정하기 위해 넘길 값

    [Header("캐릭터 설정")]
    [SerializeField] private float setHP; //설정할 체력
    [SerializeField] private float curHP; //현재 체력
    [SerializeField] private float attackPoint; //공격력
    [SerializeField] private float defendPoint; //방어력
    [SerializeField] private float moveSpeed; //이동 속도
    [SerializeField] private float startSearchRange; //탐색 범위
    [SerializeField] private float endSearchRange; //탐색 중단 범위
    [SerializeField] private float attackRange; //공격 범위
    [SerializeField] private float setAttackTimer; //설정할 공격 타이머
    [SerializeField] private float curAttackTimer; //현재 공격 타이머
    [SerializeField] private bool goAttack; //공격 발사 가능 상태
    [SerializeField] private bool isTimer; //타이머 조절용
    [SerializeField] private bool isDie; //캐릭터 사망 처리

    private WaitForSeconds waitTime = new WaitForSeconds(0.5f); //공격 대기 시간

    private void Awake()
    {
        bossAnimation = GetComponent<BossAnimation>();
        controller = GetComponent<CharacterController>();
        curHP = setHP; //현재 체력 설정
    }

    private void Start()
    {
        goAttack = true;
        //objHPImage = Instantiate(prfHPImage, gameObject.transform); //현 오브젝트에서 자식으로 생성
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) //보스 스킬 테스트용 코드이므로 추후 삭제
        {
            ComboAttack();
        }

        Gravity();

        if (isTest) { return; }
        SearchPlayer();
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
        if (isHitting || isAttacking || isDie) { return; } //피격중 혹은 공격중일 경우 혹은 사망할 경우 리턴

        GameObject player = GameObject.FindGameObjectWithTag("Player"); //플레이어 탐색

        //플레이어와의 거리 측정
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance <= startSearchRange) //몬스터의 탐색 범위보다 거리가 가깝다면
        {
            isChase = true; //추적 상태 키기
            //objHPImage.SetActive(true); //체력바 활성화

            if (distance <= attackRange) //공격 범위보다 가깝다면
            {
                attackAble = true; //전투 상태 전환
                //ComboAttack();
            }

            else //공격 범위보다 멀면
            {
                attackAble = false; //비전투 상태 전환
            }
        }

        else if (distance >= endSearchRange) //몬스터의 탐색 중단 범위 보다 거리가 멀면
        {
            isChase = false; //추적 상태 끄기
            //objHPImage.SetActive(false); //체력바 비활성화
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
        if (!isChase || isCombo) { return; } //추적 상태가 아닐경우 리턴

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

    private void ComboAttack()
    {
        bossAnimation.P_AnimPlayComboAttack();
    }

    public bool P_GetIsChase()
    {
        return isChase;
    }

    public void P_SetIsCombo(bool _isCombo)
    {
        isCombo = _isCombo;
    }
}
