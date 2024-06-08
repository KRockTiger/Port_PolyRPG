using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField, Range(0,5)] private int dashCount; //�����Ͽ� �뽬 �����ϴ� �뽬 ī��Ʈ
    private int beforeDashCount;

    [Header("UIǥ��")]
    [SerializeField] private GameObject objDashCounts; //�̹��� �θ� ������Ʈ
    [SerializeField] private Image[] imgDashCounts; //���� �ڽ� ������Ʈ���� ���� �迭 ����
    [SerializeField] private GameObject[] objDashCount; //���� �ڽ� ������Ʈ���� ���� �迭 ����
    [SerializeField] private Image fillDashImg;
    [SerializeField] private int maxDashCount = 5;

    [Header("���Ƿ� �����ϴ� ����UI")]
    [SerializeField] private TMP_Text setAttackText;
    [SerializeField] private TMP_Text weaponAttackText;
    [SerializeField] private TMP_Text curAttackText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Attack")
        {
            
        }
    }

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

    private void Start()
    {
        imgDashCounts = objDashCounts.GetComponentsInChildren<Image>();
    }

    private void Update()
    {
        CheckDashCount();
        StatUI();
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
        #region ���� �ڵ�
        //�뽬 ī��Ʈ�� ���� �̹��� ����
        //�뽬 �̹����� ī��Ʈ ������ ���� 0�� ������Ʈ ���� ��Ȱ��ȭ
        //if (dashCount > 0 && dashCount < 5) //�뽬 ī��Ʈ�� ���� ������ ������ ���� ��
        //{
        //    for (int i = 4; i >= 5 - dashCount; i--) //���κ� ���� Ȱ��ȭ
        //    {
        //        imgDashCounts[i].gameObject.SetActive(true);
        //    }

        //    for (int i = 0; i < 5 - dashCount; i++) //ù�κ� ���� ��Ȱ��ȭ
        //    {
        //        imgDashCounts[i].gameObject.SetActive(false);
        //    }
        //}

        //else if (dashCount == 0) //ī��Ʈ�� �� �Ҹ���ٸ�
        //{
        //    for (int i = 0; i < 5; i++)
        //    {
        //        imgDashCounts[i].gameObject.SetActive(false); //�� ��Ȱ��ȭ
        //    }
        //}

        //else if (dashCount == 5) //ī��Ʈ�� �� ȸ����ٸ�
        //{
        //    for (int i = 0; i < 5; i++)
        //    {
        //        imgDashCounts[i].gameObject.SetActive(true); //�� Ȱ��ȭ
        //    }
        //}
        #endregion

        if (beforeDashCount != dashCount) //ī��Ʈ ������ �ٲ������
        {
            beforeDashCount = dashCount; //ī��Ʈ ���� �ֽ�ȭ��Ű��
            ModifyDisplayCoolTime(); //�̹��� ��ȭ ��Ű��
        }
    }

    private void StatUI()
    {
        setAttackText.text = "�⺻ ���ݷ�\n" + setAttackPoint.ToString();
        weaponAttackText.text = "���� ���ݷ�\n" + weaponAttackPoint.ToString();
        curAttackText.text = "���� ���ݷ�\n" + curAttackPoint.ToString();
    }

    /// <summary>
    /// �뽬 ī��Ʈ�� ���� �ð����� �ٲ������ ���Ǵ� �Լ�
    /// </summary>
    private void ModifyDisplayCoolTime()
    {
        //int count = objDashCount.Length;
        //for (int iNum = 0; iNum < count; ++iNum)
        //{
        //    objDashCount[iNum].SetActive(iNum < dashCount);
        //}

        fillDashImg.fillAmount = (float)dashCount / maxDashCount;
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

    public float P_GetAttackPoint()
    {
        return curAttackPoint;
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

    /// <summary>
    /// �÷��̾ �ǰ� �޾��� �� ���
    /// </summary>
    public void P_Hit(float _damage)
    {
        curHP -= _damage;
    }
}
