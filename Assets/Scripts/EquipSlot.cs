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
        if (dragSlot.P_GetItemIdx() != 0) //�巡�� ���� �������� ������ ���
        {
            ItemData tempData = itemData; //������ ������ �����͸� ������ ������ ����
            Sprite tempImage = itemImage.sprite; //�̹����� �Ȱ��� ����
            itemData = dragSlot.P_GetItemData(); //�巡�� ���Կ� ����� ������ �����͸� ����
            idx = itemData.idx; //��ȣ ���
            itemImage.sprite = dragSlot.P_GetItemSprite(); //�̹��� ���

            dragSlot.P_SetDragItem(tempData, 0, tempImage); //������ ������ ������ �����͸� �巡�� ���Կ� ����
        }
    }

    private void Start()
    {
        if (idx == 0) //������ �������� �������� ���� ���
        {
            P_SetItemData(1); //�⺻ ���� ����
        }
    }

    public ItemData P_GetItemData()
    {
        return itemData;
    }
    
    /// <summary>
    /// �� ��� â�� ������ �����͸� �ҷ��ͼ� ���� ����
    /// </summary>
    /// <param name="_idx"></param>
    public void P_SetItemData(int _idx)
    {
        itemData = inventoryManager.GetItemJson(_idx);
        idx = itemData.idx;
        itemImage.sprite = inventoryManager.GetSprite(_idx);
        objItemImage.SetActive(true);
        if (curWeapon != null) //�̹� ���Ⱑ �����Ǿ� ������
        {
            Destroy(curWeapon); //���� ���� ������Ʈ �ı�
        }

        //���� ����(Json���� ��ϵ� ���� ����Ʈ�� �̿��Ͽ� ����)
        curWeapon = Instantiate(inventoryManager.GetEquip(_idx), trsWeapon.position, new Quaternion(0f, 0f, 0f, 0f), trsWeapon);
    }
}
