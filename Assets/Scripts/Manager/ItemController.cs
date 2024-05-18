using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 소모품 아이템의 클릭 간격과 사용 쿨타임을 중심으로 조정하는 스크립트
/// </summary>
public class ItemController : MonoBehaviour
{
    public static ItemController Instance;

    public enum CoolType
    {
        HealPostion, //회복 아이템
        AttackPostion, //공격력 관련 아이템
    }

    [System.Serializable]
    public class CoolItemType
    {
        public CoolType coolType; //아이템 따른 쿨타임 종류
        public float setCoolTime; //설정할 쿨타임
        public float curCoolTime; //현재 쿨타임
        public bool isCool; //쿨타임 상태 확인 => 만약 true일 경우 아이템 사용 불가
    }

    [SerializeField] private List<CoolItemType> coolItemTypes;

    private void Awake()
    {
        #region 싱글톤
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
    /// 각 사용 아이템의 종류마다 리스트에 등록하여 쿨타임 따로 적용
    /// </summary>
    private void UseCoolDown()
    {
        foreach (CoolItemType coolItemType in coolItemTypes)
        {
            if (coolItemType.isCool) //쿨타임 적용이 되면
            {
                coolItemType.curCoolTime -= Time.deltaTime; //시간 감소
                //위 코드는 TimeScale에 영향을 받는 코드인데 인벤토리로 게임을 멈춰서 의도적으로 시간을 버는 플레이를 막기 위하여
                //인벤토리 사용 중 일때는 쿨타임이 줄어들지 않는다.

                if (coolItemType.curCoolTime <= 0f) //시간이 끝나면
                {
                    coolItemType.isCool = false; //쿨타임 해제
                }
            }
        }
    }

    /// <summary>
    /// 특정 종류의 아이템의 쿨타임 적용
    /// </summary>
    /// <param name="_itemSmallType"></param>
    public void P_CoolOn(string _itemSmallType)
    {
        for (int i = 0; i < coolItemTypes.Count; i++)
        {
            //타 스크립트에 사용하여 소모 아이템 종류를 string화 하여 가져온 후 다시 enum으로 변형시켜 비교하여 적용
            if(coolItemTypes[i].coolType == (CoolType)Enum.Parse(typeof(CoolType), _itemSmallType))
            {
                coolItemTypes[i].curCoolTime = coolItemTypes[i].setCoolTime; //쿨타임 시간 설정
                coolItemTypes[i].isCool = true; //쿨타임 적용
                return; //중복되는 종류가 없으므로 바로 리턴 시켜서 코드 종료 시키기
            }
        }

        #region switch버전
        //switch (_itemSmallType)
        //{
        //    case "HealPostion": //회복 아이템일 경우
        //        coolItemTypes[0].curCoolTime = coolItemTypes[0].setCoolTime; //쿨타임 시간 설정
        //        coolItemTypes[0].isCool = true; //쿨타임 적용
        //        break;
        //}
        #endregion
    }

    /// <summary>
    /// 특정 아이템 종류의 쿨타임 유무를 제공하여 아이템 사용 제한할 때 사용
    /// </summary>
    /// <param name="_itemSmallType"></param>
    /// <returns></returns>
    public bool P_SearchCoolType(string _itemSmallType)
    {
        for (int i = 0; i < coolItemTypes.Count; i++)
        {
            //타 스크립트에 사용하여 소모 아이템 종류를 string화 하여 가져온 후 다시 enum으로 변형시켜 비교하여 적용
            if (coolItemTypes[i].coolType == (CoolType)Enum.Parse(typeof(CoolType), _itemSmallType))
            {
                return coolItemTypes[i].isCool;
            }
        }

        return false;
    }
}
