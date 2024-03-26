using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    private DragSlot dragSlot;

    [SerializeField] private Item item; //���Կ� �� ������
    [SerializeField] private Image itemImage; //������ �̹���
    [SerializeField] private GameObject checkImage; //���� �̹���, ���� �� Ŀ���� ������ ���� ����

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
        dragSlot.P_ReSetDragItem(); //������ �����
        dragSlot.gameObject.SetActive(false); //�巡�� ���� ��Ȱ��ȭ
        Color setAlpha = new Color(1, 1, 1, 1); //�⺻ ���İ� ����
        itemImage.color = setAlpha; //�÷� ������
    }

    /// <summary>
    /// ���� �巡�״� �ڽ��� �巡���� ������Ʈ�� �������� �߻��ϰ�
    /// �µ���� ���콺�� �÷��� �ִ� ������Ʈ�� �������� ���콺 Ŭ���� ������ �߻��Ѵ�.
    /// �� ���� �ٸ� ������Ʈ���� �ڵ尡 ��µȴ�.
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
    /// ���Կ� ������ �߰�
    /// </summary>
    private void GetSlotItem(Item _item)
    {
        item = _item;
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
