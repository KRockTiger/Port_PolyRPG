using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyAnimation enemyAnimation;

    [SerializeField] private float setHP; //������ ü��
    [SerializeField] private float curHP; //���� ü��

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
        curHP = setHP; //���� ü�� ����
    }

    private void Update()
    {

    }

    /// <summary>
    /// �ǰ� ����
    /// </summary>
    private void Hit()
    {
        Debug.Log("������ �¾ҽ��ϴ�.");
    }
}
