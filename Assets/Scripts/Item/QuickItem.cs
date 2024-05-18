using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuickItem : MonoBehaviour
{
    [SerializeField] private QuickSlot quickSlot; //퀵슬롯 연결
    [SerializeField] private ItemController itemController; //아이템 컨트롤러 연결

    [SerializeField] private KeyCode useKey; //아이템 사용 코드

    [Header("퀵슬롯 아이템 정보")]
    [SerializeField] private ItemData itemData;
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text objItemCount;
    [SerializeField] private int itemCount;

    private void Update()
    {
        GetQuickSlotData();
        UseItem();
    }

    /// <summary>
    /// 퀵슬롯의 아이템 정보를 실시간으로 가져옴
    /// </summary>
    private void GetQuickSlotData()
    {
        if (quickSlot.P_GetItemIdx() != 0) //만약 퀵슬롯에 아이템이 존재한다면
        {
            itemData = quickSlot.P_GetItemData(); //데이터 가져오기
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = quickSlot.P_GetItemSprite(); //아이템 이미지 가져오기
            itemCount = quickSlot.P_GetItemCount(); //현재 아이템 갯수 가져오기
            objItemCount.gameObject.SetActive(true);
            objItemCount.text = itemCount.ToString();
        }

        else //퀵슬롯에 아이템이 없으면
        {
            itemData = null;
            itemImage.gameObject.SetActive(false);
            itemImage.sprite = null;
            itemCount = 0;
            objItemCount.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 퀵슬롯 아이템 사용
    /// </summary>
    private void UseItem()
    {
        if (Input.GetKeyDown(useKey) && quickSlot.P_GetItemIdx() != 0) //퀵슬롯에 아이템이 존재한 상태에서 키를 누르면
        {
            string itemSmallType = itemData.nameSmallType;

            //만약 쿨타임 중인 아이템 종류일 경우 리턴
            if (itemController.P_SearchCoolType(itemSmallType))
            {
                Debug.Log("현재 쿨타임 중인 아이템입니다.");
                return;
            }

            quickSlot.P_SetItemCount(1); //아이템 1개 소모

            if (quickSlot.P_GetItemCount() <= 0) //퀵슬롯 내 아이템을 다 사용할 경우
            {
                quickSlot.P_ReSetItemData(); //아이템 지우기
            }

            itemController.P_CoolOn(itemSmallType); //특정 세부종류(ex.회복 아이템)의 모든 아이템 쿨타임 적용
        }
    }
}