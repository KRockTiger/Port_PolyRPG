using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHPUI : MonoBehaviour
{
    private GameObject player;
    private PlayerStats playerStats;

    [SerializeField] private GameObject frontHP; //현재 플레이어의 맨 앞 체력
    [SerializeField] private GameObject midHP; //연출용 중간 체력
    [SerializeField] private GameObject backHP; //비어있는 뒷 체력
    [SerializeField] private TMP_Text hpText; //체력 텍스트

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();
    }

    private void Update()
    {
        HPUpdate();
    }

    /// <summary>
    /// 플레이어의 체력 상태를 UI의 텍스트로 표시
    /// </summary>
    private void HPUpdate()
    {
        (float, float) hpStats = playerStats.P_GetHPStats(); //플레이어의 (현재 체력, 최대 체력) 상태를 가져오기

        hpText.text = $"{hpStats.Item1} / {hpStats.Item2}"; //체력 상태를 항상 최신화하여 표기
    }
}
