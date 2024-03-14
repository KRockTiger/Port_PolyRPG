using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackRange; //���ݹ���
    private float shortDistance; //���� ����� ������ �Ÿ�
    [SerializeField] private Transform targetEnemy; //��ġ�� ���� ������ �ϱ� �������� Transform�� ���

    private void Update()
    {
        SearchEnemy();
        LockOn();
    }

    /// <summary>
    /// ������ �ִ� ���͸� Ž��
    /// </summary>
    private void SearchEnemy()
    {
        //"Enemy"�±׸� �� ��� ���͸� ��Ƶ�
        //�ٵ� ���� �ִ� ��� ���͸� ��ġ�ϴ� �ڵ�� ���� ���� ���� ������ �ø���
        //��ɿ� ������ ���� �Ŷ� ������
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        shortDistance = Mathf.Infinity; //ó������ ���� ū ���� �־��

        Transform nearEnemy = null; //���� ����� ���� ���� ���� ����

        foreach (GameObject enemy in enemies)
        {
            //��� ������ �Ÿ��� ���Ѵ�.
            float distance = Vector3.Distance(enemy.transform.position, transform.position);

            if (distance < shortDistance) //���� ���� �Ÿ��� ����� ª�� �Ÿ����� ª�� ���
            {
                shortDistance = distance; //���� ����

                nearEnemy = enemy.transform; //���� ����� �� ��ġ ����
            }

            if (nearEnemy != null) //��ó�� ���� ���� ���
            {
                targetEnemy = nearEnemy; //Ÿ�� ����
            }
            
            else //���� ���� ���
            {
                targetEnemy = null; //Ÿ���� ���
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
