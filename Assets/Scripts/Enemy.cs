using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float setHP; //������ ü��
    [SerializeField] private float curHP; //���� ü��

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Attack")
        {
            Debug.Log("������ �¾ҽ��ϴ�.");
        }
    }

    private void Awake()
    {
        curHP = setHP; //���� ü�� ����
    }
}
