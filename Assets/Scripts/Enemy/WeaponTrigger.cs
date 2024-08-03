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
            Boss scBoss = GetComponentInParent<Boss>(); //부모 오브젝트에서 Boss 스크립트 가져오기
            PlayerMove scMove = other.GetComponent<PlayerMove>();
            PlayerStats scStats = other.GetComponent<PlayerStats>();
            PlayerAnimation scAnimation = other.GetComponent<PlayerAnimation>();

            scMove.P_SetGroggy(); //그로기 가지기
            scAnimation.PA_PlayGetHitAnimation(transform.position); //플레이어 피격 애니메이션 강제 실행
            (float, float, float) bossStats = scBoss.P_GetStats();
            scStats.P_Hit(bossStats.Item1, bossStats.Item2, bossStats.Item3); //(공격력, 관통력, 관통률) 데이터

            //scMove.P_CompulsionOffBattle(); //플레이어 전투 모션 해제            
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

            Debug.Log($"공격력은 {attackPoint}, 관통력은 {piercePoint}, 관통률은 {piercePercent}");
        }
    }
}
