using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerStats sc = other.GetComponent<PlayerStats>();
            float attackPoint = enemy.P_GetAttackPoint();
            float piercePoint = enemy.P_GetPiercePoint();
            float piercePercent = enemy.P_GetPiercePercent();
            sc.P_Hit(attackPoint, piercePoint, piercePercent); //몬스터의 공격력을 가져옴, 임시로 숫자를 넣은 상태
        }
    }

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }
}
