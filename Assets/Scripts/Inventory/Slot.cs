using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

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

    [SerializeField] private ItemData itemData; //������ ����
    [SerializeField] private ItemType itemType; //������ ����

    //[SerializeField] private Item item; //���Կ� �� ������
    [SerializeField] int idx; //������ ��ȣ Ȯ��
    [SerializeField] int itemCount; //������ ����
    [SerializeField] float setUseCoolTime; //������ ������ ��� ��Ÿ��
    [SerializeField] float curUseCoolTime; //���� ������ ��� ��Ÿ��
    [SerializeField] private Image itemImage; //������ �̹���
    [SerializeField] private GameObject checkImage; //���� �̹���, ���� �� Ŀ���� ������ ���� ����
    [SerializeField] private GameObject objItemCount; //������ ���� ������Ʈ
    [SerializeField] private TMP_Text textCount;

    [SerializeField] private bool isDragging; //�巡�� ���� ������ ��� true
    [SerializeField] private bool isUsedFull; //�Ҹ�ǰ �������� ������ �ִ��� ��� true
    [SerializeField] private bool isClickSlot; //���� �� �������� Ŭ������ ����
    [SerializeField] private bool isCoolTimeSlot; //���� �� ��Ÿ�� ����

    private int maxItemCount = 9; //���Կ� �� �� �ִ� ������ �ִ� ����
    private WaitForSeconds waitCoolTime = new WaitForSeconds(1f);

    private void Start()
    {
        dragSlot = DragSlot.Instance;
        inventoryManager = InventoryManager.Instance;
    }

    private void Update()
    {
        textCount.text = itemCount.ToString();

        if (itemCount == maxItemCount)
        {
            isUsedFull = true;
        }

        else if (itemCount < maxItemCount)
        {
            isUsedFull = false;
        }

        if (isCoolTimeSlot) //���� ������ ��Ÿ���� �߻��Ѵٸ�
        {
            curUseCoolTime -= Time.unscaledDeltaTime; //��ٿ�
                                              //(05.11 ���� => TimeScale�� 1�� �ƴϸ� -= Time.deltaTime �ڵ尡 ����� �۵� �ȵ�)
                                              //�Դٰ� �κ��丮�� ��Ȱ��ȭ ���¿��� Update�� �۵��� �ȵ�
            if (curUseCoolTime < 0)
            {
                isCoolTimeSlot = false;
            }
            
            //05.11) ���� ��ũ��Ʈ ȥ�ڼ��� TimeScale, ������Ʈ ��Ȱ��ȭ�� ���� �ڵ� ���� ��� �������� ���� ��Ÿ�� ������ ����
            //�׷��ٸ� ��� ������ ������Ʈ�� ���� InventoryManager�� ���� �� ������ ��Ÿ���� �����ؾ���
            //������ ���⼭ ����� ����
            //1. �κ��Ŵ��� ȥ�ڼ� ��� ���� ������ ��Ÿ���� ���� üũ�Ͽ� �����ϴ°�
            //2. GamePause������� TimeScale = 0f�� �� -= timeScale �� �۵����� �ʴ´�. �׷��� ��� ��Ÿ���� �κ� �Ŵ����� �����ϴ°�

            //==> ����� �ǰ� : ������ ȸ���� �κ��丮���� ���� ���� �������� ���� �����ϴ� ������ ���踦 �ϴ� ���� ��õ��
            //===> �ǽð� ���� ������ ���ӿ��� �κ��丮�� ������ ���߰� ȸ���ϴ� ���� ���Ӽ��� ������ �� �� �ִ� ���ɼ��� �����ֽ�
            //     ������ �κ��丮�� ���� �� ��Ÿ���� ���缭 ���� ������ �´� �뷱���� ����� ���� ��õ�Ͻ�
            //���� ������ �ƴ� ������ �������� ��Ÿ���� �������ִ� CoolTimeManager ��ũ��Ʈ�� ���� �����ϴ� ���� ��õ

            //==> �ǵ�� �� �� ���� : �� ������ ������ ����� ������� ������ ���� ��ȹ�Ͽ� ���� ����� ������ ��� ���� ���߾���
            //                      ������ �ϴ� �͵� ������ ���� ����� ���� �뷱���� ��� ������ ��ĥ �� �ִ��� ������ �� �ְ� ���ִ�
            //                      ������ �޾��� �׸��� �κ����� ȸ���ϴ� �ͺ��� ���������� �ǰ��� �ϴ� ���� ���Ӽ��� �� ���� �� ���ٰ�
            //                      ������ ������ ������ �ߺ� �Է� ������ �������� ����� ��Ÿ���� �����ٰ� �ǵ�� �޾� ���߿� ���� �ҵ�
        }
        
        //���콺 ������ ��ư���� ������ ���
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            UseSlot();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //���콺�� ���� ���� �÷��θ� ���� �̹��� Ȱ��ȭ
        checkImage.SetActive(true);
        isClickSlot = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //���콺�� ���� ������ ������ ���� �̹��� ��Ȱ��ȭ
        checkImage.SetActive(false);
        isClickSlot = false;
    }

    /// <summary>
    /// 1)���콺�� ���� ���� �ִ� ���¿��� �κ��丮�� ���� ���콺�� ġ��� ���� �ٽ� �κ��丮�� Ű��
    /// �� ������ Ȱ��ȭ ���·� �����Ǵ� ���װ� ����� ������ �ܺο��� ������ ���� �����.
    /// -�� �ڵ带 ������ ������ ���� InventoryManager ��ũ��Ʈ�� �ѱ��
    /// -GameManager���� for������ ������ üũ �Ͽ� ������ ���� �ϱ�
    /// </summary>
    public void P_SlotOff()
    {
        checkImage.SetActive(false);
        isClickSlot = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //�巡�׸� ������ �� �巡�� ���Կ� ������ ������ �Ѱܾ� ��
        if (idx != 0) //���Կ� �������� ���� ���� ����
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
        if (dragSlot.P_GetItemIdx() != 0)
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
        if (dragSlot.P_GetItemIdx() != 0)
        {
            //item = dragSlot.P_GetItem(); //�� ���Կ� �������� �ֱ�
            idx = dragSlot.P_GetItemIdx();
            itemData = dragSlot.P_GetItemData(); //�� ���Կ� �������� �ֱ�
            itemCount = dragSlot.P_GetItemCount();
            itemImage.sprite = dragSlot.P_GetItemSprite();
            dragSlot.P_ReSetDragItem(); //������ �����
            dragSlot.gameObject.SetActive(false); //�巡�� ���� ��Ȱ��ȭ
            objItemCount.SetActive(itemData.nameType == ItemType.Used.ToString());
            CheckItemType(itemData.nameType); //���� �� ������ Ÿ�� ����
        }

        else //�������� ���� ���
        {
            itemData = null; //������ �����
            idx = 0;
            itemCount = 0;
            itemImage.sprite = null; //������ �̹��� ����
            itemImage.gameObject.SetActive(false); //�̹��� ������Ʈ ��Ȱ��ȭ
            dragSlot.gameObject.SetActive(false); //�巡�� ���� ��Ȱ��ȭ
            objItemCount.SetActive(false); //������ ���� â ��Ȱ��ȭ
            itemType = ItemType.Null; //�������� �������� �����Ƿ� Null�� ����
        }
    }

    /// <summary>
    /// �� ��ũ��Ʈ�� ���� ������Ʈ ���� ���콺�� �� ��
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrop(PointerEventData eventData)
    {
        //���� �巡�� ������ �������� ������ �������̸鼭 �� ������ �������� ������ ����� ���
        if (dragSlot.P_GetIsQuickItem() && itemType == (ItemType)Enum.Parse(typeof(ItemType), "Equip"))
        {
            return;
        }

        ItemData dragItemData = dragSlot.P_GetItemData();

        //�巡�� ���Կ� �������� ������ ���
        if (dragSlot.P_GetItemIdx() != 0)
        {
            //������ ��ü
            //ChangeItem(); //�ܼ��� �̰͸� ���� ���װ� ����

            if (dragItemData.nameType != "Used") //�巡�� ���� �������� �Ҹ�ǰ�� �ƴ� ���
            {
                //������ ��ü
                ChangeItem();
            }

            //�巡�� ���� �������� �Ҹ�ǰ�̸鼭 �������� �ƴ� ���
            else if (dragItemData.nameType == "Used" && !dragSlot.P_GetIsQuickItem())
            {
                if (dragItemData.idx == idx) //�巡�� ������ �����۰� ����� ������ �������� ������ ���
                {
                    //�������� �ִ� ������ �Ҹ�ǰ �������� ��ģ��.
                    SumItem();
                }

                else if (dragItemData.idx != idx) //������ �������� �ƴ� ���
                {
                    ChangeItem();
                }
            }

            //�巡�� ���� �������� �������� �Ҹ�ǰ�� ���
            else if (dragItemData.nameType == "Used" && dragSlot.P_GetIsQuickItem())
            {
                //�׳� ������ ��ü
                ChangeItem();
            }
        }
    }

    /// <summary>
    /// �� ���Կ� �ִ� �Ҹ�ǰ�� ������ ������ �ִ�ġ�� �ƴ� ������ ������ ���� �� ���� �����ۿ� �ִ� ���Կ� �巡���ϸ�
    /// ����� ���Կ� �������� �������� ���� ���ϰ� ���� �������� ������ ��� �巡�׸� ������ ���Կ� ����� �Ѵ�.
    /// OnDrop�Լ� �Ʒ��� ����ϹǷ� ���� ����
    /// </summary>
    private void SumItem()
    {
        //�� �Լ��� ������ �������� �ջ��Ѵٴ� ���ǾƷ��� ����ϹǷ� ������ ���Ѵ�.
        int dragItemCount = dragSlot.P_GetItemCount();

        if (dragItemCount == maxItemCount) //�̹� �巡�� ������ ������ ������ �ִ�ġ�� ���
        {
            ChangeItem();
        }

        else //�ִ�ġ�� �ƴ� ���
        {
            int sumCount = dragItemCount + itemCount;

            //���ļ� �ִ�ġ ������ ���
            //����� ���Կ� �������� �ű�� �巡���� ������ �������� ����
            if (sumCount <= maxItemCount && isDragging == false)
            {
                itemCount = sumCount; //��� ���Կ� �ջ�
                dragSlot.P_ReSetDragItem(); //�巡�� ������ �������� ����
            }

            //���ļ� �ִ�ġ �ʰ��� ���
            //����� ������ ������ ������ �ִ�ġ�� ���� ���� ������ �巡�� ������ ������ ������ ����
            else if (sumCount > maxItemCount)
            {
                if (!isUsedFull) //����� ������ ������ ������ �ִ밡 �ƴѰ��
                {
                    itemCount = maxItemCount; //������ ������ �ִ�ġ�� ����
                    dragSlot.P_SetItemCount(sumCount - maxItemCount); //���� ������ �巡�� ���Կ� ����
                }

                else //����� ������ ������ ������ �ִ��� ���
                {
                    ChangeItem();
                }
            }
        }
    }

    /// <summary>
    /// �巡�׷� ������ �ڸ� ��ü
    /// A ���� : �巡�� �� ����
    /// B ���� : ����� ����
    /// </summary>
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
        objItemCount.SetActive(itemData.nameType == ItemType.Used.ToString());
        textCount.text = itemCount.ToString();
        CheckItemType(itemData.nameType);

        if (tempItemIdx != 0) //����� ���Կ� �������� ������
        {
            dragSlot.P_SetDragItem(tempItemData, tempItemCount, tempItemSprite); //���Ƿ� ����� �������� �巡�� ���Կ� ����
        }
    }

    /// <summary>
    /// ���� �� �������� ����ϴ� �Լ�
    /// </summary>
    private void UseSlot()
    {
        if (!isClickSlot || isCoolTimeSlot) //���� ��� ������ ���� Ȥ�� ������ �l�����̸� ����
        {
            return;
        }

        switch (itemType) //������ Ÿ�Կ� ���� ����
        {
            case ItemType.Equip:
                //Ŭ���� ��� ���� �� ����
                int dempIdx = inventoryManager.P_EquipChange(idx); //�� ���� ������ ��ȣ�� �κ��� �ѱ�� ���� �������� ��ȣ�� �����´�
                itemData = inventoryManager.GetItemJson(dempIdx); //������ ��ȣ�� ���� Json�� ��ϵ� �������� �Է�
                idx = itemData.idx;
                itemImage.sprite = inventoryManager.GetSprite(dempIdx);
                break;

            case ItemType.Used:
                //�Ҹ�ǰ �������� 1�� �Ҹ��Ͽ� ���
                itemCount -= 1;
                if (itemCount == 0) //������ ������ 0�� �Ǹ� �� �������� ����� => ������ ������ ����
                {
                    itemData = null; //������ �����
                    idx = 0;
                    itemCount = 0;
                    itemImage.sprite = null; //������ �̹��� ����
                    itemImage.gameObject.SetActive(false); //�̹��� ������Ʈ ��Ȱ��ȭ
                    dragSlot.gameObject.SetActive(false); //�巡�� ���� ��Ȱ��ȭ
                    objItemCount.SetActive(false); //������ ���� â ��Ȱ��ȭ
                    itemType = ItemType.Null; //�������� �������� �����Ƿ� Null�� ����
                }
                curUseCoolTime = setUseCoolTime; //��Ÿ�� ����
                isCoolTimeSlot = true;
                break;

            case ItemType.Null:
                Debug.Log("���� �� �������� �������� �ʽ��ϴ�.");
                break;
        }   
    }

    /// <summary>
    /// ���Կ� ������ �߰�
    /// 03.26) �ܺο��� �������� ȹ���ϴ� �ڵ带 ����ϱ� ������ public���� ��ü
    /// 04.13) ������ ������ Item.sc => Item.json���� ����
    /// </summary>
    public void P_AddItem(ItemData _itemData, Sprite _nameSprite)
    {
        if(inventoryManager == null) inventoryManager = InventoryManager.Instance;

        itemData = _itemData; //������ ������ �ֱ�
        idx = itemData.idx; //������ ��ȣ �ֱ�
        itemImage.sprite = _nameSprite; //������ �̹��� �ֱ�
        itemImage.gameObject.SetActive(true); //������ �̹��� ������Ʈ Ȱ��ȭ

        if (itemData.nameType == ItemType.Used.ToString()) //�������� �Ҹ�ǰ�� ���
        {
            objItemCount.SetActive(true); //������ ���� â Ȱ��ȭ
            textCount.text = itemCount.ToString(); //������ ������ string���� ����
        }

        CheckItemType(itemData.nameType);
    }

    /// <summary>
    /// json�� ������ Ÿ���� �ҷ��ͼ� ���� �� ������ ������ ����
    /// </summary>
    /// <param name="_typeName"></param>
    private void CheckItemType(string _typeName)
    {
        switch (_typeName)
        {
            case "Equip":
                itemType = ItemType.Equip;
                break;

            case "Used":
                itemType = ItemType.Used;
                break;
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

    public ItemData P_GetItemData()
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
    /// �Ҹ�ǰ �������� ������ �ø��� �Լ�
    /// </summary>
    /// <param name="_count"></param>
    public void P_SetItemCount(int _count)
    {
        itemCount += _count;
        if (itemCount >= maxItemCount) //������ ���� ���ڰ� �ִ� �̻��� ���
        {
            isUsedFull = true;
        }
    }

    /// <summary>
    /// �巡�� ���� ��������
    /// </summary>
    /// <returns></returns>
    public bool P_GetIsDragging()
    {
        return isDragging;
    }

    public bool P_GetIsUsedFull()
    {
        return isUsedFull;
    }

    public GameObject P_GetActiveSlot()
    {
        return checkImage;
    }

    public bool P_GetIsClickSlot()
    {
        return isClickSlot;
    }

    public bool P_GetIsCoolTimeSlot()
    {
        return isCoolTimeSlot;
    }

    public void P_CoolDown()
    {
        
    }

    public void P_UnIsClickSlot()
    {
        isClickSlot = false;
    }
}
