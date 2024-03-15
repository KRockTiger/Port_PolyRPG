using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour
{
    private CharacterController controller;
    private Camera cam;
    private Animator animator;
    private float moveHorizontal; //x�� ��ǥ ���
    private float moveVertical; //z�� ��ǥ ���
    private float targetAngle; //�Է°����� ���� ����
    private Vector3 targetDirection; //ī�޶� ��ġ�� ���� �÷��̾��� ������

    [Header("ĳ���� ������ ����")]
    [SerializeField] private float moveSpeed; //�̵� �ӵ�
    [SerializeField] private float dashSpeed; //�뽬 �ӵ�
    [SerializeField] private float setDashTime; //������ �뽬 �ð�
    [SerializeField] private float curDashTime; //���� �뽬 �ð�
    [SerializeField] private float jumpForce; //ĳ���� ������
    [SerializeField] private float floatCheckGround; //ĳ���� ������
    private float turnSpeed; //ȸ�� �ӵ�
    [SerializeField] private bool isAttacking = false; //�������� �� �̵��� ���� ���� Ʈ����
    [SerializeField] private bool isDash = false; //�뽬 ��� ���
    
    [Header("�߷� ����")]
    private float gravity = -9.81f; //�߷� ��
    [SerializeField] private Vector3 velocity; //�߷��� �����ϱ� ���� ���� ���� ����
    [SerializeField] private Transform groundChecker; //���� üũ�ϱ� ���� Transform ����
    [SerializeField] private float groundDistance; //�� ������ ������ �Ÿ� ����
    [SerializeField] private LayerMask groundMask; //�� ���̾� Ȯ�� ����
    [SerializeField] private bool isGround = true; //���� ���� ����ִ� �� Ȯ��
    [SerializeField] private bool isJump = false; //���� Ʈ����

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
        InputKey();
        Moving();
        Dash();
        Jump();
        Gravity();
    }

    /// <summary>
    /// Ű �ڵ� ����
    /// </summary>
    private void InputKey()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && !isDash) //���콺 ������ ��ư���� ����ϰ� �ߺ� �Է� ������
        {
            curDashTime = setDashTime; //Ÿ�̸� ����
            isDash = true; //�뽬 ���
            animator.SetTrigger("DashStart");
        }
    }

    /// <summary>
    /// �뽬 ���
    /// </summary>
    private void Dash()
    {
        if (!isDash) { return; }

        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f); //ĳ���� ȸ��
        controller.Move(targetDirection.normalized * dashSpeed * Time.deltaTime); //�Է��� �������� �̵�

        //Ÿ�̸� ����
        curDashTime -= Time.deltaTime;
        
        if (curDashTime <= 0f)
        {
            isDash = false; //Ÿ�̸Ӱ� �����鼭 �� ����
        }
    }

    /// <summary>
    /// ĳ���� �������� ���
    /// </summary>
    private void Moving()
    {
        if (isAttacking || isDash) { return; } //���� �� Ȥ�� �뽬 ���� �� �̵��� ���Ƴ���
        
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(moveHorizontal, 0f, moveVertical).normalized;
        //.normalized�� �־ �밢�� �̵��� ���� ���� �ʱ� ���� ���Ƴ���

        if (direction.magnitude >= 0.1f) //����Ű�� �Է��� �� ������ �� �ְ� ����
        //�� ������ ���� ������ ĳ���Ͱ� �ȿ����� �� yȸ�� ���� 0���� ����
        {
            DirectionMovingOfCamera(direction);
        }

        SetAnimation(direction); //�����ӿ� ���� �ִϸ��̼� ����
    }

    /// <summary>
    /// ī�޶��� ��ġ�� �̿��� ĳ������ �������� ���ϰ� �ϴ� �Լ�
    /// </summary>
    private void DirectionMovingOfCamera(Vector3 _direction)
    {
        //���� �ٶ� ������ ������ ��ȯ
        targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + cam.transform.rotation.eulerAngles.y;
        //�� Mathf.Deg2Rad�� ������ �۵� �ȵ� ��
        //cam.transform.rotation.eulerAngles.y�� �־� ī�޶��� yȸ������ ���� ī�޶� ������ ���������� ����
        
        //�� ĳ���� ���⿡�� �Է��� �������� ȸ���� �� �ε巴�� ������ �� �ֵ��� ����
        float smoothTurnY = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, targetAngle, ref turnSpeed, 0.1f);

        transform.rotation = Quaternion.Euler(0f, smoothTurnY, 0f); //������ �� �����Ű��

        //Euler�� ������ �����Ͽ� Vector3.forward�� ���Ͽ� �������� ����
        //�Ʒ� �ڵ带 �������� ĳ���� ������Ʈ�� ȸ���ϵ� �̵��� z���� ���������� �����Ǿ� �� �������θ� ��
        targetDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

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
    private void SetAnimation(Vector3 _moveCheck)
    {
        //Vector3 moveCheck = _direction; //�� �Է°��� ���ͷ� �ֱ�

        if (_moveCheck.magnitude >= 0.1f && isGround) //�������� ������ �� ������ ���� ���� 0���� ũ�� ������ 0.1���� ���� ����
        {
            animator.SetBool("Moving", true);
        }

        else if (_moveCheck.magnitude == 0f && isGround)
        {
            animator.SetBool("Moving", false);
        }

        if (!isGround)
        {
            animator.SetBool("Jump", true);
        }

        else
        {
            animator.SetBool("Jump", false);
        }
    }

    /// <summary>
    /// ĳ���� ���� ���
    /// </summary>
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround) //�� ���� ���ִ� ���¿��� ����Ű�� ������
        {
            isJump = true; //���� Ʈ���� Ȱ��ȭ
        }
    }

    /// <summary>
    /// ������ �߷��� ����� ���� Velocity.y�� �̿��� �Լ�
    /// </summary>
    private void Gravity()
    {
        //������ �� ������Ʈ�� ��ġ�� �̿��Ͽ� ������ �Ÿ��� �����Ͽ� ���
        //isGround = Physics.Raycast(groundChecker.position, transform.position - transform.up, controller.height * 0.5f + floatCheckGround, groundMask);
        isGround = Physics.CheckSphere(groundChecker.position, groundDistance, groundMask);

        if (isGround && velocity.y < 0f) //���� ����ְ� y�ӵ��� 0������ ���� �߷�
        {
            velocity.y = -2f;
        }

        if (isJump) //���� Ʈ���Ű� Ȱ��ȭ�� �Ǹ�
        {
            isJump = false; //���� Ʈ���Ÿ� �ٷ� ��
            velocity.y = jumpForce; //����
        }

        velocity.y += gravity * Time.deltaTime; //���� ���������� ���� �߷�
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position - transform.up);
        //Gizmos.DrawWireSphere(groundChecker.position, 0.55f);
    }

    /// <summary>
    /// PlayerAttack��ũ��Ʈ���� ���ݿ� ���� ����
    /// </summary>
    public void P_SetIsAttacking(bool _isAttacking)
    {
        isAttacking = _isAttacking;
    }

    /// <summary>
    /// isGround ���� ����
    /// </summary>
    /// <returns></returns>
    public bool P_GetIsGround()
    {
        return isGround;
    }

    /// <summary>
    /// isDash ���� ����
    /// </summary>
    /// <returns></returns>
    public bool P_GetIsDash()
    {
        return isDash;
    }
}
