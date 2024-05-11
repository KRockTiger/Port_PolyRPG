using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��� ������ ������ ������ �ϱ� ������ �̱������� ������
/// </summary>
public class PlayerStats : MonoBehaviour
{
    public PlayerStats Instance; //�̱������� ����

    [SerializeField] private float setHP; //�ʱ� ü�� ����
    [SerializeField] private float curHP; //���� ü��
    [SerializeField] private float maxHP; //�ִ� ü��
    [SerializeField] private float moveSpeed; //�÷��̾��� �̵��ӵ�
    [SerializeField] private float attackRange; //�÷��̾��� ���ݹ���
    [SerializeField] private float setAttackPoint; //������ ĳ���� ���ݷ�
    [SerializeField] private float curAttackPoint; //���� ĳ���� ���ݷ�
    [SerializeField] private float weaponAttackPoint; //�߰��Ǵ� ���� ���ݷ�
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

        dashCount = 5;
        curHP = setHP;
        curAttackPoint = setAttackPoint;
    }

    private void Update()
    {
        CheckDashCount();
    }

    /// <summary>
    /// �ǽð����� �뽬 ī��Ʈ Ȯ���ϰ� ����
    /// </summary>
    private void CheckDashCount()
    {
        if (dashCount < 5)
        {
            curDashCoolTime -= Time.deltaTime;
            
            if (curDashCoolTime <= 0f) //��Ÿ���� ������
            {
                dashCount++; //1ȸ��
                curDashCoolTime = setDashCoolTime; //��Ÿ�� �缳��
            }
        }
    }

    /// <summary>
    /// �뽬��ɿ��� ����Ͽ� ī��Ʈ ����
    /// </summary>
    public void P_UseDashCount()
    {
        dashCount--;
    }

    public int P_GetDashCount()
    {
        return dashCount;
    }

    public void P_SetWeaponAttackPoint(float _weaponAttackPoint)
    {
        weaponAttackPoint = _weaponAttackPoint; //��� ���� ������ ���ݷ��� ������
        curAttackPoint = setAttackPoint + weaponAttackPoint; //���� ���ݷ¿� ���� ���ݷ��� ���Ͽ� ���� ĳ���� ���ݷ��� ����
    }
    
    /// <summary>
    /// ĳ���� ü���� ȸ��Ȱ �� ���
    /// </summary>
    /// <param name="_setHP"></param>
    public void P_HealHP(float _setHP)
    {
        curHP += _setHP;

        if (curHP >= maxHP) //ȸ���� ü���� �ִ� ü�º��� ������
        {
            curHP = maxHP; //�ִ� ü������ ����
        }
    }
}
