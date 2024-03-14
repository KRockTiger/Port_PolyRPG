using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackAnimation : MonoBehaviour
{
    [Header("�Ķ���� ����")]
    [SerializeField] private string attacking; //������ �ִϸ����� �Ķ����
    [SerializeField] private string nextAttack; //���� ����
    [SerializeField] private string attack01; //ù ��° ���� �ִϸ��̼�
    [SerializeField] private string attack02; //�� ��° ���� �ִϸ��̼�

    private Animator animator;
    //private PlayerMove playerMove; //�÷��̾� ������ ��ũ��Ʈ
    private PlayerAttack playerAttack; //�÷��̾��� ���� ��ũ��Ʈ
    //[SerializeField, Tooltip("�������� �� True")] private bool isAttacking = false;
    //[SerializeField, Tooltip("�������� �� True")] private bool isGround = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerAttack = GetComponent<PlayerAttack>();
        //playerMove = GetComponent<PlayerMove>();
    }

    private void Update()
    {
        Attack();
    }

    /// <summary>
    /// �÷��̾ �����ϴ� ���
    /// </summary>
    private void Attack()
    {
        //ó���� �� ���ǹ� : Input.GetKeyDown(KeyCode.Mouse0) && !isAttacking && isGround
        if (playerAttack.P_GetIsAttack())
        {
            animator.SetBool(attacking, true); //���� �ִϸ��̼� ���
        }

        if (playerAttack.P_GetNextAttack())
        {
            animator.SetBool(nextAttack, true);
        }
    }

    /// <summary>
    /// �ִϸ��̼ǿ� ���� �Լ�
    /// isAttack�� false�� �ٲ� ������ �������� Ȯ�ν�Ű�� ��
    /// </summary>
    private void A_UnIsAttack()
    {
        playerAttack.P_SetIsAttack(false); //PlayerAttack ��ũ��Ʈ�� isAttack���� false�� ����
        playerAttack.P_SetIsNext(false); //������ �������Ƿ� ���� ���� Ʈ���� ����
        animator.SetBool(attacking, false); //���� �ִϸ��̼� ����
    }

    /// <summary>
    /// ���� ������ ����ϱ� ���� �ִϸ��̼� �������� Ʈ���� Ȱ��ȭ
    /// </summary>
    private void A_IsNext()
    {
        playerAttack.P_SetIsNext(true);
    }

    /// <summary>
    /// ���� ������ ������� ���ϰ� ����
    /// </summary>
    private void A_UnIsNext()
    {
        playerAttack.P_SetIsNext(false);
    }
    /// <summary>
    /// �ִϸ��̼ǿ� ���� �Լ�
    /// isAttack�� true�� �ٲ� �������̶�� ���� Ȯ�ν�Ű�� ��
    /// </summary>
    //private void A_IsAttack()
    //{
    //    isAttacking = true;
    //}
}
