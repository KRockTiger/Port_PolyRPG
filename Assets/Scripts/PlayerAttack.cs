using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackRange; //공격범위
    private float shortDistance; //제일 가까운 적과의 거리
    [SerializeField] private Transform targetEnemy; //위치에 따라 공격을 하기 위함으로 Transform을 사용

    private void Update()
    {
        SearchEnemy();
        LockOn();
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

    private void LockOn()
    {
        if (targetEnemy != null)
        {
            float distance = Vector3.Distance(targetEnemy.transform.position, transform.position);

            if (distance < attackRange)
            {
                Vector3 direction = targetEnemy.transform.position - transform.position;
                Quaternion lookRotate = Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.Euler(0f, lookRotate.y, 0f);
            }
        }
    }
}
