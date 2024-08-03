using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHPUI : MonoBehaviour
{
    private GameObject player;
    private PlayerStats playerStats;

    [SerializeField] private GameObject frontHP; //���� �÷��̾��� �� �� ü��
    [SerializeField] private GameObject midHP; //����� �߰� ü��
    [SerializeField] private GameObject backHP; //����ִ� �� ü��
    [SerializeField] private TMP_Text hpText; //ü�� �ؽ�Ʈ

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
    /// �÷��̾��� ü�� ���¸� UI�� �ؽ�Ʈ�� ǥ��
    /// </summary>
    private void HPUpdate()
    {
        (float, float) hpStats = playerStats.P_GetHPStats(); //�÷��̾��� (���� ü��, �ִ� ü��) ���¸� ��������

        hpText.text = $"{hpStats.Item1} / {hpStats.Item2}"; //ü�� ���¸� �׻� �ֽ�ȭ�Ͽ� ǥ��
    }
}
