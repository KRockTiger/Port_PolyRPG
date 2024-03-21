using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 모든 씬에서 스텟은 공유를 하기 때문에 싱글톤으로 설정함
/// </summary>
public class PlayerStats : MonoBehaviour
{
    public PlayerStats Instance; //싱글톤으로 설정

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
}
