using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;
    private float moveHorizontal; //x�� ��ǥ ���
    private float moveVertical; //z�� ��ǥ ���

    [Header("ĳ���� ���� ����")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;
    private float gravity = 9.81f;

    [Header("ī�޶� ����")]
    private Camera cam;
    [SerializeField] private float camX;
    [SerializeField] private float camZ;

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

        //float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Deg2Rad;
        //transform.rotation = Quaternion.Euler(0f, angle, 0f);
        //controller.Move(direction * moveSpeed * Time.deltaTime);

        //controller.Move(rotTurn * inputMove.normalized * Time.deltaTime * moveSpeed);

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

        controller.Move(_direction * moveSpeed * Time.deltaTime);

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
    /// �߷� ����
    /// </summary>
    private void IsGravity()
    {
        
    }
}
