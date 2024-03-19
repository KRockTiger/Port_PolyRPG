using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private string hit;
    [SerializeField] private string hitting;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// 피격 중일 때
    /// </summary>
    private void A_Hitting()
    {
        animator.SetBool(hitting, true);
    }

    /// <summary>
    /// 피격 끝날 때
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
