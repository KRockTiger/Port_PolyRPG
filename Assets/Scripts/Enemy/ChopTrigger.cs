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
            PlayerMove scMove = other.GetComponent<PlayerMove>();
            PlayerAnimation scAnimation = other.GetComponent<PlayerAnimation>();
            //scMove.P_SetBounce(bounceForce); //플레이어 튕기기
            scAnimation.PA_PlayKnockdownAnimation(); //녹다운 애니메이션 강제 실행
            scMove.P_SetGroggy(); //플레이어를 그로기 상태로 만들기
            scMove.P_CompulsionOffBattle(); //플레이어 전투 모션 해제
        }
    }
}
