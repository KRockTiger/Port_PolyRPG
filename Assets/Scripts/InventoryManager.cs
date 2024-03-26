using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private DragSlot dragSlot;

    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject itemSlots;
    [SerializeField] private Slot[] slots;

    [SerializeField] private Item addItem; //치트로 추가할 임의의 아이템으로 추후 삭제 예정

    private void Start()
    {
        slots = itemSlots.GetComponentsInChildren<Slot>();
        dragSlot = DragSlot.Instance;
    }

    private void Update()
    {
        CheckSlots();

        InputGetItem(); //치트용 함수로 추후 삭제
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
    private void InputGetItem()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            int count = slots.Length;
            for (int iNum = 0; iNum < count; iNum++)
            {
                if (slots[iNum].P_GetItem() == null)
                {
                    slots[iNum].P_AddItem(addItem);
                }
            }
        }
    }
}
