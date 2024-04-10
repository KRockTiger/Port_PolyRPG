using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] private DragSlot dragSlot;

    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject itemSlots;
    [SerializeField] private Slot[] slots;

    [SerializeField] private Item addItem; //치트로 추가할 임의의 아이템으로 추후 삭제 예정

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        slots = itemSlots.GetComponentsInChildren<Slot>();
        dragSlot = DragSlot.Instance;
    }

    private void Update()
    {
        CheckSlots();
    }

    private void CheckSlots()
    {
        //if (Input.GetKeyDown(KeyCode.B) && inventory.activeSelf == true) //인벤토리 오브젝트가 비활성화 상태일 때
        //{
        //    int count = slots.Length; //슬롯 개수 확인

        //    for (int iNum = 0; iNum < count; iNum++)
        //    {
        //        //모든 슬롯의 체크 이미지 오브젝트의 활성화 여부를 확인
        //        if (slots[iNum].P_GetActiveSlot().activeSelf == true)
        //        {
        //            //만약 켜져있으면 false로 비활성화
        //            slots[iNum].P_GetActiveSlot().SetActive(false);
        //        }
        //    }
        //}

        if (dragSlot == null)
        {
            dragSlot = DragSlot.Instance;
        }

        if (Input.GetKeyDown(KeyCode.B) && inventory.activeSelf == false)
        {
            int count = slots.Length; //슬롯 개수 확인

            for (int iNum = 0; iNum < count; iNum++)
            {
                //모든 슬롯의 체크 이미지 오브젝트의 활성화 여부를 확인
                if (slots[iNum].P_GetActiveSlot().activeSelf == true)
                {
                    //만약 켜져있으면 false로 비활성화
                    slots[iNum].P_GetActiveSlot().SetActive(false);
                }

                //드래그 중인 슬롯 확인
                if (slots[iNum].P_GetIsDragging())
                {
                    //드래그 중이였던 아이템일 경우 되돌리기
                    slots[iNum].P_ReSetSlotItem();
                }
            }

            dragSlot.P_ReSetDragItem();
            dragSlot.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 키를 입력하여 임의의 아이템을 인벤토리에 저장
    /// -치트용 코드이므로 아이템 획득이 잘 작동되면 추후 삭제 예정
    /// </summary>
    public void P_InputGetItem(Item _item)
    {
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    int count = slots.Length;
        //    for (int iNum = 0; iNum < count; iNum++)
        //    {
        //        if (slots[iNum].P_GetItem() == null)
        //        {
        //            slots[iNum].P_AddItem(addItem);
        //        }
        //    }
        //}

        int count = slots.Length;

        for (int iNum = 0;iNum < count; iNum++)
        {
            if (slots[iNum].P_GetItem() == null)
            {
                Item scItem = _item;
                slots[iNum].P_AddItem(scItem);
                //Destroy(_item.gameObject);
                return; //아이템이 추가되면 리턴하여 멈추게 하기
            }
        }
    }
}
