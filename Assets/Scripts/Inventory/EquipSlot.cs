using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    [Header("Ÿ ������Ʈ�� ��ũ��Ʈ�� ������ ����")]
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
        //�巡�� ���� �������� ��� ���������� ������ ���
        if (dragSlot.P_GetItemIdx() != 0 && dragSlot.P_GetItemType().ToString() == "Equip")
        {
            ItemData tempData = itemData; //������ ������ �����͸� ������ ������ ����
            Sprite tempImage = itemImage.sprite; //�̹����� �Ȱ��� ����
            P_SetItemData(dragSlot.P_GetItemIdx());
            dragSlot.P_SetDragItem(tempData, 0, tempImage); //������ ������ ������ �����͸� �巡�� ���Կ� ����

            //itemData = dragSlot.P_GetItemData(); //�巡�� ���Կ� ����� ������ �����͸� ����
            //idx = itemData.idx; //��ȣ ���
            //itemImage.sprite = dragSlot.P_GetItemSprite(); //�̹��� ���
        }
    }

    /// <summary>
    /// ���� ������ �� ĳ���Ͱ� ���⸦ ��� ���� ������ �⺻ ���� ����
    /// InventoryManager���� ���
    /// </summary>
    public void P_StartSetBasicItem()
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
        
        //SetWeaponCollider(05.11 �߰�)
        //���� ������Ʈ�� ����� ���� ������Ʈ(�̸��� "Equip_"���� �����ϴ� ������Ʈ)���� BoxCollider�� ������� �����Ƿ�
        //�� �ڵ忡 ������ ���� ������Ʈ�� �ݸ��� ������Ʈ�� �����´�
        BoxCollider boxCollider = curWeapon.GetComponent<BoxCollider>();

        if(playerAttack == null) playerAttack = GameManager.Instance.PlayerAttack;
        //playerAttack.P_SetWeaponCollider(boxCollider); //���� ����

        boxCollider.enabled = false; //���� �ڽ� �ݸ����� ���������� �ٷ� �� �� �ְ� ����� ����ó�� �ڵ�

        //�� ���Ⱑ ���� ������ ���� PlayerStats ��ũ��Ʈ�� �ѱ�� �÷��̾��� ���ݷ��� ����
        //���� �÷��̾� ���ݷ� = ���� �÷��̾� ���ݷ� + ���⿡ ����� ���ݷ�
        playerStats.P_SetWeaponAttackPoint(itemData.floatValue);
    }
}
