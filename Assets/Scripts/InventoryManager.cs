using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject itemSlots;
    [SerializeField] private Slot[] slots;

    private void Start()
    {
        slots = itemSlots.GetComponentsInChildren<Slot>();
    }

    private void Update()
    {
        CheckSlots();
    }

    private void CheckSlots()
    {
        if (Input.GetKeyDown(KeyCode.B) && inventory.activeSelf == true) //인벤토리 오브젝트가 비활성화 상태일 때
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
            }
        }
    }
}
