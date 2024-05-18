using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class QuickSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public static QuickSlot Instance;

    public enum ItemType
    {
        Equip, //���
        Used, //�Ҹ�ǰ
    }

    [Header("���� �� ������Ʈ")]
    [SerializeField] private GameObject objCheckImage;
    [SerializeField] private TMP_Text objItemCount;

    [Header("���� ���� ������ ���� Ŭ���� ����")]
    [SerializeField] private DragSlot dragSlot; //�巡�� ���� ����

    [Header("������ ����")]
    [SerializeField] private int idx; //������ ��ȣ
    [SerializeField] private ItemData itemData; //������ ����
    [SerializeField] private Image itemImage; //������ �̹���
    [SerializeField] private int itemCount; //������ ����
    [SerializeField] private bool isDragging; //�巡�� ����

    private void Awake()
    {
        #region �̱���
        if (Instance == null)
        {
            Instance = this;
        }

        else
        {
            Destroy(gameObject);
        }
        #endregion

        objCheckImage.SetActive(false); //���� ���� ���� ��� �ٷ� ����
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        objCheckImage.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        objCheckImage.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //�巡�׸� ������ �� �巡�� ���Կ� ������ ������ �Ѱܾ� ��
        if (idx != 0) //���Կ� �������� ���� ���� ����
        {
            isDragging = true;
            dragSlot.gameObject.SetActive(true); //�巡�� ���� Ȱ��ȭ
            dragSlot.P_SetDragItem(itemData, itemCount, itemImage.sprite); //�巡�� ���Կ� ������ �ֱ�
            dragSlot.P_SetIsQuickItem(true); //�巡�� �������� ������ �������� ���
            Color setAlpha = new Color(1, 1, 1, 0.5f); //������ ����
            itemImage.color = setAlpha; //�÷� ������
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //�巡�� ���Կ� �������� ������ ���� ����
        if (dragSlot.P_GetItemIdx() != 0)
        {
            dragSlot.transform.position = eventData.position; //�巡�� �ϴ� ���� ���콺�� ���� ������
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //�������� �巡�� ���� �ƴ� ���¿��� ����
        if (!isDragging) { return; }
        Color setAlpha = new Color(1, 1, 1, 1); //�⺻ ���İ� ����
        itemImage.color = setAlpha; //�÷� ������
        isDragging = false;
        dragSlot.P_SetIsQuickItem(false); //�巡�װ� ������ �ʱ�ȭ

        //���� �巡�� ���Կ� �������� �ִ� ���
        //=> ����� ���Կ� �������� ������ ���
        if (dragSlot.P_GetItemIdx() != 0)
        {
            idx = dragSlot.P_GetItemIdx();
            itemData = dragSlot.P_GetItemData(); //�� ���Կ� �������� �ֱ�
            itemCount = dragSlot.P_GetItemCount();
            itemImage.sprite = dragSlot.P_GetItemSprite();
            dragSlot.P_ReSetDragItem(); //������ �����
            dragSlot.gameObject.SetActive(false); //�巡�� ���� ��Ȱ��ȭ
            objItemCount.gameObject.SetActive(true); //������ ���� Ȱ��ȭ ==> �Ҹ�ǰ�� ����� �����̹Ƿ� ���� ����
            objItemCount.text = itemCount.ToString();
            //CheckItemType(itemData.nameType); //���� �� ������ Ÿ�� ����
        }

        else //�������� ���� ���
        {
            itemData = null; //������ �����
            idx = 0;
            itemCount = 0;
            itemImage.sprite = null; //������ �̹��� ����
            itemImage.gameObject.SetActive(false); //�̹��� ������Ʈ ��Ȱ��ȭ
            dragSlot.gameObject.SetActive(false); //�巡�� ���� ��Ȱ��ȭ
            objItemCount.gameObject.SetActive(false); //������ ���� â ��Ȱ��ȭ
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        //�巡�� ���� ������
        ItemData dragItemData = dragSlot.P_GetItemData();

        if (dragItemData.nameType == "Used") //�巡�� ���� �������� �Ҹ�ǰ�� ���
        {
            ChangeItem();
        }
    }

    private void ChangeItem()
    {
        ItemData tempItemData = itemData; //B ������ ������ ��ȣ�� ������ ������ ����
        int tempItemIdx = idx; //B ������ ������ ��ȣ�� ������ ������ ����
        int tempItemCount = itemCount;
        Sprite tempItemSprite = itemImage.sprite; //B ������ ������ �̹����� ������ ������ ����
        itemData = dragSlot.P_GetItemData(); //B���Կ� �巡���� ������ ��ȣ�� ����
        idx = itemData.idx;
        itemCount = dragSlot.P_GetItemCount();
        itemImage.sprite = dragSlot.P_GetItemSprite(); //������ �̹����� ����
        itemImage.gameObject.SetActive(true); //�̹��� ������Ʈ Ȱ��ȭ
        dragSlot.P_ReSetDragItem(); //�巡�� ���Կ� �ִ� ������ ����
        objItemCount.gameObject.SetActive(true);
        objItemCount.text = itemCount.ToString();

        if (tempItemIdx != 0) //����� ���Կ� �������� ���� ���
        {
            dragSlot.P_SetDragItem(tempItemData, tempItemCount, tempItemSprite); //���Ƿ� ����� �������� �巡�� ���Կ� ����
        }
    }

    /// <summary>
    /// ������ ����� �� ī��Ʈ ���̱�
    /// </summary>
    /// <param name="_itemCount"></param>
    public void P_SetItemCount(int _itemCount)
    {
        itemCount -= _itemCount;
    }

    public int P_GetItemCount()
    {
        return itemCount;
    }

    public int P_GetItemIdx()
    {
        return idx;
    }

    public ItemData P_GetItemData()
    {
        return itemData;
    }

    public Sprite P_GetItemSprite()
    {
        return itemImage.sprite;
    }

    public void P_ReSetItemData()
    {
        itemData = null; //������ �����
        idx = 0;
        itemCount = 0;
        itemImage.sprite = null; //������ �̹��� ����
        itemImage.gameObject.SetActive(false); //�̹��� ������Ʈ ��Ȱ��ȭ
        dragSlot.gameObject.SetActive(false); //�巡�� ���� ��Ȱ��ȭ
        objItemCount.gameObject.SetActive(false); //������ ���� â ��Ȱ��ȭ
    }
}
