using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    [Header("타 오브젝트의 스크립트를 저장할 변수")]
    [SerializeField] private DragSlot dragSlot;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private InventoryManager inventoryManager;

    private ItemData itemData;
    private GameObject curWeapon;
    private PlayerAttack playerAttack;
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
        //드래그 중인 아이템이 장비 아이템으로 존재할 경우
        if (dragSlot.P_GetItemIdx() != 0 && dragSlot.P_GetItemType().ToString() == "Equip")
        {
            ItemData tempData = itemData; //장착된 아이템 데이터를 임의의 변수에 저장
            Sprite tempImage = itemImage.sprite; //이미지도 똑같이 저장
            P_SetItemData(dragSlot.P_GetItemIdx());
            dragSlot.P_SetDragItem(tempData, 0, tempImage); //임의의 변수에 저장한 데이터를 드래그 슬롯에 저장

            //itemData = dragSlot.P_GetItemData(); //드래그 슬롯에 저장된 아이템 데이터를 장착
            //idx = itemData.idx; //번호 등록
            //itemImage.sprite = dragSlot.P_GetItemSprite(); //이미지 등록
        }
    }

    /// <summary>
    /// 게임 시작할 때 캐릭터가 무기를 들고 있지 않으면 기본 무기 장착
    /// InventoryManager에서 사용
    /// </summary>
    public void P_StartSetBasicItem()
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
        
        //SetWeaponCollider(05.11 추가)
        //현재 프로젝트에 저장된 무기 오브젝트(이름이 "Equip_"으로 시작하는 오브젝트)에는 BoxCollider가 만들어져 있으므로
        //위 코드에 생성된 무기 오브젝트의 콜리터 컴포넌트를 가져온다
        BoxCollider boxCollider = curWeapon.GetComponent<BoxCollider>();

        if(playerAttack == null) playerAttack = GameManager.Instance.PlayerAttack;
        //playerAttack.P_SetWeaponCollider(boxCollider); //추후 조정

        boxCollider.enabled = false; //만약 박스 콜리더가 켜져있으면 바로 끌 수 있게 만드는 예외처리 코드

        //현 무기가 가진 데이터 값을 PlayerStats 스크립트에 넘기며 플레이어의 공격력을 조정
        //현재 플레이어 공격력 = 기존 플레이어 공격력 + 무기에 저장된 공격력
        playerStats.P_SetWeaponAttackPoint(itemData.floatValue);
    }
}
