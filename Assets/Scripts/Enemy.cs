using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyAnimation enemyAnimation;
    private CharacterController controller;

    [Header("�⺻ ����")]
    private float turnSpeed; //ȸ�� �ӵ��� ���� ����
    private float gravity = -9.81f; //�߷°�
    private Vector3 velocity; //�߷� ���� ����
    [SerializeField] private bool isChase; //���� ����
    [SerializeField] private bool isGround; //���� ����
    [SerializeField] private bool isHitting; //�ǰ� ���� ==> �ǰ� ���� ��� �ణ�� �׷α� �ð��� ����
    [SerializeField] private bool isAttacking; //���� ����
    [SerializeField] private bool isMoving; //�̵� ����
    [SerializeField] private Transform groundChecker;
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundMask;

    [Header("ĳ���� ����")]
    [SerializeField] private float setHP; //������ ü��
    [SerializeField] private float curHP; //���� ü��
    [SerializeField] private float moveSpeed; //�̵� �ӵ�
    [SerializeField] private float startSearchRange; //Ž�� ����
    [SerializeField] private float endSearchRange; //Ž�� �ߴ� ����
    [SerializeField] private float attackRange; //���� ����

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
        curHP = setHP; //���� ü�� ����
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
    /// �÷��̾� Ž��
    /// </summary>
    private void SearchPlayer()
    {
        if (isHitting) { return; } //�ǰ����� ��� ����

        GameObject player = GameObject.FindGameObjectWithTag("Player"); //�÷��̾� Ž��

        //�÷��̾���� �Ÿ� ����
        float distance = Vector3.Distance(player.transform.position, transform.position);
    
        if (distance <= startSearchRange) //������ Ž�� �������� �Ÿ��� �����ٸ�
        {
            isChase = true; //���� ���� Ű��

            if (distance <= attackRange) //���� �������� �����ٸ�
            {
                isAttacking = true; //���� ���� ��ȯ
            }

            else //���� �������� �ָ�
            {
                isAttacking = false; //������ ���� ��ȯ
            }
        }

        else if (distance >= endSearchRange) //������ Ž�� �ߴ� ���� ���� �Ÿ��� �ָ�
        {
            isChase = false; //���� ���� ����
        }

        ChasePlayer(player);
    }

    /// <summary>
    /// �÷��̾� ����
    /// </summary>
    private void ChasePlayer(GameObject _objPlayer)
    {
        if (!isChase) { return; } //���� ���°� �ƴҰ�� ����

        //���� ����
        Vector3 direction = (_objPlayer.transform.position - transform.position).normalized;

        //������ ������ ������ ȸ���� ����
        Quaternion angle = Quaternion.LookRotation(direction);

        //�� ������ ȸ������ y���� �����ͼ� ȸ���� �ε巴�� �� �� �ֵ��� ����
        float smoothTurnY = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, angle.eulerAngles.y, ref turnSpeed, 0.1f);

        //ȸ�� ����
        transform.rotation = Quaternion.Euler(0f, smoothTurnY, 0f);

        //������Ʈ�� ������ ����
        Vector3 targetDirection = Quaternion.Euler(0f, angle.eulerAngles.y, 0f) * Vector3.forward;

        if (isAttacking) { return; } //���� ������ ��� ����

        //������Ʈ �̵�
        controller.Move(targetDirection * moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// ���� ���·� ��ȯ
    /// </summary>
    private void Attacking()
    {
        if (!isAttacking) { return; }


    }

    /// <summary>
    /// �ǰ� ����
    /// </summary>
    private void Hit()
    {
        Debug.Log("�¾ҽ��ϴ�!");
        StartCoroutine(C_SetIsHitting());
    }

    /// <summary>
    /// �ǰ� �� 1�ʵ��� �׷α� ���¸� ����
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
