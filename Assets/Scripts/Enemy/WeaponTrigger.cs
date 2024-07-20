using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTrigger : MonoBehaviour
{
    //무기를 휘둘러서 플레이어를 맞히면 플레이어가 잠깐 넉백됨
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("공격에 맞았습니다.");
            PlayerMove scMove = other.GetComponent<PlayerMove>();
            PlayerAnimation scAnimation = other.GetComponent<PlayerAnimation>();
            scAnimation.PA_PlayGetHitAnimation(); //플레이어 피격 애니메이션 강제 실행
            scMove.P_SetGroggy();
            scMove.P_CompulsionOffBattle(); //플레이어 전투 모션 해제
        }
    }
}
