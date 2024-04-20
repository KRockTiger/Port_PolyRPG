using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] private DragSlot dragSlot;

    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject itemSlots;
    [SerializeField] private Slot[] slots;

    [SerializeField] private Item addItem; //ġƮ�� �߰��� ������ ���������� ���� ���� ����

    public enum jsonType
    {
        Item,
    }
    [SerializeField] List<TextAsset> listJsons; //json���� ���
    [SerializeField] List<ItemJson> listJsonItem; //����� �������� json����(Ȯ�ο�)

    [SerializeField] List<Sprite> listSpr;

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
        listJsonItem = JsonConvert.DeserializeObject<List<ItemJson>>(json);

        //Sprite spr = P_GetSprite(1);
    }

    private void Start()
    {
        slots = itemSlots.GetComponentsInChildren<Slot>();
        dragSlot = DragSlot.Instance;

        //���Ƿ� ������ �ֱ� => json���� �������� �� ���� �� �ִ��� Ȯ�� �� ����
        //P_InputGetItem(0);
        //P_InputGetItem(1);
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
    public void P_InputGetItem(int _idx)
    {
        int count = slots.Length; //���� ���� Ȯ��

        for (int iNum = 0; iNum < count; iNum++)
        {
            //������ ��ȣ�� �̿��Ͽ� �� ���� Ȯ��
            if (slots[iNum].P_GetItemIdx() == -1) //������ ��ȣ�� -1�̸� �� ����
            {
                slots[iNum].P_AddItem(GetItemJson(_idx), GetSprite(_idx));
                return; //�������� �߰��Ǹ� �����Ͽ� ���߰� �ϱ�
            }
        }
    }

    public string P_GetItemType(int _idx)
    {
        return listJsonItem[_idx].nameType;
    }

    private ItemJson GetItemJson(int _idx)
    {
        ItemJson data = listJsonItem.Find(x => x.idx == _idx);
        
        if (data == null)
        {
            Debug.LogError("�������� ���� �������Դϴ�.");
            return null;
        }
        
        return data;
    }

    private Sprite GetSprite(int _idx)
    {
        ItemJson data = listJsonItem.Find(x => x.idx == _idx);
        if (data == null)
        {
            Debug.LogError($"�ùٸ����� ������ �ʾҽ��ϴ�.\n idx = {_idx}");
            return null;
        }

        return listSpr.Find(x => x.name == data.nameSprite);
    }
}
