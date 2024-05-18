using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolTimeManager : MonoBehaviour
{
    public static CoolTimeManager Instance;

    public enum CoolType
    {
        UseCool, //�ߺ� �Է� ����
        Postion, //ȸ�� ������
    }

    [SerializeField] private CoolType coolType;

    [System.Serializable]
    public class Class_CoolTime
    {
        public CoolType coolType; //��Ÿ�� ����
        public float setCoolTime; //������ ��Ÿ��
        public float curCoolTime; //���� ��Ÿ��
        public bool isCool; //��Ÿ�� ���� Ȯ��
    }

    [SerializeField] private List<Class_CoolTime> coolTimeList;

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

    private void UseCoolDown()
    {
        if (coolTimeList[0].curCoolTime <= 0f)
        {
            coolTimeList[0].isCool = false;
            return;
        }

        coolTimeList[0].curCoolTime -= Time.deltaTime;
    }
}
