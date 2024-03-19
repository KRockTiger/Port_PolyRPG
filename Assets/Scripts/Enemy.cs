using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyAnimation enemyAnimation;

    [SerializeField] private float setHP; //설정할 체력
    [SerializeField] private float curHP; //현재 체력

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Attack")
        {
            Hit();
            enemyAnimation.P_SetTrigger_Hit();
        }

        
    }

    private void Awake()
    {
        enemyAnimation = GetComponent<EnemyAnimation>();
        curHP = setHP; //현재 체력 설정
    }

    private void Update()
    {

    }

    /// <summary>
    /// 피격 판정
    /// </summary>
    private void Hit()
    {
        Debug.Log("공격을 맞았습니다.");
    }
}
