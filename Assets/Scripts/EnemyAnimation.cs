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
        if (enemy.P_GetIsChase() && !enemy.P_GetIsAttacking())
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

    public void P_SetTrigger_Hit()
    {
        animator.SetTrigger(hit);
    }
}
