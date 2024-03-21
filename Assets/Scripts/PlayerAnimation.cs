using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("파라미터 설정")]
    [SerializeField] private string attacking; //설정할 애니메이터 파라미터
    [SerializeField] private string nextAttack; //다음 공격
    [SerializeField] private string dash; //대쉬 파라미터
    [SerializeField] private string attack01; //첫 번째 공격 애니메이션

    private Animator animator;
    private PlayerMove playerMove; //플레이어 움직임 스크립트
    private PlayerAttack playerAttack; //플레이어의 공격 스크립트
    private bool notEndAttack = false;
    //[SerializeField, Tooltip("공격중일 때 True")] private bool isAttacking = false;
    //[SerializeField, Tooltip("땅에있을 때 True")] private bool isGround = false;

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
    /// 플레이어가 공격하는 기능
    /// </summary>
    private void AttackAnimation()
    {
        //처음에 쓴 조건문 : Input.GetKeyDown(KeyCode.Mouse0) && !isAttacking && isGround
        if (playerAttack.P_GetIsAttack())
        {
            animator.SetBool(attacking, true); //공격 애니메이션 사용
        }

        else
        {
            animator.SetBool(attacking, false);
        }

        if (playerAttack.P_GetNextAttack()) //다음 공격 트리거가 실행되면
        {
            animator.SetBool(nextAttack, true); //파라미터 키기
        }

        else //트리거가 종료되면
        {
            animator.SetBool(nextAttack, false); //파라미터 끄기
            
        } //=> nextAttack은 다음 공격을 실행하게 하는 bool형 값이므로 제 때 꺼야 자동 연속 공격이 막아짐
    }

    /// <summary>
    /// 대쉬 애니메이션 관리
    /// PlayerMove의 isDash 값이 true면 실행 false면 종료
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
    /// 애니메이션에 넣을 함수
    /// isAttack을 false로 바꿔 공격이 끝난것을 확인시키게 함
    /// </summary>
    private void A_UnIsAttack()
    {
        if (notEndAttack) //만약 공격 중단을 하면 안되는 상황이 나올 경우를 대비해 예외 처리를 하여 막음
        { return; }
        playerAttack.P_SetIsAttack(false); //PlayerAttack 스크립트의 isAttack값을 false로 설정
        playerAttack.P_SetIsNext(false); //공격이 끊겼으므로 다음 공격 트리거 끄기
        playerAttack.P_ReSetAttackCycle(); //연속 공격 횟수 초기화
        //animator.SetBool(attacking, false); //공격 애니메이션 끄기
    }

    /// <summary>
    /// 다음 공격을 사용하기 위해 애니메이션 기준으로 트리거 활성화
    /// </summary>
    private void A_IsNext()
    {
        //03.21) 공격 중 캔슬 했을 때 절묘한 타이밍에 현 코드가 작동되어 연속 공격이 빠르게 일어나는 버그가 있음
        if (playerAttack.P_GetIsAttack()) //따라서 캐릭터가 공격 중일 상태에만 코드가 작동되게 변경
        {
            playerAttack.P_SetIsNext(true);
        }
    }

    /// <summary>
    /// 다음 공격을 사용하지 못하게 끄기
    /// </summary>
    private void A_UnIsNext()
    {
        playerAttack.P_SetIsNext(false);
        StartCoroutine(C_NotEndAttack()); //공격이 갑자기 중단되는 버그를 방지하기 위한 처리
        //animator.SetBool(nextAttack, false);
    }

    /// <summary>
    /// 공격 애니메이션 초기에 횟수 쌓기
    /// </summary>
    private void A_SetAttackCycle()
    {
        playerAttack.P_SetAttackCycle();
    }

    /// <summary>
    /// 공격 중단을 막기 위한 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator C_NotEndAttack()
    {
        notEndAttack = true;
        yield return new WaitForSeconds(0.3f);
        notEndAttack = false;
    }

    /// <summary>
    /// 공격 애니메이션이 끝나면 스택 초기화 하기
    /// </summary>
    //private void A_ReSetAttackCycle()
    //{
    //    playerAttack.P_ReSetAttackCycle();
    //}

    /// <summary>
    /// 애니메이션에 넣을 함수
    /// isAttack을 true로 바꿔 공격중이라는 것을 확인시키게 함
    /// </summary>
    //private void A_IsAttack()
    //{
    //    isAttacking = true;
    //}
}
