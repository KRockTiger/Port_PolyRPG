using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] private DragSlot dragSlot;

    [SerializeField] private int maxSlotItemCount; //�ϳ��� ���Կ� �� �� �ִ� �ִ� ������ ����
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject itemSlots;
    [SerializeField] private Slot[] slots;
    [SerializeField] private EquipSlot equipSlot;

    [SerializeField] private Item addItem; //ġƮ�� �߰��� ������ ���������� ���� ���� ����

    public enum jsonType
    {
        Item,
    }
    [SerializeField] List<TextAsset> listJsons; //json���� ���
    [SerializeField] List<ItemData> listJsonItem; //����� �������� json����(Ȯ�ο�)

    [SerializeField] List<Sprite> listSpr;
    [SerializeField] List<GameObject> listObjEquip;

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
        string json = listJsons[(int)jsonType.Item].text;
        listJsonItem = JsonConvert.DeserializeObject<List<ItemData>>(json);

        //Sprite spr = P_GetSprite(1);
    }

    private void Start()
    {
        slots = itemSlots.GetComponentsInChildren<Slot>();
        equipSlot.P_StartSetBasicItem();
        
        //dragSlot = DragSlot.Instance;

        //���Ƿ� ������ �ֱ� => json���� �������� �� ���� �� �ִ��� Ȯ�� �� ����
        //P_InputGetItem(0);
        //P_InputGetItem(1);
    }

    /// <summary>
    /// GameManager���� �κ��丮�� ��Ȱ��ȭ �� �� ���� �����ִ� ������ ������ ������ ���� �ϱ�
    /// </summary>
    public void P_CheckSlots()
    {
        int count = slots.Length; //���� ���� Ȯ��

        for (int iNum = 0; iNum < count; iNum++)
        {
            //��� ������ üũ �̹��� ������Ʈ�� Ȱ��ȭ ���θ� Ȯ��
            if (slots[iNum].P_GetIsClickSlot() == true)
            {
                //���� ���������� ��Ȱ��ȭ
                slots[iNum].P_UnIsClickSlot(); //���� Ŭ�� ��Ȱ��ȭ
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

    /// <summary>
    /// Ű�� �Է��Ͽ� ������ �������� �κ��丮�� ����
    /// -ġƮ�� �ڵ��̹Ƿ� ������ ȹ���� �� �۵��Ǹ� ���� ���� ����
    /// </summary>
    public void P_InputGetItem(int _idx, int _count = 1)
    {
        int count = slots.Length; //���� ���� Ȯ��
        ItemData itemData = GetItemJson(_idx); //������ ���� ��������

        if (itemData.nameType != "Used")
        //ȹ���� �������� �Ҹ�ǰ�� �ƴ� ���
        {
            //������ ��ȣ�� �̿��Ͽ� ������ ������ ���� �� ���� Ȯ���ϱ�
            for (int iNum01 = 0; iNum01 < count; iNum01++)
            {
                if (slots[iNum01].P_GetItemIdx() == 0)
                //������ ��ȣ�� -1�̸� �� ����
                {
                    slots[iNum01].P_AddItem(GetItemJson(_idx), GetSprite(_idx));
                    return; //�������� �߰��Ǹ� �����Ͽ� ���߰� �ϱ�
                }
            }
        }

        else if (itemData.nameType == "Used")
        //���Կ� ���� �������� �Ҹ�ǰ�� ���
        {
            //���� ��ü Ȯ��
            for (int iNum02 = 0; iNum02 < count; iNum02++)
            {
                if (itemData.idx == slots[iNum02].P_GetItemIdx())
                //���Կ� ������ �������� ������ ���
                {
                    if (!slots[iNum02].P_GetIsUsedFull())
                    //������ �������� ���������� ������ ������ �ִ�ġ�� �ƴ� ���
                    {
                        //���Ƿ� ����� ������ ������ ������ ȹ���� ������ ������ ��
                        //�ϳ��� ������ �ִ�� ���� �� �ִ� �Ҹ�ǰ ������ ������ �ִ� �������� ��� ����
                        int sumItemCount = slots[iNum02].P_GetItemCount() + _count;

                        if (sumItemCount <= maxSlotItemCount)
                        //���� ���Կ� �ִ� ������ ������ ȹ���� ������ ������ ���� �ִ� ������ ���
                        {
                            slots[iNum02].P_SetItemCount(_count); //���õ� �������� ����ŭ ������Ű��
                            return;
                        }

                        else if (sumItemCount > maxSlotItemCount) //������ ���� �ִ� �ʰ��� ���                    
                        {
                            int overCount = sumItemCount - maxSlotItemCount;
                            for (int iNum03 = 0; iNum03 < count; iNum03++)
                            {
                                if (slots[iNum03].P_GetItemIdx() == 0)
                                //������ ��ȣ�� 0�̸� �� ����
                                {
                                    slots[iNum03].P_AddItem(GetItemJson(_idx), GetSprite(_idx)); //���Կ� ������ �߰�
                                    slots[iNum03].P_SetItemCount(overCount); //���� ����
                                    return;
                                }
                            }
                        }
                    }
                }
            }

            //������ return�� ���� ������ ���Կ� ������ �������� �������� �����Ƿ� ���� �߰�
            for (int iNum04 = 0; iNum04 < count; iNum04++)
            {
                if (slots[iNum04].P_GetItemIdx() == 0)
                //������ ��ȣ�� -1�̸� �� ����
                {
                    slots[iNum04].P_AddItem(GetItemJson(_idx), GetSprite(_idx)); //���Կ� ������ �߰�
                    slots[iNum04].P_SetItemCount(_count); //���� ����
                    return;
                }
            }
        }
    }

    public string P_GetItemType(int _idx)
    {
        return listJsonItem[_idx].nameType;
    }

    public ItemData GetItemJson(int _idx)
    {
        ItemData data = listJsonItem.Find(x => x.idx == _idx);
        
        if (data == null)
        {
            Debug.LogError("�������� ���� �������Դϴ�.");
            return null;
        }
        
        return data;
    }

    public Sprite GetSprite(int _idx)
    {
        ItemData data = listJsonItem.Find(x => x.idx == _idx);
        if (data == null)
        {
            Debug.LogError($"�ùٸ����� ������ �ʾҽ��ϴ�.\n idx = {_idx}");
            return null;
        }

        return listSpr.Find(x => x.name == data.nameSprite);
    }

    public GameObject GetEquip(int _idx)
    {
        ItemData data = listJsonItem.Find(x => x.idx == _idx);
        
        if (data == null || data.nameType != "Equip")
        {
            Debug.Log("�ùٸ� ������Ʈ�� �ƴմϴ�.");
            return null;
        }

        return listObjEquip.Find(x => x.name == data.nameObject);
    }

    /// <summary>
    /// ��� ��ü �Լ�
    /// </summary>
    public int P_EquipChange(int _idx)
    {
        ItemData dempData = equipSlot.P_GetItemData();
        int dempIdx = dempData.idx;
        equipSlot.P_SetItemData(_idx);
        return dempIdx;
    }
}
