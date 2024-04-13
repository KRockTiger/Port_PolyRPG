using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//04.16) ������ ������ ������ Item.sc => Item.json���� �����Ͽ� ���� ���� ����
public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private DragSlot dragSlot;

    [SerializeField] private Item item; //���Կ� �� ������
    [SerializeField] int idx; //������ ��ȣ Ȯ��
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
        if (idx != -1) //���Կ� �������� ���� ���� ����
        {
            isDragging = true;
            dragSlot.gameObject.SetActive(true); //�巡�� ���� Ȱ��ȭ
            dragSlot.P_SetDragItem(idx, itemImage.sprite); //�巡�� ���Կ� ������ �ֱ�
            Color setAlpha = new Color(1, 1, 1, 0.5f); //������ ����
            itemImage.color = setAlpha; //�÷� ������
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //�巡�� ���Կ� �������� ������ ���� ����
        if (dragSlot.P_GetItemIdx() != -1)
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
        if (dragSlot.P_GetItemIdx() != -1)
        {
            //item = dragSlot.P_GetItem(); //�� ���Կ� �������� �ֱ�
            idx = dragSlot.P_GetItemIdx(); //�� ���Կ� �������� �ֱ�
            itemImage.sprite = dragSlot.P_GetItemSprite();
            dragSlot.P_ReSetDragItem(); //������ �����
            dragSlot.gameObject.SetActive(false); //�巡�� ���� ��Ȱ��ȭ
        }

        else //�������� ���� ���
        {
            idx = -1; //������ �����
            itemImage.sprite = null; //������ �̹��� ����
            itemImage.gameObject.SetActive(false); //�̹��� ������Ʈ ��Ȱ��ȭ
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
        if (dragSlot.P_GetItemIdx() != -1)
        {
            //������ ��ü
            ChangeItem();
        }
    }

    /// <summary>
    /// �巡�׷� ������ �ڸ� ��ü
    /// A ���� : �巡�� �� ����
    /// B ���� : ����� ����
    /// </summary>
    private void ChangeItem()
    {
        int tempItemIdx = idx; //B ������ ������ ��ȣ�� ������ ������ ����
        Sprite tempItemSprite = itemImage.sprite; //B ������ ������ �̹����� ������ ������ ����
        idx = dragSlot.P_GetItemIdx(); //B���Կ� �巡���� ������ ��ȣ�� ����
        itemImage.sprite = dragSlot.P_GetItemSprite(); //������ �̹����� ����
        itemImage.gameObject.SetActive(true); //�̹��� ������Ʈ Ȱ��ȭ
        dragSlot.P_ReSetDragItem(); //�巡�� ���Կ� �ִ� ������ ����

        if (tempItemIdx != -1) //����� ���Կ� �������� ������
        {
            dragSlot.P_SetDragItem(tempItemIdx, tempItemSprite); //���Ƿ� ����� �������� �巡�� ���Կ� ����
        }
    }

    /// <summary>
    /// ���Կ� ������ �߰�
    /// 03.26) �ܺο��� �������� ȹ���ϴ� �ڵ带 ����ϱ� ������ public���� ��ü
    /// 04.13) ������ ������ Item.sc => Item.json���� ����
    /// </summary>
    public void P_AddItem(int _idx, Sprite _nameSprite)
    {
        idx = _idx;
        itemImage.sprite = _nameSprite;
        itemImage.gameObject.SetActive(true); //������ �̹��� ������Ʈ Ȱ��ȭ
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
    public int P_GetItemIdx()
    {
        return idx;
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
