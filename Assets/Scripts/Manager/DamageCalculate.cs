using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculate : MonoBehaviour
{
    [Header("데미지 계산 설정할 변수")]
    [SerializeField, Tooltip("나의 공격력이 아닌 상대에게 받는 공격력")] float attackPoint; //공격력
    [SerializeField, Tooltip("나의 방어력")] float defendPoint; //방어력
    [SerializeField, Tooltip("임의로 지정할 방어상수(비율 조정용)")] float defendConstant; //방어상수

    [Header("계산을 담을 변수들(확인용으로 조정X)")]
    [SerializeField, Tooltip("방어력과 방어상수로 비율 계산(확인용)")] float defendPercent; //방어율
    [SerializeField, Tooltip("위 수치들로 인하여 받는 데미지(확인용)")] float damage; //데미지
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //방어율(피해흡수량) = 방어력 / (방어율 + 방어상수)
            defendPercent = defendPoint / (defendPoint + defendConstant);

            //데미지 = 공격력 * (1 - 방어율)
            damage = attackPoint * (1 - defendPercent);

            Debug.Log($"계산한 방어율 : {defendPercent}");
            Debug.Log($"계산한 데미지 : {damage}");
        }
    }
}
