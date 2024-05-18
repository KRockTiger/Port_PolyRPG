using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Ҹ�ǰ �������� Ŭ�� ���ݰ� ��� ��Ÿ���� �߽����� �����ϴ� ��ũ��Ʈ
/// </summary>
public class ItemController : MonoBehaviour
{
    public static ItemController Instance;

    public enum CoolType
    {
        HealPostion, //ȸ�� ������
        AttackPostion, //���ݷ� ���� ������
    }

    [System.Serializable]
    public class CoolItemType
    {
        public CoolType coolType; //������ ���� ��Ÿ�� ����
        public float setCoolTime; //������ ��Ÿ��
        public float curCoolTime; //���� ��Ÿ��
        public bool isCool; //��Ÿ�� ���� Ȯ�� => ���� true�� ��� ������ ��� �Ұ�
    }

    [SerializeField] private List<CoolItemType> coolItemTypes;

    private void Awake()
    {
        #region �̱���
        if (Instance == null)
        {
            Instance = this;
        }

        else
        {
            Destroy(gameObject);
        }
        #endregion
    }

    private void Update()
    {
        UseCoolDown();
    }

    /// <summary>
    /// �� ��� �������� �������� ����Ʈ�� ����Ͽ� ��Ÿ�� ���� ����
    /// </summary>
    private void UseCoolDown()
    {
        foreach (CoolItemType coolItemType in coolItemTypes)
        {
            if (coolItemType.isCool) //��Ÿ�� ������ �Ǹ�
            {
                coolItemType.curCoolTime -= Time.deltaTime; //�ð� ����
                //�� �ڵ�� TimeScale�� ������ �޴� �ڵ��ε� �κ��丮�� ������ ���缭 �ǵ������� �ð��� ���� �÷��̸� ���� ���Ͽ�
                //�κ��丮 ��� �� �϶��� ��Ÿ���� �پ���� �ʴ´�.

                if (coolItemType.curCoolTime <= 0f) //�ð��� ������
                {
                    coolItemType.isCool = false; //��Ÿ�� ����
                }
            }
        }
    }

    /// <summary>
    /// Ư�� ������ �������� ��Ÿ�� ����
    /// </summary>
    /// <param name="_itemSmallType"></param>
    public void P_CoolOn(string _itemSmallType)
    {
        for (int i = 0; i < coolItemTypes.Count; i++)
        {
            //Ÿ ��ũ��Ʈ�� ����Ͽ� �Ҹ� ������ ������ stringȭ �Ͽ� ������ �� �ٽ� enum���� �������� ���Ͽ� ����
            if(coolItemTypes[i].coolType == (CoolType)Enum.Parse(typeof(CoolType), _itemSmallType))
            {
                coolItemTypes[i].curCoolTime = coolItemTypes[i].setCoolTime; //��Ÿ�� �ð� ����
                coolItemTypes[i].isCool = true; //��Ÿ�� ����
                return; //�ߺ��Ǵ� ������ �����Ƿ� �ٷ� ���� ���Ѽ� �ڵ� ���� ��Ű��
            }
        }

        #region switch����
        //switch (_itemSmallType)
        //{
        //    case "HealPostion": //ȸ�� �������� ���
        //        coolItemTypes[0].curCoolTime = coolItemTypes[0].setCoolTime; //��Ÿ�� �ð� ����
        //        coolItemTypes[0].isCool = true; //��Ÿ�� ����
        //        break;
        //}
        #endregion
    }

    /// <summary>
    /// Ư�� ������ ������ ��Ÿ�� ������ �����Ͽ� ������ ��� ������ �� ���
    /// </summary>
    /// <param name="_itemSmallType"></param>
    /// <returns></returns>
    public bool P_SearchCoolType(string _itemSmallType)
    {
        for (int i = 0; i < coolItemTypes.Count; i++)
        {
            //Ÿ ��ũ��Ʈ�� ����Ͽ� �Ҹ� ������ ������ stringȭ �Ͽ� ������ �� �ٽ� enum���� �������� ���Ͽ� ����
            if (coolItemTypes[i].coolType == (CoolType)Enum.Parse(typeof(CoolType), _itemSmallType))
            {
                return coolItemTypes[i].isCool;
            }
        }

        return false;
    }
}
