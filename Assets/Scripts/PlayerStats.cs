using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 모든 씬에서 스텟은 공유를 하기 때문에 싱글톤으로 설정함
/// </summary>
public class PlayerStats : MonoBehaviour
{
    public PlayerStats Instance; //싱글톤으로 설정

    [SerializeField] private float setHP; //초기 체력 설정
    [SerializeField] private float curHP; //현재 체력
    [SerializeField] private float maxHP; //최대 체력
    [SerializeField] private float moveSpeed; //플레이어의 이동속도
    [SerializeField] private float attackRange; //플레이어의 공격범위
    [SerializeField] private float setDashCoolTime; //설정한 대쉬 쿨타임
    [SerializeField] private float curDashCoolTime; //현재 대쉬 쿨타임
    [SerializeField] private int dashCount; //차감하여 대쉬 쓰게하는 대쉬 카운트

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        dashCount = 5;
        curHP = setHP;
    }

    private void Update()
    {
        CheckDashCount();
    }

    /// <summary>
    /// 실시간으로 대쉬 카운트 확인
    /// </summary>
    private void CheckDashCount()
    {
        if (dashCount < 5)
        {
            curDashCoolTime -= Time.deltaTime;
            
            if (curDashCoolTime <= 0f) //쿨타임이 끝나면
            {
                dashCount += 1; //1회복
                curDashCoolTime = setDashCoolTime; //쿨타임 재설정
            }
        }
    }
    
    /// <summary>
    /// 캐릭터 체력을 회복활 때 사용
    /// </summary>
    /// <param name="_setHP"></param>
    public void P_SetHP(float _setHP)
    {
        curHP += _setHP;

        if (curHP >= maxHP) //회복한 체력이 최대 체력보다 높으면
        {
            curHP = maxHP; //최대 체력으로 설정
        }
    }
}
