using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private DragSlot dragSlot;

    [SerializeField] private Item item; //���Կ� �� ������
    [SerializeField] private Image itemImage; //������ �̹���
    [SerializeField] private GameObject checkImage; //���� �̹���, ���� �� Ŀ���� ������ ���� ����

    [SerializeField] private bool isDragging; //�巡�� ���� ������ ��� true

    private void Start()
    {
        dragSlot = DragSlot.Instance;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //���콺�� ���� ���� �÷��θ� ���� �̹��� Ȱ��ȭ
        checkImage.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //���콺�� ���� ������ ������ ���� �̹��� ��Ȱ��ȭ
        checkImage.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //�巡�׸� ������ �� �巡�� ���Կ� ������ ������ �Ѱܾ� ��
        if (item != null) //���Կ� �������� ���� ���� ����
        {
            isDragging = true;
            dragSlot.gameObject.SetActive(true); //�巡�� ���� Ȱ��ȭ
            dragSlot.P_SetDragItem(item); //�巡�� ���Կ� ������ �ֱ�
            Color setAlpha = new Color(1, 1, 1, 0.5f); //������ ����
            itemImage.color = setAlpha; //�÷� ������
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //�巡�� ���Կ� �������� ������ ���� ����
        if (dragSlot.P_GetItem() != null)
        {
            dragSlot.transform.position = eventData.position; //�巡�� �ϴ� ���� ���콺�� ���� ������
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging) { return; }
        //dragSlot.P_ReSetDragItem(); //������ �����
        //dragSlot.gameObject.SetActive(false); //�巡�� ���� ��Ȱ��ȭ
        Color setAlpha = new Color(1, 1, 1, 1); //�⺻ ���İ� ����
        itemImage.color = setAlpha; //�÷� ������
        isDragging = false;

        ///����׷� �ڵ� ������ Ȯ���غ� ���
        ///OnDrop -> OnEndDrag ������ �ڵ尡 ����Ǵ� ���� Ȯ���� �� �ִ�.
        ///���� OnDrop���� �巡�� ���԰� ����� ������ �������� ��ü ��
        ///�巡�� ������ ������ ������ ����� ���Կ� �������� �־����� Ȯ���� �����߱� ������
        ///�������� �ű�ų� ��ȯ�� ������ �� �־���.
        
        //���� �巡�� ���Կ� �������� �ִ� ���
        //=> ����� ���Կ� �������� ������ ���
        if (dragSlot.P_GetItem() != null)
        {
            item = dragSlot.P_GetItem(); //�� ���Կ� �������� �ֱ�
            dragSlot.P_ReSetDragItem(); //������ �����
            dragSlot.gameObject.SetActive(false); //�巡�� ���� ��Ȱ��ȭ
        }

        else //�������� ���� ���
        {
            item = null; //������ �����
            dragSlot.gameObject.SetActive(false); //�巡�� ���� ��Ȱ��ȭ
        }
    }

    /// <summary>
    /// �� ��ũ��Ʈ�� ���� ������Ʈ ���� ���콺�� �� ��
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrop(PointerEventData eventData)
    {
        //�巡�� ���Կ� �������� ������ ���
        if (dragSlot.P_GetItem() != null)
        {
            //������ ��ü
            ChangeItem();
        }
    }

    private void Update()
    {
        CheckItem();
    }

    /// <summary>
    /// ���� �� ������ Ȯ��
    /// -���� Update�� ��� Ȯ���� �ƴ� �������� �ְų� ���� �������� ����� �� �ֵ��� ���� �ʿ�
    /// </summary>
    private void CheckItem()
    {
        if (item != null) //���Կ� �������� ������ ���
        {
            itemImage.gameObject.SetActive(true); //������ �̹��� ������Ʈ Ȱ��ȭ
            itemImage.sprite = item.P_GetItemSprite(); //����� �������� ��������Ʈ�� ��������
        }

        else
        {
            itemImage.gameObject.SetActive(false); //������ �̹��� ������Ʈ ��Ȱ��ȭ
            itemImage.sprite = null; //������ �̹��� ����
        }
    }

    /// <summary>
    /// �巡�׷� ������ �ڸ� ��ü
    /// A ���� : �巡�� �� ����
    /// B ���� : ����� ����
    /// </summary>
    private void ChangeItem()
    {
        Item tempItem = item; //B ������ �������� ������ ������ ����
        item = dragSlot.P_GetItem(); //B���Կ� �巡���� �������� ����
        dragSlot.P_ReSetDragItem(); //�巡�� ���Կ� �ִ� ������ ����

        if (tempItem != null) //����� ���Կ� �������� ������
        {
            dragSlot.P_SetDragItem(tempItem); //���Ƿ� ����� �������� �巡�� ���Կ� ����
        }
    }

    /// <summary>
    /// ���Կ� ������ �߰�
    /// 03.26) �ܺο��� �������� ȹ���ϴ� �ڵ带 ����ϱ� ������ public���� ��ü
    /// </summary>
    public void P_AddItem(Item _item)
    {
        item = _item;
    }

    /// <summary>
    /// InventoryManager ��ũ��Ʈ ���� �ڵ�
    /// ���� �巡�� �� �κ��丮�� ������ �Ǹ� �巡�� ���Կ� �������� ������ ä�� ����������
    /// ���� ���� ������ �� ��� �巡�� �� ���� ���·� �ǵ����� ���� ���
    /// </summary>
    public void P_ReSetSlotItem()
    {
        if (isDragging)
        {
            Color setAlpha = new Color(1, 1, 1, 1); //�⺻ ���İ� ����
            itemImage.color = setAlpha; //�÷� ������
            isDragging = false;
        }
    }

    /// <summary>
    /// ������ ���� ��������
    /// </summary>
    /// <returns></returns>
    public Item P_GetItem()
    {
        return item;
    }

    /// <summary>
    /// �巡�� ���� ��������
    /// </summary>
    /// <returns></returns>
    public bool P_GetIsDragging()
    {
        return isDragging;
    }

    /// <summary>
    /// ���Կ� ���콺�� �ø� ä�� �κ��丮�� ���� ���콺�� ġ�� ���� �ٽ� Ű��
    /// ���콺�� ���� ��� üũ ������Ʈ�� �����ִ� ������ �����Ƿ� �����ϱ� ���� ���
    /// </summary>
    public GameObject P_GetActiveSlot()
    {
        return checkImage;
    }
}
