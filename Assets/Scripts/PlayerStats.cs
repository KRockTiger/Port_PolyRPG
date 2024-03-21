using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��� ������ ������ ������ �ϱ� ������ �̱������� ������
/// </summary>
public class PlayerStats : MonoBehaviour
{
    public PlayerStats Instance; //�̱������� ����

    [SerializeField] private float moveSpeed; //�÷��̾��� �̵��ӵ�
    [SerializeField] private float attackRange; //�÷��̾��� ���ݹ���
    [SerializeField] private float setDashCoolTime; //������ �뽬 ��Ÿ��
    [SerializeField] private float curDashCoolTime; //���� �뽬 ��Ÿ��
    [SerializeField] private int dashCount; //�����Ͽ� �뽬 �����ϴ� �뽬 ī��Ʈ

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        dashCount = 5;
    }

    private void Update()
    {
        CheckDashCount();
    }

    /// <summary>
    /// �ǽð����� �뽬 ī��Ʈ Ȯ��
    /// </summary>
    private void CheckDashCount()
    {
        if (dashCount < 5)
        {
            curDashCoolTime -= Time.deltaTime;
            
            if (curDashCoolTime <= 0f) //��Ÿ���� ������
            {
                dashCount += 1; //1ȸ��
                curDashCoolTime = setDashCoolTime; //��Ÿ�� �缳��
            }
        }
    }
}
