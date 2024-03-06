using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;
    [SerializeField] private float moveHorizontal; //x�� ��ǥ ���
    [SerializeField] private float moveVertical; //z�� ��ǥ ���

    [Header("ĳ���� ���� ����")]
    [SerializeField] private float moveSpeed;
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
        DirectionOfCamera();
        AnimMoving();
    }

    /// <summary>
    /// ĳ���� �������� ���
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
    /// ī�޶��� ��ġ�� �̿��� ĳ������ �������� ���ϰ� �ϴ� �Լ�
    /// </summary>
    private void DirectionOfCamera()
    {
        //Vector3 posCamera = cam.transform.position; //ī�޶��� ��ġ�� ����
        //posCamera.y = 0f; //y���� ���⿡ ���� �����Ƿ� 0���� ó��

        //Vector3 dir = (transform.forward - cam.transform.forward);
        //camX = dir.x;
        //camZ = dir.z;
        //Vector3 direction = posCamera.normalized; //�״�� �ٽ� ���� �� ��ֶ�����

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
    /// ĳ������ �������� üũ�Ͽ� �ִϸ��̼� ����
    /// </summary>
    private void AnimMoving()
    {
        Vector3 moveCheck = new Vector3(moveHorizontal, 0f, moveVertical); //�� �Է°��� ���ͷ� �ֱ�

        if (moveCheck.magnitude >= 0.1f) //�������� ������ �� ������ ���� ���� 0���� ũ�� ������ 0.1���� ���� ����
        {
            animator.SetBool("Moving", true);
        }

        else if (moveCheck.magnitude == 0f)
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
