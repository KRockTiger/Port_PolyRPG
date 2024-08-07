using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 아이템을 드래그할 때 사용하기 위한 스크립트
/// </summary>
public class DragSlot : MonoBehaviour
{
    public static DragSlot Instance; //싱글톤을 사용해서 인벤토리 내의 아이템을 넘길 수 있게 해줌
    //private Slot dragSlot; //드래그 슬롯 스크립트에 슬롯 스크립트를 연결해보기 위해 만듦

    public enum ItemType
    {
        Equip,
        Used,
        Null
    }

    //[SerializeField] private Item item;
    [SerializeField] private ItemData itemData; //아이템 정보
    private int itemCount;
    [SerializeField] private ItemType itemType;
    [SerializeField] private int idx = 0; //아이템 번호 => 0은 빈 슬롯을 의미함
    [SerializeField] private Image itemImage;
    [SerializeField] private bool isQuickItem; //퀵슬롯의 아이템을 드래그 중일 경우 true

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

        //itemSprite = GetComponent<Image>();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 드래그를 시작할 때 드래그 슬롯에 아이템 정보를 넘긴다.
    /// </summary>
    public void P_SetDragItem(ItemData _itemJson, int _itemCount, Sprite _itemImage)
    {
        //idx = _idx; //아이템 정보 받기
        itemData = _itemJson;
        idx = itemData.idx;
        itemCount = _itemCount;
        itemImage.sprite = _itemImage; //아이템 이미지를 저장
        CheckItemType(itemData.nameType); //드래그 슬롯의 아이템 타입 확인
    }

    /// <summary>
    /// 드래그 슬롯 안에 있는 아이템 종류에 따라 적용
    /// </summary>
    /// <param name="_itemType"></param>
    private void CheckItemType(string _itemType)
    {
        switch (_itemType)
        {
            case "Equip":
                itemType = ItemType.Equip;
                break;

            case "Used": 
                itemType = ItemType.Used;
                break;
        }
    }

    public void P_ReSetDragItem()
    {
        itemData = null;
        idx = 0;
        itemCount = 0;
        itemImage.sprite = null;
    }

    /// <summary>
    /// 아이템 정보 가져오기
    /// </summary>
    /// <returns></returns>
    public ItemData P_GetItemData()
    {
        return itemData;
    }

    /// <summary>
    /// 아이템 번호 가져오기
    /// </summary>
    /// <returns></returns>
    public int P_GetItemIdx()
    {
        return idx;
    }

    public int P_GetItemCount()
    {
        return itemCount;
    }

    public void P_SetItemCount(int _count)
    {
        itemCount = _count;
    }

    public void P_SetIsQuickItem(bool _isQuickItem)
    {
        isQuickItem = _isQuickItem;
    }

    public Sprite P_GetItemSprite()
    {
        return itemImage.sprite;
    }

    public ItemType P_GetItemType()
    {
        return itemType;
    }

    public bool P_GetIsQuickItem()
    {
        return isQuickItem;
    }
}
