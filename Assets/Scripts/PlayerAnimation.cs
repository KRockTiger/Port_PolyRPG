using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("�Ķ���� ����")]
    [SerializeField] private string attacking; //������ �ִϸ����� �Ķ����
    [SerializeField] private string nextAttack; //���� ����
    [SerializeField] private string dash; //�뽬 �Ķ����
    [SerializeField] private string attack01; //ù ��° ���� �ִϸ��̼�

    private Animator animator;
    private PlayerMove playerMove; //�÷��̾� ������ ��ũ��Ʈ
    private PlayerAttack playerAttack; //�÷��̾��� ���� ��ũ��Ʈ
    private bool notEndAttack = false;
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

        else
        {
            animator.SetBool(attacking, false);
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
        if (notEndAttack) //���� ���� �ߴ��� �ϸ� �ȵǴ� ��Ȳ�� ���� ��츦 ����� ���� ó���� �Ͽ� ����
        { return; }
        playerAttack.P_SetIsAttack(false); //PlayerAttack ��ũ��Ʈ�� isAttack���� false�� ����
        playerAttack.P_SetIsNext(false); //������ �������Ƿ� ���� ���� Ʈ���� ����
        playerAttack.P_ReSetAttackCycle(); //���� ���� Ƚ�� �ʱ�ȭ
        //animator.SetBool(attacking, false); //���� �ִϸ��̼� ����
    }

    /// <summary>
    /// ���� ������ ����ϱ� ���� �ִϸ��̼� �������� Ʈ���� Ȱ��ȭ
    /// </summary>
    private void A_IsNext()
    {
        //03.21) ���� �� ĵ�� ���� �� ������ Ÿ�ֿ̹� �� �ڵ尡 �۵��Ǿ� ���� ������ ������ �Ͼ�� ���װ� ����
        if (playerAttack.P_GetIsAttack()) //���� ĳ���Ͱ� ���� ���� ���¿��� �ڵ尡 �۵��ǰ� ����
        {
            playerAttack.P_SetIsNext(true);
        }
    }

    /// <summary>
    /// ���� ������ ������� ���ϰ� ����
    /// </summary>
    private void A_UnIsNext()
    {
        playerAttack.P_SetIsNext(false);
        StartCoroutine(C_NotEndAttack()); //������ ���ڱ� �ߴܵǴ� ���׸� �����ϱ� ���� ó��
        //animator.SetBool(nextAttack, false);
    }

    /// <summary>
    /// ���� �ִϸ��̼� �ʱ⿡ Ƚ�� �ױ�
    /// </summary>
    private void A_SetAttackCycle()
    {
        playerAttack.P_SetAttackCycle();
    }

    /// <summary>
    /// ���� �ߴ��� ���� ���� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    private IEnumerator C_NotEndAttack()
    {
        notEndAttack = true;
        yield return new WaitForSeconds(0.3f);
        notEndAttack = false;
    }

    /// <summary>
    /// ���� �ִϸ��̼��� ������ ���� �ʱ�ȭ �ϱ�
    /// </summary>
    //private void A_ReSetAttackCycle()
    //{
    //    playerAttack.P_ReSetAttackCycle();
    //}

    /// <summary>
    /// �ִϸ��̼ǿ� ���� �Լ�
    /// isAttack�� true�� �ٲ� �������̶�� ���� Ȯ�ν�Ű�� ��
    /// </summary>
    //private void A_IsAttack()
    //{
    //    isAttacking = true;
    //}
}
