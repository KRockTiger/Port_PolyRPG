using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimation : MonoBehaviour
{
    private Boss boss;
    private Animator animator;

    [SerializeField] private string moving;
    [SerializeField] private List<bool> testCombos; //�޺� ��ų�� �׽�Ʈ �ϱ� ���� ����Ʈ

    [SerializeField] private List<string> listComboAttacks;

    private void Awake()
    {
        boss = GetComponent<Boss>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        MovingAnimation();
    }

    /// <summary>
    /// ���� ������ �ִϸ��̼� ����
    /// </summary>
    private void MovingAnimation()
    {
        if (boss.P_GetIsChase()) /*&& !boss.P_GetAttackAble()*/
        {
            animator.SetBool(moving, true);
        }

        else
        {
            animator.SetBool(moving, false);
        }
    }

    /// <summary>
    /// ������ ��ȣ�� �����Ͽ� �޺� ��ų �����ϱ�
    /// -�Ű� ������ Ư�� �޺��� Ȯ���ϱ� ���� �и��ؼ� ����ϱ� ���� �����̹Ƿ� ���� ���� �ʿ�
    /// </summary>
    public void P_AnimPlayComboAttack(string _comboName)
    {
        int randNum = Random.Range(0, listComboAttacks.Count); //����Ʈ�� ��ϵ� �޺� ��ų ���� ����

        string combo = listComboAttacks[randNum]; //������ ��ȣ�� ���Ͽ� �޺��� �� ��ų �ִϸ��̼� �̸� ����

        animator.Play(_comboName); //�÷���
    }

    public void A_OnIsCombo()
    {
        boss.P_SetIsCombo(true);
    }

    public void A_OffIsCombo()
    {
        boss.P_SetIsCombo(false);
    }

    /// <summary>
    /// ���� ��ũ��Ʈ�� ��ϵ� ���� �޽� �ݸ����� true�� �����Ͽ� �ݸ��� ����
    /// </summary>
    public void A_OnWeaponCollider()
    {
        boss.P_SetWeaponCollider(true);
    }

    /// <summary>
    /// �ݴ�� ���� �޽� �ݸ����� false�� �����Ͽ� �ݸ��� ����
    /// </summary>
    public void A_OffWeaponCollider()
    {
        boss.P_SetWeaponCollider(false);
    }

    public void A_SetChopTrigger()
    {
        //StartCoroutine(boss.PC_SetChopTrigger());
        boss.PC_SetChopTrigger();
    }
}
