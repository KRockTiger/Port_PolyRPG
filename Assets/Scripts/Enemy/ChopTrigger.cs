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
            Boss scBoss = GetComponentInParent<Boss>(); //�θ� ������Ʈ���� Boss ��ũ��Ʈ ��������
            PlayerMove scMove = other.GetComponent<PlayerMove>();
            PlayerStats scStats = other.GetComponent<PlayerStats>();
            PlayerAnimation scAnimation = other.GetComponent<PlayerAnimation>();
            
            scMove.P_SetGroggy(); //�÷��̾ �׷α� ���·� �����
            scAnimation.PA_PlayKnockdownAnimation(); //��ٿ� �ִϸ��̼� ���� ����
            (float, float, float) bossStats = scBoss.P_GetStats();
            scStats.P_Hit(bossStats.Item1, bossStats.Item2, bossStats.Item3); //(���ݷ�, �����, �����) ������

            //scMove.P_CompulsionOffBattle(); //�÷��̾� ���� ��� ����
            //scMove.P_SetBounce(bounceForce); //�÷��̾� ƨ���
        }
    }
}
