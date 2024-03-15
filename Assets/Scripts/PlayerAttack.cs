using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //적을 탐지하기 위해 만들어둔 변수
    private float shortDistance; //제일 가까운 적과의 거리
    private Transform targetEnemy; //위치에 따라 공격을 하기 위함으로 Transform을 사용

    private PlayerMove playerMove;

    [Header("플레이어 공격 스텟")]
    [SerializeField] private float attackRange; //공격범위

    [SerializeField] private bool isAttack; //플레이어의 공격을 담당
    [SerializeField] private bool isGround; //플레이어가 땅에 있는 지 유무
    [SerializeField] private bool isNext; //다음 공격 가능하게 하는 트리거
    [SerializeField] private bool nextAttack; //연속 공격 사용

    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
    }

    private void Update()
    {
        Attack();
        SetIsAttack();
        GetIsGround();
        SearchEnemy();
    }

    /// <summary>
    /// 플레이어의 공격을 담당하는 함수
    /// </summary>
    private void Attack()
    {
        //플레이어가 땅에 밝아있는 상태에서 공격 중인 상태가 아닐 때 공격 ==> 나중에 연속 공격을 넣게 되면 추후 수정
        if (Input.GetKeyDown(KeyCode.Mouse0) && isGround)
        {
            //목표 몬스터가 근처에 있을 경우  바로 회전하여 공격
            if (targetEnemy != null) //만약 목표 몬스터가 존재하지 않을 경우 예외 처리
            {
                //목표 몬스터와 내 캐릭터의 거리를 계산
                float distance = Vector3.Distance(targetEnemy.transform.position, transform.position);

                if (distance < attackRange) //계산한 거리가 공격 범위 안에 들 경우
                {
                    LockOn(); //목표 몬스터를 향해 회전
                }
            }

            if (!isAttack) //공격 중이 아닐 경우(지금 공격이 첫 공격일 경우)
            {
                isAttack = true;
            }

            else if (isAttack && isNext) //공격 중 다음 공격이 가능해졌을 때
            {
                nextAttack = true;
            }
        }
    }

    /// <summary>
    /// 주위에 있는 몬스터를 탐지
    /// </summary>
    private void SearchEnemy()
    {
        //"Enemy"태그를 단 모든 몬스터를 담아둠
        //근데 씬에 있는 모든 몬스터를 서치하는 코드라서 만약 몬스터 수를 수없이 늘리면
        //기능에 지장이 있을 거라 생각됨
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        shortDistance = Mathf.Infinity; //처음에는 제일 큰 수를 넣어둠

        Transform nearEnemy = null; //제일 가까운 적을 담을 변수 생성

        foreach (GameObject enemy in enemies)
        {
            //모든 적과의 거리를 비교한다.
            float distance = Vector3.Distance(enemy.transform.position, transform.position);

            if (distance < shortDistance) //현재 비교한 거리가 저장된 짧은 거리보다 짧을 경우
            {
                shortDistance = distance; //새로 갱신

                nearEnemy = enemy.transform; //제일 가까운 적 위치 갱신
            }

            if (nearEnemy != null) //근처에 적이 있을 경우
            {
                targetEnemy = nearEnemy; //타겟 설정
            }

            else //만약 없을 경우
            {
                targetEnemy = null; //타겟을 비움
            }
        }
    }

    /// <summary>
    /// 내 캐릭터가 몬스터를 향해 회전하는 함수
    /// </summary>
    private void LockOn()
    {
        if (targetEnemy != null) //타겟이 비지 않았을 때
        {
            Vector3 direction = targetEnemy.transform.position - transform.position; //방향 설정
            Quaternion lookRotate = Quaternion.LookRotation(direction); //특정 방향으로 돌아가게 변수 설정
            //Vector3 look = lookRotate.eulerAngles;

            //위 쿼터니언에서 계산한 y값을 eulerAngle로 가져와서 회전
            transform.rotation = Quaternion.Euler(0f, lookRotate.eulerAngles.y, 0f);
        }
    }

    /// <summary>
    /// PlayerMove 스크립트의 isGround 값을 계속 가져옴
    /// </summary>
    private void GetIsGround()
    {
        isGround = playerMove.P_GetIsGround();
    }

    /// <summary>
    /// 플레이어 공격에 따라 PlayerMove스크립트의 IsAttacking 변수 설정
    /// </summary>
    private void SetIsAttack()
    {
        playerMove.P_SetIsAttacking(isAttack);
    }

    /// <summary>
    /// PlayerAttack의 isAttack 값을 설정하기
    /// </summary>
    public void P_SetIsAttack(bool _isAttack)
    {
        isAttack = _isAttack;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_isNext"></param>
    public void P_SetIsNext(bool _isNext)
    {
        isNext = _isNext;
    }

    /// <summary>
    /// PlayerAttack의 isAttack 값을 가져오기
    /// </summary>
    /// <returns></returns>
    public bool P_GetIsAttack()
    {
        return isAttack;
    }

    public bool P_GetNextAttack()
    {
        return nextAttack;
    }
}
