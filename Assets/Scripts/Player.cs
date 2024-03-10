using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;
    private float moveHorizontal; //x축 좌표 담당
    private float moveVertical; //z축 좌표 담당

    [Header("캐릭터 무빙 설정")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;
    private float gravity = 9.81f;

    [Header("카메라 설정")]
    private Camera cam;
    [SerializeField] private float camX;
    [SerializeField] private float camZ;

    private void Awake()
    {
        controller = GetComponent<CharacterController>(); //캐릭터 컨트롤러 접근
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        Moving();
    }

    /// <summary>
    /// 캐릭터 움직임을 담당
    /// </summary>
    private void Moving()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(moveHorizontal, 0f, moveVertical).normalized;
        //.normalized를 넣어서 대각선 이동이 빨리 가지 않기 위해 막아놓음

        //float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Deg2Rad;
        //transform.rotation = Quaternion.Euler(0f, angle, 0f);
        //controller.Move(direction * moveSpeed * Time.deltaTime);

        //controller.Move(rotTurn * inputMove.normalized * Time.deltaTime * moveSpeed);

        if (direction.magnitude >= 0.1f) //방향키를 입력할 때 적용할 수 있게 적용
        //이 조건을 넣지 않으면 캐릭터가 안움직일 때 y회전 값이 0으로 고정
        {
            DirectionMovingOfCamera(direction);
        }

        AnimMoving(direction); //움직임에 따라 애니메이션 적용
    }

    /// <summary>
    /// 카메라의 위치를 이용해 캐릭터의 정방향을 정하게 하는 함수
    /// </summary>
    private void DirectionMovingOfCamera(Vector3 _direction)
    {
        //내가 바라볼 방향을 각도로 전환
        float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + cam.transform.rotation.eulerAngles.y;
        //※ Mathf.Deg2Rad를 넣으면 작동 안됨 ※
        //cam.transform.rotation.eulerAngles.y를 넣어 카메라의 y회전값을 더해 카메라 방향을 정방향으로 설정

        //현 캐릭터 방향에서 입력한 방향으로 회전할 때 부드럽게 움직일 수 있도록 설정
        float smoothTurnY = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, targetAngle, ref turnSpeed, 0.1f);

        transform.rotation = Quaternion.Euler(0f, smoothTurnY, 0f); //설정한 값 적용시키기

        controller.Move(_direction * moveSpeed * Time.deltaTime);

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
    private void AnimMoving(Vector3 _moveCheck)
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
    }

    /// <summary>
    /// 중력 적용
    /// </summary>
    private void IsGravity()
    {
        
    }
}
