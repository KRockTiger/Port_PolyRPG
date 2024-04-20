using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

//04.16) 슬롯의 아이템 정보를 Item.sc => Item.json으로 변경하여 구조 변경 시작
public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private DragSlot dragSlot;
    [SerializeField] private InventoryManager inventoryManager;

    public enum ItemType
    {
        Equip, //장비
        Used, //소모품
        Null, //아이템 없음
    }

    [SerializeField] private ItemJson itemData; //아이템 정보
    [SerializeField] private ItemType itemType; //아이템 종류

    //[SerializeField] private Item item; //슬롯에 들어갈 아이템
    [SerializeField] int idx; //아이템 번호 확인
    [SerializeField] int itemCount; //아이템 갯수
    [SerializeField] private Image itemImage; //아이템 이미지
    [SerializeField] private GameObject checkImage; //슬롯 이미지, 슬롯 위 커서의 유무에 따라 결정
    [SerializeField] private GameObject objItemCount; //아이템 갯수 오브젝트
    [SerializeField] private TMP_Text textCount;

    [SerializeField] private bool isDragging; //드래그 중인 상태일 경우 true

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
        //마우스를 슬롯 위에 올려두면 슬롯 이미지 활성화
        checkImage.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //마우스를 슬롯 밖으로 꺼내면 슬롯 이미지 비활성화
        checkImage.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //드래그를 시작할 때 드래그 슬롯에 아이템 정보를 넘겨야 함
        if (idx != -1) //슬롯에 아이템이 있을 때만 실행 => 아이템 번호가 -1일 때
        {
            isDragging = true;
            dragSlot.gameObject.SetActive(true); //드래그 슬롯 활성화
            dragSlot.P_SetDragItem(itemData, itemCount, itemImage.sprite); //드래그 슬롯에 아이템 넣기
            Color setAlpha = new Color(1, 1, 1, 0.5f); //반투명 설정
            itemImage.color = setAlpha; //컬러 입히기
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //드래그 슬롯에 아이템이 존재할 때만 적용
        if (dragSlot.P_GetItemIdx() != -1)
        {
            dragSlot.transform.position = eventData.position; //드래그 하는 동안 마우스에 따라 움직임
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging) { return; }
        //dragSlot.P_ReSetDragItem(); //아이템 지우기
        //dragSlot.gameObject.SetActive(false); //드래그 슬롯 비활성화
        Color setAlpha = new Color(1, 1, 1, 1); //기본 알파값 설정
        itemImage.color = setAlpha; //컬러 입히기
        isDragging = false;

        ///디버그로 코드 순서를 확인해본 결과
        ///OnDrop -> OnEndDrag 순으로 코드가 진행되는 것을 확인할 수 있다.
        ///따라서 OnDrop에서 드래그 슬롯과 드랍한 슬롯의 아이템을 교체 후
        ///드래그 슬롯의 아이템 유무로 드랍한 슬롯에 아이템이 있었는지 확인이 가능했기 때문에
        ///아이템을 옮기거나 교환이 가능할 수 있었다.
        
        //만약 드래그 슬롯에 아이템이 있는 경우
        //=> 드롭한 슬롯에 아이템이 존재한 경우
        if (dragSlot.P_GetItemIdx() != -1)
        {
            //item = dragSlot.P_GetItem(); //현 슬롯에 아이템을 넣기
            itemData = dragSlot.P_GetItemData(); //현 슬롯에 아이템을 넣기
            itemCount = dragSlot.P_GetItemCount();
            itemImage.sprite = dragSlot.P_GetItemSprite();
            dragSlot.P_ReSetDragItem(); //아이템 지우기
            dragSlot.gameObject.SetActive(false); //드래그 슬롯 비활성화
            objItemCount.SetActive(itemData.nameType == ItemType.Used.ToString());
        }

        else //아이템이 없는 경우
        {
            itemData = null; //아이템 지우기
            idx = -1;
            itemCount = 0;
            itemImage.sprite = null; //아이템 이미지 삭제
            itemImage.gameObject.SetActive(false); //이미지 오브젝트 비활성화
            dragSlot.gameObject.SetActive(false); //드래그 슬롯 비활성화
            objItemCount.SetActive(false); //아이템 갯수 창 비활성화
        }
    }

    /// <summary>
    /// 현 스크립트를 가진 오브젝트 위에 마우스를 땔 때
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrop(PointerEventData eventData)
    {
        //드래그 슬롯에 아이템이 존재할 경우
        if (dragSlot.P_GetItemIdx() != -1)
        {
            //아이템 교체
            ChangeItem();
        }
    }

    /// <summary>
    /// 드래그로 아이템 자리 교체
    /// A 슬롯 : 드래그 한 슬롯
    /// B 슬롯 : 드롭할 슬롯
    /// </summary>
    private void ChangeItem()
    {
        ItemJson tempItemData = itemData; //B 슬롯의 아이템 번호를 임의의 변수에 저장
        int tempItemIdx = idx; //B 슬롯의 아이템 번호를 임의의 변수에 저장
        int tempItemCount = itemCount;
        Sprite tempItemSprite = itemImage.sprite; //B 슬롯의 아이템 이미지를 임의의 변수에 저장
        itemData = dragSlot.P_GetItemData(); //B슬롯에 드래그한 아이템 번호를 저장
        idx = itemData.idx;
        itemCount = dragSlot.P_GetItemCount();
        itemImage.sprite = dragSlot.P_GetItemSprite(); //아이템 이미지를 저장
        itemImage.gameObject.SetActive(true); //이미지 오브젝트 활성화
        dragSlot.P_ReSetDragItem(); //드래그 슬롯에 있는 아이템 삭제
        objItemCount.SetActive(itemData.nameType == ItemType.Used.ToString());

        if (tempItemIdx != -1) //드롭한 슬롯에 아이템이 있으면
        {
            dragSlot.P_SetDragItem(tempItemData, tempItemCount, tempItemSprite); //임의로 저장된 아이템을 드래그 슬롯에 저장
        }
    }

    /// <summary>
    /// 슬롯에 아이템 추가
    /// 03.26) 외부에서 아이템을 획득하는 코드를 사용하기 때문에 public으로 교체
    /// 04.13) 아이템 정보를 Item.sc => Item.json으로 변경
    /// </summary>
    public void P_AddItem(ItemJson _itemData, Sprite _nameSprite, int _count = 1)
    {
        if(inventoryManager == null) inventoryManager = InventoryManager.Instance;

        itemData = _itemData; //아이템 데이터 넣기
        idx = itemData.idx; //아이템 번호 넣기
        itemImage.sprite = _nameSprite; //아이템 이미지 넣기
        itemImage.gameObject.SetActive(true); //아이템 이미지 오브젝트 활성화

        if (itemData.nameType == ItemType.Used.ToString()) //아이템이 소모품일 경우
        {
            objItemCount.SetActive(true); //아이템 갯수 창 활성화
            itemCount += _count;
            textCount.text = itemCount.ToString();
        }
    }

    /// <summary>
    /// InventoryManager 스크립트 전용 코드
    /// 만약 드래그 중 인벤토리가 꺼지게 되면 드래그 슬롯에 아이템이 남겨진 채로 남겨져있음
    /// 따라서 만약 꺼지게 될 경우 드래그 전 원래 상태로 되돌리기 위해 사용
    /// </summary>
    public void P_ReSetSlotItem()
    {
        if (isDragging)
        {
            Color setAlpha = new Color(1, 1, 1, 1); //기본 알파값 설정
            itemImage.color = setAlpha; //컬러 입히기
            isDragging = false;
        }
    }

    /// <summary>
    /// 아이템 정보 가져오기
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
    /// 아이템 갯수 확인하기
    /// 만약 소모품 아이템이 한 칸에 max값 보다 적으면 그 칸에 갯수 증가하고 초과되면 다음 칸에 채우기
    /// </summary>
    /// <returns></returns>
    public int P_GetItemCount()
    {
        return itemCount;
    }

    /// <summary>
    /// 드래그 정보 가져오기
    /// </summary>
    /// <returns></returns>
    public bool P_GetIsDragging()
    {
        return isDragging;
    }

    /// <summary>
    /// 슬롯에 마우스를 올린 채로 인벤토리를 끄고 마우스를 치운 다음 다시 키면
    /// 마우스가 위에 없어서 체크 오브젝트가 켜져있는 현상이 있으므로 방지하기 위해 사용
    /// </summary>
    public GameObject P_GetActiveSlot()
    {
        return checkImage;
    }
}
