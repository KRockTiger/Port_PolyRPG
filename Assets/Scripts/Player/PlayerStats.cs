using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 모든 씬에서 스텟은 공유를 하기 때문에 싱글톤으로 설정함
/// </summary>
public class PlayerStats : MonoBehaviour
{
    public PlayerStats Instance; //싱글톤으로 설정

    [SerializeField] private float setHP; //초기 체력 설정
    [SerializeField] private float curHP; //현재 체력
    [SerializeField] private float maxHP; //최대 체력
    [SerializeField] private float moveSpeed; //플레이어의 이동속도
    [SerializeField] private float attackRange; //플레이어의 공격범위
    [SerializeField] private float setAttackPoint; //설정할 캐릭터 공격력
    [SerializeField] private float curAttackPoint; //현재 캐릭터 공격력
    [SerializeField] private float weaponAttackPoint; //추가되는 무기 공격력
    [SerializeField] private float setDashCoolTime; //설정한 대쉬 쿨타임
    [SerializeField] private float curDashCoolTime; //현재 대쉬 쿨타임
    [SerializeField, Range(0,5)] private int dashCount; //차감하여 대쉬 쓰게하는 대쉬 카운트
    private int beforeDashCount;

    [Header("UI표기")]
    [SerializeField] private GameObject objDashCounts; //이미지 부모 오브젝트
    [SerializeField] private Image[] imgDashCounts; //위의 자식 오브젝트들을 담을 배열 변수
    [SerializeField] private GameObject[] objDashCount; //위의 자식 오브젝트들을 담을 배열 변수
    [SerializeField] private Image fillDashImg;
    [SerializeField] private int maxDashCount = 5;

    [Header("임의로 저장하는 스탯UI")]
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
    /// 실시간으로 대쉬 카운트 확인하고 관리
    /// </summary>
    private void CheckDashCount()
    {
        if (dashCount < 5)
        {
            curDashCoolTime -= Time.deltaTime;

            if (curDashCoolTime <= 0f) //쿨타임이 끝나면
            {
                dashCount++; //1회복
                curDashCoolTime = setDashCoolTime; //쿨타임 재설정
            }
        }
        #region 이전 코드
        //대쉬 카운트에 따라 이미지 적용
        //대쉬 이미지는 카운트 갯수에 따라 0번 오브젝트 부터 비활성화
        //if (dashCount > 0 && dashCount < 5) //대쉬 카운트가 일정 수량만 가지고 있을 때
        //{
        //    for (int i = 4; i >= 5 - dashCount; i--) //끝부분 부터 활성화
        //    {
        //        imgDashCounts[i].gameObject.SetActive(true);
        //    }

        //    for (int i = 0; i < 5 - dashCount; i++) //첫부분 부터 비활성화
        //    {
        //        imgDashCounts[i].gameObject.SetActive(false);
        //    }
        //}

        //else if (dashCount == 0) //카운트가 다 소모됬다면
        //{
        //    for (int i = 0; i < 5; i++)
        //    {
        //        imgDashCounts[i].gameObject.SetActive(false); //다 비활성화
        //    }
        //}

        //else if (dashCount == 5) //카운트가 다 회복됬다면
        //{
        //    for (int i = 0; i < 5; i++)
        //    {
        //        imgDashCounts[i].gameObject.SetActive(true); //다 활성화
        //    }
        //}
        #endregion

        if (beforeDashCount != dashCount) //카운트 정보가 바뀌어지면
        {
            beforeDashCount = dashCount; //카운트 정보 최신화시키고
            ModifyDisplayCoolTime(); //이미지 변화 시키기
        }
    }

    private void StatUI()
    {
        setAttackText.text = "기본 공격력\n" + setAttackPoint.ToString();
        weaponAttackText.text = "무기 공격력\n" + weaponAttackPoint.ToString();
        curAttackText.text = "현재 공격력\n" + curAttackPoint.ToString();
    }

    /// <summary>
    /// 대쉬 카운트가 일정 시간마다 바뀌어지면 사용되는 함수
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
    /// 대쉬기능에서 사용하여 카운트 감소
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
        weaponAttackPoint = _weaponAttackPoint; //장비 슬롯 무기의 공격력을 가져옴
        curAttackPoint = setAttackPoint + weaponAttackPoint; //설정 공격력에 무기 공격력을 더하여 현재 캐릭터 공격력을 저장
    }

    /// <summary>
    /// 캐릭터 체력을 회복활 때 사용
    /// </summary>
    /// <param name="_setHP"></param>
    public void P_HealHP(float _setHP)
    {
        curHP += _setHP;

        if (curHP >= maxHP) //회복한 체력이 최대 체력보다 높으면
        {
            curHP = maxHP; //최대 체력으로 설정
        }
    }

    /// <summary>
    /// 플레이어가 피격 받았을 때 사용
    /// </summary>
    public void P_Hit(float _damage)
    {
        curHP -= _damage;
    }
}
