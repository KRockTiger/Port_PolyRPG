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
            sc.P_Hit(attackPoint, piercePoint, piercePercent); //������ ���ݷ��� ������, �ӽ÷� ���ڸ� ���� ����
        }
    }

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }
}
