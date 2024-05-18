using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolTimeManager : MonoBehaviour
{
    public static CoolTimeManager Instance;

    public enum CoolType
    {
        UseCool, //중복 입력 방지
        Postion, //회복 아이템
    }

    [SerializeField] private CoolType coolType;

    [System.Serializable]
    public class Class_CoolTime
    {
        public CoolType coolType; //쿨타임 종류
        public float setCoolTime; //설정할 쿨타임
        public float curCoolTime; //현재 쿨타임
        public bool isCool; //쿨타임 상태 확인
    }

    [SerializeField] private List<Class_CoolTime> coolTimeList;

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
