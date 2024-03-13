using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackAnimation : MonoBehaviour
{
    [Header("애니메이터 설정")]
    [SerializeField] private string attack01; //설정할 애니메이터 파라미터

    private Animator animator;
    private PlayerMove playerMove;
    [SerializeField, Tooltip("공격중일 때 True")] private bool isAttacking = false;
    [SerializeField, Tooltip("땅에있을 때 True")] private bool isGround = false;

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
    /// 플레이어가 공격하는 기능
    /// </summary>
    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttacking && isGround)
        {
            animator.SetBool(attack01, true); //공격 애니메이션 사용
        }
    }

    /// <summary>
    /// 플레이어 공격에 따라 PlayerMove스크립트의 IsAttacking 변수 설정
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
    /// 애니메이션에 넣을 함수
    /// isAttack을 true로 바꿔 공격중이라는 것을 확인시키게 함
    /// </summary>
    private void A_IsAttack()
    {
        isAttacking = true;
    }

    /// <summary>
    /// 애니메이션에 넣을 함수
    /// isAttack을 false로 바꿔 공격이 끝난것을 확인시키게 함
    /// </summary>
    private void A_UnIsAttack()
    {
        isAttacking = false;
        animator.SetBool(attack01, isAttacking);
    }
}
