using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class QuickSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public static QuickSlot Instance;

    public enum ItemType
    {
        Equip, //장비
        Used, //소모품
    }

    [Header("슬롯 내 오브젝트")]
    [SerializeField] private GameObject objCheckImage;
    [SerializeField] private TMP_Text objItemCount;

    [Header("슬롯 간의 연결을 위한 클래스 변수")]
    [SerializeField] private DragSlot dragSlot; //드래그 슬롯 연결

    [Header("아이템 정보")]
    [SerializeField] private int idx; //아이템 번호
    [SerializeField] private ItemData itemData; //아이템 정보
    [SerializeField] private Image itemImage; //아이템 이미지
    [SerializeField] private int itemCount; //아이템 갯수
    [SerializeField] private bool isDragging; //드래그 유무

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

        objCheckImage.SetActive(false); //만약 켜져 있을 경우 바로 끄기
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        objCheckImage.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        objCheckImage.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //드래그를 시작할 때 드래그 슬롯에 아이템 정보를 넘겨야 함
        if (idx != 0) //슬롯에 아이템이 있을 때만 실행
        {
            isDragging = true;
            dragSlot.gameObject.SetActive(true); //드래그 슬롯 활성화
            dragSlot.P_SetDragItem(itemData, itemCount, itemImage.sprite); //드래그 슬롯에 아이템 넣기
            dragSlot.P_SetIsQuickItem(true); //드래그 아이템이 퀵슬롯 아이템일 경우
            Color setAlpha = new Color(1, 1, 1, 0.5f); //반투명 설정
            itemImage.color = setAlpha; //컬러 입히기
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //드래그 슬롯에 아이템이 존재할 때만 적용
        if (dragSlot.P_GetItemIdx() != 0)
        {
            dragSlot.transform.position = eventData.position; //드래그 하는 동안 마우스에 따라 움직임
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //아이템이 드래그 중이 아닌 상태에는 막기
        if (!isDragging) { return; }
        Color setAlpha = new Color(1, 1, 1, 1); //기본 알파값 설정
        itemImage.color = setAlpha; //컬러 입히기
        isDragging = false;
        dragSlot.P_SetIsQuickItem(false); //드래그가 끝나면 초기화

        //만약 드래그 슬롯에 아이템이 있는 경우
        //=> 드롭한 슬롯에 아이템이 존재한 경우
        if (dragSlot.P_GetItemIdx() != 0)
        {
            idx = dragSlot.P_GetItemIdx();
            itemData = dragSlot.P_GetItemData(); //현 슬롯에 아이템을 넣기
            itemCount = dragSlot.P_GetItemCount();
            itemImage.sprite = dragSlot.P_GetItemSprite();
            dragSlot.P_ReSetDragItem(); //아이템 지우기
            dragSlot.gameObject.SetActive(false); //드래그 슬롯 비활성화
            objItemCount.gameObject.SetActive(true); //아이템 갯수 활성화 ==> 소모품만 등록할 예정이므로 같이 켜줌
            objItemCount.text = itemCount.ToString();
            //CheckItemType(itemData.nameType); //슬롯 내 아이템 타입 설정
        }

        else //아이템이 없는 경우
        {
            itemData = null; //아이템 지우기
            idx = 0;
            itemCount = 0;
            itemImage.sprite = null; //아이템 이미지 삭제
            itemImage.gameObject.SetActive(false); //이미지 오브젝트 비활성화
            dragSlot.gameObject.SetActive(false); //드래그 슬롯 비활성화
            objItemCount.gameObject.SetActive(false); //아이템 갯수 창 비활성화
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        //드래그 슬롯 아이템
        ItemData dragItemData = dragSlot.P_GetItemData();

        if (dragItemData.nameType == "Used") //드래그 슬롯 아이템이 소모품일 경우
        {
            ChangeItem();
        }
    }

    private void ChangeItem()
    {
        ItemData tempItemData = itemData; //B 슬롯의 아이템 번호를 임의의 변수에 저장
        int tempItemIdx = idx; //B 슬롯의 아이템 번호를 임의의 변수에 저장
        int tempItemCount = itemCount;
        Sprite tempItemSprite = itemImage.sprite; //B 슬롯의 아이템 이미지를 임의의 변수에 저장
        itemData = dragSlot.P_GetItemData(); //B슬롯에 드래그한 아이템 번호를 저장
        idx = itemData.idx;
        itemCount = dragSlot.P_GetItemCount();
        itemImage.sprite = dragSlot.P_GetItemSprite(); //아이템 이미지를 저장
        itemImage.gameObject.SetActive(true); //이미지 오브젝트 활성화
        dragSlot.P_ReSetDragItem(); //드래그 슬롯에 있는 아이템 삭제
        objItemCount.gameObject.SetActive(true);
        objItemCount.text = itemCount.ToString();

        if (tempItemIdx != 0) //드롭한 슬롯에 아이템이 있을 경우
        {
            dragSlot.P_SetDragItem(tempItemData, tempItemCount, tempItemSprite); //임의로 저장된 아이템을 드래그 슬롯에 저장
        }
    }

    /// <summary>
    /// 아이템 사용할 때 카운트 줄이기
    /// </summary>
    /// <param name="_itemCount"></param>
    public void P_SetItemCount(int _itemCount)
    {
        itemCount -= _itemCount;
    }

    public int P_GetItemCount()
    {
        return itemCount;
    }

    public int P_GetItemIdx()
    {
        return idx;
    }

    public ItemData P_GetItemData()
    {
        return itemData;
    }

    public Sprite P_GetItemSprite()
    {
        return itemImage.sprite;
    }

    public void P_ReSetItemData()
    {
        itemData = null; //아이템 지우기
        idx = 0;
        itemCount = 0;
        itemImage.sprite = null; //아이템 이미지 삭제
        itemImage.gameObject.SetActive(false); //이미지 오브젝트 비활성화
        dragSlot.gameObject.SetActive(false); //드래그 슬롯 비활성화
        objItemCount.gameObject.SetActive(false); //아이템 갯수 창 비활성화
    }
}
