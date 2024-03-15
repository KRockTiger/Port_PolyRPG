using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackAnimation : MonoBehaviour
{
    [Header("�Ķ���� ����")]
    [SerializeField] private string attacking; //������ �ִϸ����� �Ķ����
    [SerializeField] private string nextAttack; //���� ����
    [SerializeField] private string dash; //�뽬 �Ķ����
    [SerializeField] private string attack01; //ù ��° ���� �ִϸ��̼�

    private Animator animator;
    private PlayerMove playerMove; //�÷��̾� ������ ��ũ��Ʈ
    private PlayerAttack playerAttack; //�÷��̾��� ���� ��ũ��Ʈ
    //[SerializeField, Tooltip("�������� �� True")] private bool isAttacking = false;
    //[SerializeField, Tooltip("�������� �� True")] private bool isGround = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerAttack = GetComponent<PlayerAttack>();
        playerMove = GetComponent<PlayerMove>();
    }

    private void Update()
    {
        AttackAnimation();
        DashAnimation();
    }

    /// <summary>
    /// �÷��̾ �����ϴ� ���
    /// </summary>
    private void AttackAnimation()
    {
        //ó���� �� ���ǹ� : Input.GetKeyDown(KeyCode.Mouse0) && !isAttacking && isGround
        if (playerAttack.P_GetIsAttack())
        {
            animator.SetBool(attacking, true); //���� �ִϸ��̼� ���
        }

        if (playerAttack.P_GetNextAttack()) //���� ���� Ʈ���Ű� ����Ǹ�
        {
            animator.SetBool(nextAttack, true); //�Ķ���� Ű��
        }

        else //Ʈ���Ű� ����Ǹ�
        {
            animator.SetBool(nextAttack, false); //�Ķ���� ����
            
        } //=> nextAttack�� ���� ������ �����ϰ� �ϴ� bool�� ���̹Ƿ� �� �� ���� �ڵ� ���� ������ ������
    }

    /// <summary>
    /// �뽬 �ִϸ��̼� ����
    /// PlayerMove�� isDash ���� true�� ���� false�� ����
    /// </summary>
    private void DashAnimation()
    {
        bool isDash = playerMove.P_GetIsDash();
        bool curAnimState = animator.GetBool(dash);
        if ((isDash == true && curAnimState == false) || (isDash == false && curAnimState == true))
        {
            animator.SetBool(dash, isDash);
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
        //animator.SetBool(nextAttack, false);
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
