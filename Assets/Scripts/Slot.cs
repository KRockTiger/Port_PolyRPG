using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

//04.16) ������ ������ ������ Item.sc => Item.json���� �����Ͽ� ���� ���� ����
public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private DragSlot dragSlot;
    [SerializeField] private InventoryManager inventoryManager;

    public enum ItemType
    {
        Equip, //���
        Used, //�Ҹ�ǰ
        Null, //������ ����
    }

    [SerializeField] private ItemJson itemData; //������ ����
    [SerializeField] private ItemType itemType; //������ ����

    //[SerializeField] private Item item; //���Կ� �� ������
    [SerializeField] int idx; //������ ��ȣ Ȯ��
    [SerializeField] int itemCount; //������ ����
    [SerializeField] private Image itemImage; //������ �̹���
    [SerializeField] private GameObject checkImage; //���� �̹���, ���� �� Ŀ���� ������ ���� ����
    [SerializeField] private GameObject objItemCount; //������ ���� ������Ʈ
    [SerializeField] private TMP_Text textCount;

    [SerializeField] private bool isDragging; //�巡�� ���� ������ ��� true

    private void Start()
    {
        dragSlot = DragSlot.Instance;
    }

    private void Update()
    {
        textCount.text = itemCount.ToString();
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
        if (idx != -1) //���Կ� �������� ���� ���� ���� => ������ ��ȣ�� -1�� ��
        {
            isDragging = true;
            dragSlot.gameObject.SetActive(true); //�巡�� ���� Ȱ��ȭ
            dragSlot.P_SetDragItem(itemData, itemCount, itemImage.sprite); //�巡�� ���Կ� ������ �ֱ�
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
            itemData = dragSlot.P_GetItemData(); //�� ���Կ� �������� �ֱ�
            itemCount = dragSlot.P_GetItemCount();
            itemImage.sprite = dragSlot.P_GetItemSprite();
            dragSlot.P_ReSetDragItem(); //������ �����
            dragSlot.gameObject.SetActive(false); //�巡�� ���� ��Ȱ��ȭ
            objItemCount.SetActive(itemData.nameType == ItemType.Used.ToString());
        }

        else //�������� ���� ���
        {
            itemData = null; //������ �����
            idx = -1;
            itemCount = 0;
            itemImage.sprite = null; //������ �̹��� ����
            itemImage.gameObject.SetActive(false); //�̹��� ������Ʈ ��Ȱ��ȭ
            dragSlot.gameObject.SetActive(false); //�巡�� ���� ��Ȱ��ȭ
            objItemCount.SetActive(false); //������ ���� â ��Ȱ��ȭ
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
        ItemJson tempItemData = itemData; //B ������ ������ ��ȣ�� ������ ������ ����
        int tempItemIdx = idx; //B ������ ������ ��ȣ�� ������ ������ ����
        int tempItemCount = itemCount;
        Sprite tempItemSprite = itemImage.sprite; //B ������ ������ �̹����� ������ ������ ����
        itemData = dragSlot.P_GetItemData(); //B���Կ� �巡���� ������ ��ȣ�� ����
        idx = itemData.idx;
        itemCount = dragSlot.P_GetItemCount();
        itemImage.sprite = dragSlot.P_GetItemSprite(); //������ �̹����� ����
        itemImage.gameObject.SetActive(true); //�̹��� ������Ʈ Ȱ��ȭ
        dragSlot.P_ReSetDragItem(); //�巡�� ���Կ� �ִ� ������ ����
        objItemCount.SetActive(itemData.nameType == ItemType.Used.ToString());

        if (tempItemIdx != -1) //����� ���Կ� �������� ������
        {
            dragSlot.P_SetDragItem(tempItemData, tempItemCount, tempItemSprite); //���Ƿ� ����� �������� �巡�� ���Կ� ����
        }
    }

    /// <summary>
    /// ���Կ� ������ �߰�
    /// 03.26) �ܺο��� �������� ȹ���ϴ� �ڵ带 ����ϱ� ������ public���� ��ü
    /// 04.13) ������ ������ Item.sc => Item.json���� ����
    /// </summary>
    public void P_AddItem(ItemJson _itemData, Sprite _nameSprite, int _count = 1)
    {
        if(inventoryManager == null) inventoryManager = InventoryManager.Instance;

        itemData = _itemData; //������ ������ �ֱ�
        idx = itemData.idx; //������ ��ȣ �ֱ�
        itemImage.sprite = _nameSprite; //������ �̹��� �ֱ�
        itemImage.gameObject.SetActive(true); //������ �̹��� ������Ʈ Ȱ��ȭ

        if (itemData.nameType == ItemType.Used.ToString()) //�������� �Ҹ�ǰ�� ���
        {
            objItemCount.SetActive(true); //������ ���� â Ȱ��ȭ
            itemCount += _count;
            textCount.text = itemCount.ToString();
        }
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

    public ItemJson P_GetItemData()
    {
        return itemData;
    }

    /// <summary>
    /// ������ ���� Ȯ���ϱ�
    /// ���� �Ҹ�ǰ �������� �� ĭ�� max�� ���� ������ �� ĭ�� ���� �����ϰ� �ʰ��Ǹ� ���� ĭ�� ä���
    /// </summary>
    /// <returns></returns>
    public int P_GetItemCount()
    {
        return itemCount;
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
