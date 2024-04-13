using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour
{
    private PlayerAttack playerAttack;
    private CharacterController controller;
    private Camera cam;
    private Animator animator;
    private float moveHorizontal; //x축 좌표 담당
    private float moveVertical; //z축 좌표 담당
    private float targetAngle; //입력값으로 인한 각도
    private Vector3 targetDirection; //카메라 위치에 따른 플레이어의 정방향
    private Vector3 inputDirection; //카메라 입력에 따른 플레이어의 정방향

    [Header("캐릭터 움직임 설정")]
    [SerializeField] private float moveSpeed; //이동 속도
    [SerializeField] private float dashSpeed; //대쉬 속도
    [SerializeField] private float setDashTime; //설정할 대쉬 시간
    [SerializeField] private float curDashTime; //현재 대쉬 시간
    [SerializeField] private float jumpForce; //캐릭터 점프력
    [SerializeField] private float floatCheckGround; //캐릭터 점프력
    private float turnSpeed; //회전 속도
    [SerializeField] private bool isAttacking = false; //공격중일 때 이동을 막기 위한 트리거
    [SerializeField] private bool isDash = false; //대쉬 기능 사용
    
    [Header("중력 설정")]
    private float gravity = -9.81f; //중력 값
    [SerializeField] private Vector3 velocity; //중력을 적용하기 위해 만든 벡터 변수
    [SerializeField] private Transform groundChecker; //땅을 체크하기 위한 Transform 변수
    [SerializeField] private float groundDistance; //땅 판정을 결정할 거리 변수
    [SerializeField] private LayerMask groundMask; //땅 레이어 확인 변수
    [SerializeField] private bool isGround = true; //발이 땅에 닿아있는 지 확인
    [SerializeField] private bool isJump = false; //점프 트리거
                                                  //==> (03.18) 점프 사용하는 기능에서 점프 사용 가능 여부로 변경
                                                  //==> 점프는 공격과 대쉬를 캔슬할 수 있는 중요 기능이므로
                                                  //    애니메이터에서 트리거를 받는 걸로 변경

    private void Awake()
    {
        controller = GetComponent<CharacterController>(); //캐릭터 컨트롤러 접근
        animator = GetComponent<Animator>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        InputKey();
        Moving();
        Dash();
        Jump();
        Gravity();
    }

    /// <summary>
    /// 키 코드 관리
    /// </summary>
    private void InputKey()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && !isDash && isGround) //마우스 오른쪽 버튼으로 사용하고 중복 입력 방지함
        {
            curDashTime = setDashTime; //타이머 설정
            isDash = true; //대쉬 사용
            animator.SetTrigger("DashStart");
            playerAttack.P_SetIsNext(false);
            //=> 연속 공격 중 대쉬 혹은 점프로 캔슬 할 경우 isNext가 true로 계속 남게 되어 다음 연속 공격할 때
            //=> 비정상적인 속도로 연속 공격이 실행됨
            playerAttack.P_SetIsAttack(false); //공격 끄기
            playerAttack.P_ReSetAttackCycle(); //횟수 초기화
        }
    }

    /// <summary>
    /// 대쉬 사용
    /// </summary>
    private void Dash()
    {
        if (!isDash) { return; }

        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f); //캐릭터 회전
        controller.Move(targetDirection.normalized * dashSpeed * Time.deltaTime); //입력한 방향으로 이동

        //타이머 설정
        curDashTime -= Time.deltaTime;
        
        if (curDashTime <= 0f)
        {
            isDash = false; //타이머가 끝나면서 값 변경
        }
    }

    /// <summary>
    /// 캐릭터 움직임을 담당
    /// </summary>
    private void Moving()
    {
        if (isDash) { return; } //03.18) 공격 중일 경우 이동을 막아 놓지만 입력을 통하여 대쉬 방향을 정해야하므로
                                //       대쉬 중일 경우에만 입력을 막아놓는다.
        
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        inputDirection = new Vector3(moveHorizontal, 0f, moveVertical).normalized;
        //.normalized를 넣어서 대각선 이동이 빨리 가지 않기 위해 막아놓음

        //if (isAttacking || isDash) { return; } //공격 중 혹은 대쉬 중일 때 이동을 막아놓음
        
        if (inputDirection.magnitude >= 0.1f) //방향키를 입력할 때 적용할 수 있게 적용
        //이 조건을 넣지 않으면 캐릭터가 안움직일 때 y회전 값이 0으로 고정
        {
            DirectionMovingOfCamera(inputDirection);
        }

        SetAnimation(inputDirection); //움직임에 따라 애니메이션 적용
    }

    /// <summary>
    /// 카메라의 위치를 이용해 캐릭터의 정방향을 정하게 하는 함수
    /// </summary>
    private void DirectionMovingOfCamera(Vector3 _direction)
    {
        //내가 바라볼 방향을 각도로 전환
        targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + cam.transform.rotation.eulerAngles.y;
        //※ Mathf.Deg2Rad를 넣으면 작동 안됨 ※
        //cam.transform.rotation.eulerAngles.y를 넣어 카메라의 y회전값을 더해 카메라 방향을 정방향으로 설정
        
        //Euler로 방향을 설정하여 Vector3.forward를 곱하여 정방향을 결정
        //아래 코드를 안적으면 캐릭터 오브젝트만 회전하되 이동은 z축을 정방향으로 고정되어 한 방향으로만 감
        targetDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; //=> 방향 결정

        if (isAttacking) { return; } //공격중일 때 이동을 막아놓음

        //현 캐릭터 방향에서 입력한 방향으로 회전할 때 부드럽게 움직일 수 있도록 설정
        float smoothTurnY = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, targetAngle, ref turnSpeed, 0.1f);

        transform.rotation = Quaternion.Euler(0f, smoothTurnY, 0f); //설정한 값 적용시키기 => 회전

        controller.Move(targetDirection * moveSpeed * Time.deltaTime);

        #region 회전 수정 전 코드
        //transform.eulerAngles = new Vector3(0f, angle, 0f);

        //transform.rotation = Quaternion.Euler(0f, angle, 0f);

        //if (Input.GetKey(KeyCode.W))
        //{ 
        //    float targetAngle = cam.transform.rotation.eulerAngles.y;
        //    float vel = 0f;
        //    float fixedAngle = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, 
        //        targetAngle, ref vel, 0.04f);

        //    //transform.rotation = Quaternion.Euler(new Vector3(0f, targetAngle, 0f));
        //    transform.rotation = Quaternion.Euler(new Vector3(0f, fixedAngle, 0f));
        //}
        #endregion
    }

    /// <summary>
    /// 캐릭터의 움직임을 체크하여 애니메이션 적용
    /// </summary>
    private void SetAnimation(Vector3 _moveCheck)
    {
        //Vector3 moveCheck = _direction; //위 입력값을 벡터로 넣기

        if (_moveCheck.magnitude >= 0.1f) //움직임이 있으면 위 벡터의 길이 값이 0보다 크기 때문에 0.1보다 높게 측정
        {
            animator.SetBool("Moving", true);
        }

        else if (_moveCheck.magnitude == 0f)
        {
            animator.SetBool("Moving", false);
        }

        //if (!isGround)
        //{
        //    animator.SetBool("Jump", true);
        //}

        //else
        //{
        //    animator.SetBool("Jump", false);
        //} ==> 03.18) 기능 변경으로 인한 취소
    }

    /// <summary>
    /// 캐릭터 점프 담당
    /// - 점프로 공격 혹은 대쉬를 캔슬할 수 있다.
    /// </summary>
    private void Jump()
    {
        if (isGround && velocity.y < 0f) //땅에 붙어 있고 velocity 값이 음수일 경우
        {
            isJump = true; //점프 가능
            animator.SetBool("Jumping", false); //땅에 닿으면 끄기
        }

        else //그 외의 상황일 경우
        {
            isJump = false; //점프 불가능
        }

        if (Input.GetKeyDown(KeyCode.Space) && isJump) //땅 위에 서있는 상태에서 점프키를 누르면
        {
            //isJump = true; //점프 트리거 활성화 ==> 03.18) 기능 변경으로 인한 취소

            velocity.y = Mathf.Sqrt(jumpForce * 2);
            animator.SetTrigger("Jump"); //anistate에서 바로 실행할 수 있게 설정
            animator.SetBool("Jumping", true); //점프 애니메이션에서 머무를 수 있게 true

            if (isAttacking || isDash) //공격중 혹은 대쉬중일 때
            {
                playerAttack.P_SetIsAttack(false); //공격 중지
                playerAttack.P_ReSetAttackCycle(); //횟수 초기화
                isAttacking = false; //공격 캔슬
                isDash = false; //대쉬 캔슬
                playerAttack.P_SetIsNext(false);
                //=> 연속 공격 중 대쉬 혹은 점프로 캔슬 할 경우 isNext가 true로 계속 남게 되어 다음 연속 공격할 때
                //=> 비정상적인 속도로 연속 공격이 실행됨
            }
        }
    }

    /// <summary>
    /// 점프와 중력을 만들기 위해 Velocity.y를 이용한 함수
    /// </summary>
    private void Gravity()
    {
        //지정한 빈 오브젝트의 위치를 이용하여 땅과의 거리를 측정하여 사용
        //isGround = Physics.Raycast(groundChecker.position, transform.position - transform.up, controller.height * 0.5f + floatCheckGround, groundMask);
        isGround = Physics.CheckSphere(groundChecker.position, groundDistance, groundMask);

        if (isGround && velocity.y < 0f) //땅에 닿아있고 y속도가 0이하일 때의 중력
        {
            velocity.y = -2f;
        }

        //if (isJump) //점프 트리거가 활성화가 되면
        //{
        //    isJump = false; //점프 트리거를 바로 끔
        //    velocity.y = jumpForce; //점프
        //} ==> 03.18) 기능 변경으로 취소

        velocity.y += gravity * Time.deltaTime; //땅에 떨어져있을 때의 중력
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position - transform.up);
        //Gizmos.DrawWireSphere(groundChecker.position, 0.55f);
    }

    /// <summary>
    /// PlayerAttack스크립트에서 공격에 따라 적용
    /// </summary>
    public void P_SetIsAttacking(bool _isAttacking)
    {
        isAttacking = _isAttacking;
    }

    /// <summary>
    /// isGround 값을 전달
    /// </summary>
    /// <returns></returns>
    public bool P_GetIsGround()
    {
        return isGround;
    }

    /// <summary>
    /// isDash 값을 전달
    /// </summary>
    /// <returns></returns>
    public bool P_GetIsDash()
    {
        return isDash;
    }
}
