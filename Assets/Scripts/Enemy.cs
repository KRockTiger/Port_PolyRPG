using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float setHP; //설정할 체력
    [SerializeField] private float curHP; //현재 체력

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Attack")
        {
            Debug.Log("공격을 맞았습니다.");
        }
    }

    private void Awake()
    {
        curHP = setHP; //현재 체력 설정
    }
}
