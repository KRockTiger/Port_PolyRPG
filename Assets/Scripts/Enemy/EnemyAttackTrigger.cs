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
            float damage = enemy.P_GetAttackPoint();
            sc.P_Hit(damage); //������ ���ݷ��� ������
        }
    }

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }
}
