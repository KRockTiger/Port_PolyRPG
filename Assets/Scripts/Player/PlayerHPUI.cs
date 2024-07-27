using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHPUI : MonoBehaviour
{
    private GameObject player;
    private PlayerStats playerStats;

    [SerializeField] private GameObject frontHP; //���� �÷��̾��� �� �� ü��
    [SerializeField] private GameObject midHP; //����� �߰� ü��
    [SerializeField] private GameObject backHP; //����ִ� �� ü��

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();
    }
}
