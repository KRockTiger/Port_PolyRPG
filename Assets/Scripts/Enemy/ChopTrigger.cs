using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopTrigger : MonoBehaviour
{
    [SerializeField, Range(0, 10)] private float bounceForce; //플레이어가 튕겨지는 높이

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Boss scBoss = GetComponentInParent<Boss>(); //부모 오브젝트에서 Boss 스크립트 가져오기
            PlayerMove scMove = other.GetComponent<PlayerMove>();
            PlayerStats scStats = other.GetComponent<PlayerStats>();
            PlayerAnimation scAnimation = other.GetComponent<PlayerAnimation>();
            
            scMove.P_SetGroggy(); //플레이어를 그로기 상태로 만들기
            scAnimation.PA_PlayKnockdownAnimation(); //녹다운 애니메이션 강제 실행
            (float, float, float) bossStats = scBoss.P_GetStats();
            scStats.P_Hit(bossStats.Item1, bossStats.Item2, bossStats.Item3); //(공격력, 관통력, 관통률) 데이터

            //scMove.P_CompulsionOffBattle(); //플레이어 전투 모션 해제
            //scMove.P_SetBounce(bounceForce); //플레이어 튕기기
        }
    }
}
