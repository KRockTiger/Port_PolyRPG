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

    [SerializeField] private Item addItem; //ġƮ�� �߰��� ������ ���������� ���� ���� ����

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

        if (dragSlot == null)
        {
            dragSlot = DragSlot.Instance;
        }

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
                return; //�������� �߰��Ǹ� �����Ͽ� ���߰� �ϱ�
            }
        }
    }
}
