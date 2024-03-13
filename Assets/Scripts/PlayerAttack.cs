using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float searchRange; //적 탐지 범위

    [SerializeField] private GameObject[] targets;

    private void Update()
    {
        SearchEnemy();
    }

    /// <summary>
    /// 주위에 있는 몬스터를 탐지
    /// </summary>
    private void SearchEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        foreach (GameObject enemy in enemies)
        {
            Vector3 enemyRange = enemy.transform.position - transform.position;

            float distance = Mathf.Infinity;

            if (enemyRange.magnitude < distance)
            {
                distance = enemyRange.magnitude;
            }

            if (distance < searchRange)
            {
                GameObject target = enemy;
            }
        }
    }
}
