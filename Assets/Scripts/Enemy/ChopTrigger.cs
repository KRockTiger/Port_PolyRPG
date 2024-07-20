using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopTrigger : MonoBehaviour
{
    [SerializeField, Range(0, 10)] private float bounceForce; //�÷��̾ ƨ������ ����

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerMove scMove = other.GetComponent<PlayerMove>();
            PlayerAnimation scAnimation = other.GetComponent<PlayerAnimation>();
            //scMove.P_SetBounce(bounceForce); //�÷��̾� ƨ���
            scAnimation.PA_PlayKnockdownAnimation(); //��ٿ� �ִϸ��̼� ���� ����
            scMove.P_SetGroggy(); //�÷��̾ �׷α� ���·� �����
            scMove.P_CompulsionOffBattle(); //�÷��̾� ���� ��� ����
        }
    }
}
