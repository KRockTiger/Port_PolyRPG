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
            Debug.Log("���ݿ� �¾ҽ��ϴ�.");
            PlayerMove scMove = other.GetComponent<PlayerMove>();
            PlayerAnimation scAnimation = other.GetComponent<PlayerAnimation>();
            scAnimation.PA_PlayGetHitAnimation(); //�÷��̾� �ǰ� �ִϸ��̼� ���� ����
            scMove.P_SetGroggy();
            scMove.P_CompulsionOffBattle(); //�÷��̾� ���� ��� ����
        }
    }
}
