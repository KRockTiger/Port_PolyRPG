using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackAnimation : MonoBehaviour
{
    [Header("�ִϸ����� ����")]
    [SerializeField] private string attack01; //������ �ִϸ����� �Ķ����

    private Animator animator;
    private PlayerMove playerMove;
    [SerializeField, Tooltip("�������� �� True")] private bool isAttacking = false;
    [SerializeField, Tooltip("�������� �� True")] private bool isGround = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        playerMove = GetComponent<PlayerMove>();
    }

    private void Update()
    {
        Attack();
        SetIsAttacking();
        GetIsGround();
    }

    /// <summary>
    /// �÷��̾ �����ϴ� ���
    /// </summary>
    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttacking && isGround)
        {
            animator.SetBool(attack01, true); //���� �ִϸ��̼� ���
        }
    }

    /// <summary>
    /// �÷��̾� ���ݿ� ���� PlayerMove��ũ��Ʈ�� IsAttacking ���� ����
    /// </summary>
    private void SetIsAttacking()
    {
        playerMove.P_SetIsAttacking(isAttacking);
    }

    private void GetIsGround()
    {
        isGround = playerMove.P_GetIsGround();
    }

    /// <summary>
    /// �ִϸ��̼ǿ� ���� �Լ�
    /// isAttack�� true�� �ٲ� �������̶�� ���� Ȯ�ν�Ű�� ��
    /// </summary>
    private void A_IsAttack()
    {
        isAttacking = true;
    }

    /// <summary>
    /// �ִϸ��̼ǿ� ���� �Լ�
    /// isAttack�� false�� �ٲ� ������ �������� Ȯ�ν�Ű�� ��
    /// </summary>
    private void A_UnIsAttack()
    {
        isAttacking = false;
        animator.SetBool(attack01, isAttacking);
    }
}
