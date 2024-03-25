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
        if (Input.GetKeyDown(KeyCode.B) && inventory.activeSelf == true) //�κ��丮 ������Ʈ�� ��Ȱ��ȭ ������ ��
        {
            int count = slots.Length; //���� ���� Ȯ��

            for (int iNum = 0; iNum < count; iNum++)
            {
                //��� ������ üũ �̹��� ������Ʈ�� Ȱ��ȭ ���θ� Ȯ��
                if (slots[iNum].P_GetActiveSlot().activeSelf == true)
                {
                    //���� ���������� false�� ��Ȱ��ȭ
                    slots[iNum].P_GetActiveSlot().SetActive(false);
                }
            }
        }
    }
}
