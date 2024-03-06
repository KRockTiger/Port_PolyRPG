using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;
    [SerializeField] private float moveHorizontal; //x축 좌표 담당
    [SerializeField] private float moveVertical; //z축 좌표 담당

    [Header("캐릭터 무빙 설정")]
    [SerializeField] private float moveSpeed;
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
        DirectionOfCamera();
        AnimMoving();
    }

    /// <summary>
    /// 캐릭터 움직임을 담당
    /// </summary>
    private void Moving()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 inputMove = new Vector3(moveHorizontal, 0f, moveVertical);
        Quaternion rotTurn = cam.transform.rotation;
        rotTurn.y = 0f;
        //controller.Move(inputMove * moveSpeed * Time.deltaTime);

        controller.Move(rotTurn * inputMove.normalized * Time.deltaTime * moveSpeed);
    }

    /// <summary>
    /// 카메라의 위치를 이용해 캐릭터의 정방향을 정하게 하는 함수
    /// </summary>
    private void DirectionOfCamera()
    {
        //Vector3 posCamera = cam.transform.position; //카메라의 위치를 저장
        //posCamera.y = 0f; //y값은 방향에 관련 없으므로 0으로 처리

        //Vector3 dir = (transform.forward - cam.transform.forward);
        //camX = dir.x;
        //camZ = dir.z;
        //Vector3 direction = posCamera.normalized; //그대로 다시 저장 후 노멀라이즈

        //float angleY = Mathf.Atan2(dir.z, dir.x) * Mathf.Deg2Rad;
        //Debug.Log($"dir = {dir},angleY = {angleY}");
        //transform.eulerAngles = new Vector3 (0f, angle, 0f);

        //transform.rotation = Quaternion.Euler(0f, angle, 0f);

        if (Input.GetKey(KeyCode.W))
        { 
            float targetAngle = cam.transform.rotation.eulerAngles.y;
            float vel = 0f;
            float fixedAngle = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, 
                targetAngle, ref vel, 0.04f);

            //transform.rotation = Quaternion.Euler(new Vector3(0f, targetAngle, 0f));
            transform.rotation = Quaternion.Euler(new Vector3(0f, fixedAngle, 0f));
        }
    }

    /// <summary>
    /// 캐릭터의 움직임을 체크하여 애니메이션 적용
    /// </summary>
    private void AnimMoving()
    {
        Vector3 moveCheck = new Vector3(moveHorizontal, 0f, moveVertical); //위 입력값을 벡터로 넣기

        if (moveCheck.magnitude >= 0.1f) //움직임이 있으면 위 벡터의 길이 값이 0보다 크기 때문에 0.1보다 높게 측정
        {
            animator.SetBool("Moving", true);
        }

        else if (moveCheck.magnitude == 0f)
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
