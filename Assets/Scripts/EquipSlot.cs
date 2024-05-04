using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    [SerializeField] private DragSlot dragSlot;
    [SerializeField] private InventoryManager inventoryManager;

    private ItemData itemData;
    private GameObject curWeapon;
    [SerializeField] private GameObject objItemImage;
    [SerializeField] private GameObject objCheckImage;
    [SerializeField] private Transform trsWeapon;
    [SerializeField] private int idx = 0;
    [SerializeField] private Image itemImage;

    public void OnPointerEnter(PointerEventData eventData)
    {
        objCheckImage.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        objCheckImage.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (dragSlot.P_GetItemIdx() != 0) //드래그 중인 아이템이 존재할 경우
        {
            ItemData tempData = itemData; //장착된 아이템 데이터를 임의의 변수에 저장
            Sprite tempImage = itemImage.sprite; //이미지도 똑같이 저장
            itemData = dragSlot.P_GetItemData(); //드래그 슬롯에 저장된 아이템 데이터를 장착
            idx = itemData.idx; //번호 등록
            itemImage.sprite = dragSlot.P_GetItemSprite(); //이미지 등록

            dragSlot.P_SetDragItem(tempData, 0, tempImage); //임의의 변수에 저장한 데이터를 드래그 슬롯에 저장
        }
    }

    private void Start()
    {
        if (idx == 0) //장착된 아이템이 존재하지 않은 경우
        {
            P_SetItemData(1); //기본 무기 장착
        }
    }

    public ItemData P_GetItemData()
    {
        return itemData;
    }
    
    /// <summary>
    /// 현 장비 창에 아이템 데이터를 불러와서 새로 장착
    /// </summary>
    /// <param name="_idx"></param>
    public void P_SetItemData(int _idx)
    {
        itemData = inventoryManager.GetItemJson(_idx);
        idx = itemData.idx;
        itemImage.sprite = inventoryManager.GetSprite(_idx);
        objItemImage.SetActive(true);
        if (curWeapon != null) //이미 무기가 장착되어 있으면
        {
            Destroy(curWeapon); //현재 무기 오브젝트 파괴
        }

        //무기 생성(Json으로 등록된 무기 리스트를 이용하여 생성)
        curWeapon = Instantiate(inventoryManager.GetEquip(_idx), trsWeapon.position, new Quaternion(0f, 0f, 0f, 0f), trsWeapon);
    }
}
