using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimation : MonoBehaviour
{
    private Boss boss;
    private Animator animator;

    [SerializeField] private string moving;

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
    /// </summary>
    public void P_AnimPlayComboAttack()
    {
        int randNum = Random.Range(0, listComboAttacks.Count); //����Ʈ�� ��ϵ� �޺� ��ų ���� ����

        string combo = listComboAttacks[randNum]; //������ ��ȣ�� ���Ͽ� �޺��� �� ��ų �ִϸ��̼� �̸� ����

        animator.Play("Combo01"); //�÷���
    }

    public void A_OnIsCombo()
    {
        boss.P_SetIsCombo(true);
    }

    public void A_OffIsCombo()
    {
        boss.P_SetIsCombo(false);
    }
}
