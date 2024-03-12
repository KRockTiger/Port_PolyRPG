using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController controller;
    private Camera cam;
    private Animator animator;
    private float moveHorizontal; //x�� ��ǥ ���
    private float moveVertical; //z�� ��ǥ ���

    [Header("ĳ���� ������ ����")]
    [SerializeField] private float moveSpeed; //�̵� �ӵ�
    [SerializeField] private float jumpPower; //ĳ���� ������
    private float turnSpeed; //ȸ�� �ӵ�

    [Header("�߷� ����")]
    private float gravity = -9.81f; //�߷� ��
    [SerializeField] private Vector3 velocity; //�߷��� �����ϱ� ���� ���� ���� ����
    [SerializeField] private Transform groundChecker; //���� üũ�ϱ� ���� Transform ����
    [SerializeField] private float groundDistance; //�� ������ ������ �Ÿ� ����
    [SerializeField] private LayerMask groundMask; //�� ���̾� Ȯ�� ����
    [SerializeField] private bool isGround = true; //���� ���� ����ִ� �� Ȯ��
    [SerializeField] private bool isJump = false; //���� �ߴ��� Ȯ��

    private void Awake()
    {
        controller = GetComponent<CharacterController>(); //ĳ���� ��Ʈ�ѷ� ����
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        Moving();
        Gravity();
    }

    /// <summary>
    /// ĳ���� �������� ���
    /// </summary>
    private void Moving()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(moveHorizontal, 0f, moveVertical).normalized;
        //.normalized�� �־ �밢�� �̵��� ���� ���� �ʱ� ���� ���Ƴ���

        if (direction.magnitude >= 0.1f) //����Ű�� �Է��� �� ������ �� �ְ� ����
        //�� ������ ���� ������ ĳ���Ͱ� �ȿ����� �� yȸ�� ���� 0���� ����
        {
            DirectionMovingOfCamera(direction);
        }

        AnimMoving(direction); //�����ӿ� ���� �ִϸ��̼� ����
    }

    /// <summary>
    /// ī�޶��� ��ġ�� �̿��� ĳ������ �������� ���ϰ� �ϴ� �Լ�
    /// </summary>
    private void DirectionMovingOfCamera(Vector3 _direction)
    {
        //���� �ٶ� ������ ������ ��ȯ
        float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + cam.transform.rotation.eulerAngles.y;
        //�� Mathf.Deg2Rad�� ������ �۵� �ȵ� ��
        //cam.transform.rotation.eulerAngles.y�� �־� ī�޶��� yȸ������ ���� ī�޶� ������ ���������� ����
        
        //�� ĳ���� ���⿡�� �Է��� �������� ȸ���� �� �ε巴�� ������ �� �ֵ��� ����
        float smoothTurnY = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, targetAngle, ref turnSpeed, 0.1f);

        transform.rotation = Quaternion.Euler(0f, smoothTurnY, 0f); //������ �� �����Ű��

        //Euler�� ������ ���Ͽ� Vector3.forward�� ���Ͽ� �������� ����
        Vector3 targetDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        controller.Move(targetDirection * moveSpeed * Time.deltaTime);

        #region ȸ�� ���� �� �ڵ�
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
    /// ĳ������ �������� üũ�Ͽ� �ִϸ��̼� ����
    /// </summary>
    private void AnimMoving(Vector3 _moveCheck)
    {
        //Vector3 moveCheck = _direction; //�� �Է°��� ���ͷ� �ֱ�

        if (_moveCheck.magnitude >= 0.1f) //�������� ������ �� ������ ���� ���� 0���� ũ�� ������ 0.1���� ���� ����
        {
            animator.SetBool("Moving", true);
        }

        else if (_moveCheck.magnitude == 0f)
        {
            animator.SetBool("Moving", false);
        }
    }

    /// <summary>
    /// ������ �߷��� ����� ���� Velocity.y�� �̿��� �Լ�
    /// </summary>
    private void Gravity()
    {
        //������ �� ������Ʈ�� ��ġ�� �̿��Ͽ� ������ �Ÿ��� �����Ͽ� ���
        isGround = Physics.CheckSphere(groundChecker.position, groundDistance, groundMask);
        //isGround = Physics.Raycast(groundChecker.position + new Vector3(0, 1f, 0),
                                   //Vector3.down, 1f, groundMask);

        if (isGround && velocity.y < 0f) //���� ������� ���� �߷�
        {
            velocity.y = -2f;
        }


        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            velocity.y = Mathf.Sqrt(jumpPower * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime; //���� ���������� ���� �߷�

        controller.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundChecker.position + new Vector3(0, -1f, 0f), Vector3.down);
    }
}
