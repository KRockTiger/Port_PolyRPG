using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Enemy enemy;
    private Animator animator;

    [SerializeField] private string hit;
    [SerializeField] private string hitting;
    [SerializeField] private string moving;
    [SerializeField] private string attack;
    [SerializeField] private string die;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
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
        if (enemy.P_GetIsChase() && !enemy.P_GetAttackAble())
        { 
            animator.SetBool(moving, true);
        }

        else
        {
            animator.SetBool(moving, false);
        }
    }

    /// <summary>
    /// �ǰ� ���� ��
    /// </summary>
    private void A_Hitting()
    {
        animator.SetBool(hitting, true);
    }

    /// <summary>
    /// �ǰ� ���� ��
    /// </summary>
    private void A_UnHitting()
    {
        animator.SetBool(hitting, false);
    }

    /// <summary>
    /// ���� ���� ���·� �����
    /// </summary>
    private void A_IsAttacking()
    {
        enemy.P_SetIsAttacking(true);
    }

    /// <summary>
    /// ���� ���� �ƴ� ���·� �����
    /// </summary>
    private void A_UnIsAttacking()
    {
        enemy.P_SetIsAttacking(false);
    }

    public void P_SetPlay_Hit()
    {
        animator.Play(hit);
    }

    public void P_GoAttack()
    {
        animator.SetTrigger(attack);
    }

    public void P_SetPlay_Die()
    {
        animator.Play(die);
    }
}
