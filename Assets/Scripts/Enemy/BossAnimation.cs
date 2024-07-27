using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimation : MonoBehaviour
{
    private Boss boss;
    private Animator animator;

    [SerializeField] private string moving;
    [SerializeField] private List<bool> testCombos; //콤보 스킬을 테스트 하기 위한 리스트

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
    /// 몬스터 움직임 애니메이션 관리
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
    /// 임의의 번호를 지정하여 콤보 스킬 결정하기
    /// -매개 변수는 특정 콤보를 확인하기 위해 분리해서 사용하기 위한 수단이므로 추후 수정 필요
    /// </summary>
    public void P_AnimPlayComboAttack(string _comboName)
    {
        int randNum = Random.Range(0, listComboAttacks.Count); //리스트에 등록된 콤보 스킬 수로 결정

        string combo = listComboAttacks[randNum]; //임의의 번호를 정하여 콤보로 쓸 스킬 애니메이션 이름 결정

        animator.Play(_comboName); //플레이
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
    /// 보스 스크립트에 등록된 무기 메쉬 콜리더를 true로 설정하여 콜리더 생성
    /// </summary>
    public void A_OnWeaponCollider()
    {
        boss.P_SetWeaponCollider(true);
    }

    /// <summary>
    /// 반대로 무기 메쉬 콜리더를 false로 설정하여 콜리더 제거
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
