using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHPUI : MonoBehaviour
{
    private GameObject player;
    private PlayerStats playerStats;

    [SerializeField] private GameObject frontHP; //현재 플레이어의 맨 앞 체력
    [SerializeField] private GameObject midHP; //연출용 중간 체력
    [SerializeField] private GameObject backHP; //비어있는 뒷 체력

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();
    }
}
