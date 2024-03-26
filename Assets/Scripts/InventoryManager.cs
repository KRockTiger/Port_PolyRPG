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

    [SerializeField] private Item addItem; //ġƮ�� �߰��� ������ ���������� ���� ���� ����

    private void Start()
    {
        slots = itemSlots.GetComponentsInChildren<Slot>();
        dragSlot = DragSlot.Instance;
    }

    private void Update()
    {
        CheckSlots();

        InputGetItem(); //ġƮ�� �Լ��� ���� ����
    }

    private void CheckSlots()
    {
        //if (Input.GetKeyDown(KeyCode.B) && inventory.activeSelf == true) //�κ��丮 ������Ʈ�� ��Ȱ��ȭ ������ ��
        //{
        //    int count = slots.Length; //���� ���� Ȯ��

        //    for (int iNum = 0; iNum < count; iNum++)
        //    {
        //        //��� ������ üũ �̹��� ������Ʈ�� Ȱ��ȭ ���θ� Ȯ��
        //        if (slots[iNum].P_GetActiveSlot().activeSelf == true)
        //        {
        //            //���� ���������� false�� ��Ȱ��ȭ
        //            slots[iNum].P_GetActiveSlot().SetActive(false);
        //        }
        //    }
        //}

        if (Input.GetKeyDown(KeyCode.B) && inventory.activeSelf == false)
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

                //�巡�� ���� ���� Ȯ��
                if (slots[iNum].P_GetIsDragging())
                {
                    //�巡�� ���̿��� �������� ��� �ǵ�����
                    slots[iNum].P_ReSetSlotItem();
                }
            }

            dragSlot.P_ReSetDragItem();
            dragSlot.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Ű�� �Է��Ͽ� ������ �������� �κ��丮�� ����
    /// -ġƮ�� �ڵ��̹Ƿ� ������ ȹ���� �� �۵��Ǹ� ���� ���� ����
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
