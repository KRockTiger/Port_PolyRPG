using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTrigger : MonoBehaviour
{
    //���⸦ �ֵѷ��� �÷��̾ ������ �÷��̾ ��� �˹��
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Boss scBoss = GetComponentInParent<Boss>(); //�θ� ������Ʈ���� Boss ��ũ��Ʈ ��������
            PlayerMove scMove = other.GetComponent<PlayerMove>();
            PlayerStats scStats = other.GetComponent<PlayerStats>();
            PlayerAnimation scAnimation = other.GetComponent<PlayerAnimation>();

            scMove.P_SetGroggy(); //�׷α� ������
            scAnimation.PA_PlayGetHitAnimation(transform.position); //�÷��̾� �ǰ� �ִϸ��̼� ���� ����
            (float, float, float) bossStats = scBoss.P_GetStats();
            scStats.P_Hit(bossStats.Item1, bossStats.Item2, bossStats.Item3); //(���ݷ�, �����, �����) ������

            //scMove.P_CompulsionOffBattle(); //�÷��̾� ���� ��� ����            
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Boss scBoss = GetComponentInParent<Boss>();

            (float, float, float) bossStats = scBoss.P_GetStats();

            float attackPoint = bossStats.Item1;
            float piercePoint = bossStats.Item2;
            float piercePercent = bossStats.Item3;

            Debug.Log($"���ݷ��� {attackPoint}, ������� {piercePoint}, ������� {piercePercent}");
        }
    }
}
