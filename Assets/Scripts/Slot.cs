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

    [SerializeField] private ItemData itemData; //아이템 정보
    [SerializeField] private ItemType itemType; //아이템 종류

    //[SerializeField] private Item item; //슬롯에 들어갈 아이템
    [SerializeField] int idx; //아이템 번호 확인
    [SerializeField] int itemCount; //아이템 갯수
    [SerializeField] float setUseCoolTime; //설정한 아이템 사용 쿨타임
    [SerializeField] float curUseCoolTime; //현재 아이템 사용 쿨타임
    [SerializeField] private Image itemImage; //아이템 이미지
    [SerializeField] private GameObject checkImage; //슬롯 이미지, 슬롯 위 커서의 유무에 따라 결정
    [SerializeField] private GameObject objItemCount; //아이템 갯수 오브젝트
    [SerializeField] private TMP_Text textCount;

    [SerializeField] private bool isDragging; //드래그 중인 상태일 경우 true
    [SerializeField] private bool isUsedFull; //소모품 아이템의 갯수가 최대일 경우 true
    [SerializeField] private bool isClickSlot; //슬롯 내 아이템을 클릭가능 여부
    [SerializeField] private bool isCoolTimeSlot; //슬롯 내 쿨타임 여부

    private int maxItemCount = 9; //슬롯에 들어갈 수 있는 아이템 최대 갯수

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

        //마우스 오른쪽 버튼으로 아이템 사용
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            UseSlot();
        }

        if (isCoolTimeSlot) //만약 아이템 쿨타임이 발생한다면
        {
            curUseCoolTime -= Time.deltaTime; //쿨다운
            if (curUseCoolTime < 0)
            {
                isCoolTimeSlot = false;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //마우스를 슬롯 위에 올려두면 슬롯 이미지 활성화
        checkImage.SetActive(true);
        isClickSlot = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //마우스를 슬롯 밖으로 꺼내면 슬롯 이미지 비활성화
        checkImage.SetActive(false);
        isClickSlot = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //드래그를 시작할 때 드래그 슬롯에 아이템 정보를 넘겨야 함
        if (idx != 0) //슬롯에 아이템이 있을 때만 실행
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
        if (dragSlot.P_GetItemIdx() != 0)
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
        if (dragSlot.P_GetItemIdx() != 0)
        {
            //item = dragSlot.P_GetItem(); //현 슬롯에 아이템을 넣기
            idx = dragSlot.P_GetItemIdx();
            itemData = dragSlot.P_GetItemData(); //현 슬롯에 아이템을 넣기
            itemCount = dragSlot.P_GetItemCount();
            itemImage.sprite = dragSlot.P_GetItemSprite();
            dragSlot.P_ReSetDragItem(); //아이템 지우기
            dragSlot.gameObject.SetActive(false); //드래그 슬롯 비활성화
            objItemCount.SetActive(itemData.nameType == ItemType.Used.ToString());
            CheckItemType(itemData.nameType); //슬롯 내 아이템 타입 설정
        }

        else //아이템이 없는 경우
        {
            itemData = null; //아이템 지우기
            idx = 0;
            itemCount = 0;
            itemImage.sprite = null; //아이템 이미지 삭제
            itemImage.gameObject.SetActive(false); //이미지 오브젝트 비활성화
            dragSlot.gameObject.SetActive(false); //드래그 슬롯 비활성화
            objItemCount.SetActive(false); //아이템 갯수 창 비활성화
            itemType = ItemType.Null; //아이템이 존재하지 않으므로 Null로 설정
        }
    }

    /// <summary>
    /// 현 스크립트를 가진 오브젝트 위에 마우스를 땔 때
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrop(PointerEventData eventData)
    {
        ItemData dragItemData = dragSlot.P_GetItemData();

        //드래그 슬롯에 아이템이 존재할 경우
        if (dragSlot.P_GetItemIdx() != 0)
        {
            //아이템 교체
            //ChangeItem(); //단순히 이것만 쓰면 버그가 없음

            if (dragItemData.nameType != "Used") //드래그 중인 아이템이 소모품이 아닌 경우
            {
                //아이템 교체
                ChangeItem();
            }

            else if (dragItemData.nameType == "Used") //드래그 중인 아이템이 소모품일 경우
            {
                if (dragItemData.idx == idx) //드래그 슬롯의 아이템과 드롭한 슬롯의 아이템이 동일할 경우
                {
                    //나뉘어져 있던 동일한 소모품 아이템을 합친다.
                    SumItem();
                }

                else if (dragItemData.idx != idx) //동일한 아이템이 아닐 경우
                {
                    ChangeItem();
                }
            }
        }
    }

    /// <summary>
    /// 한 슬롯에 있는 소모품의 아이템 갯수가 최대치가 아닌 갯수를 가지고 있을 때 같은 아이템에 있는 슬롯에 드래그하면
    /// 드롭한 슬롯에 아이템이 더해지고 만약 더하고 남은 아이템이 존재할 경우 드래그를 시작한 슬롯에 남기게 한다.
    /// OnDrop함수 아래에 사용하므로 순서 주의
    /// </summary>
    private void SumItem()
    {
        //이 함수는 동일한 아이템을 합산한다는 조건아래에 사용하므로 갯수만 구한다.
        int dragItemCount = dragSlot.P_GetItemCount();

        if (dragItemCount == maxItemCount) //이미 드래그 슬롯의 아이템 갯수가 최대치일 경우
        {
            ChangeItem();
        }

        else //최대치가 아닐 경우
        {
            int sumCount = dragItemCount + itemCount;

            //합쳐서 최대치 이하일 경우
            //드롭한 슬롯에 아이템을 옮기고 드래그한 슬롯의 아이템을 삭제
            if (sumCount <= maxItemCount && isDragging == false)
            {
                itemCount = sumCount; //드롭 슬롯에 합산
                dragSlot.P_ReSetDragItem(); //드래그 슬롯의 아이템을 삭제
            }

            //합쳐서 최대치 초과할 경우
            //드롭한 슬롯의 아이템 갯수는 최대치로 놓고 남는 갯수를 드래그 슬롯의 아이템 갯수로 저장
            else if (sumCount > maxItemCount)
            {
                if (!isUsedFull) //드롭한 슬롯의 아이템 갯수가 최대가 아닌경우
                {
                    itemCount = maxItemCount; //아이템 갯수를 최대치로 설정
                    dragSlot.P_SetItemCount(sumCount - maxItemCount); //남는 갯수를 드래그 슬롯에 저장
                }

                else //드롭한 슬롯의 아이템 갯수가 최대인 경우
                {
                    ChangeItem();
                }
            }
        }
    }

    /// <summary>
    /// 드래그로 아이템 자리 교체
    /// A 슬롯 : 드래그 한 슬롯
    /// B 슬롯 : 드롭할 슬롯
    /// </summary>
    private void ChangeItem()
    {
        ItemData tempItemData = itemData; //B 슬롯의 아이템 번호를 임의의 변수에 저장
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
        CheckItemType(itemData.nameType);

        if (tempItemIdx != 0) //드롭한 슬롯에 아이템이 있으면
        {
            dragSlot.P_SetDragItem(tempItemData, tempItemCount, tempItemSprite); //임의로 저장된 아이템을 드래그 슬롯에 저장
        }
    }

    /// <summary>
    /// 슬롯 내 아이템을 사용하는 함수
    /// </summary>
    private void UseSlot()
    {
        if (!isClickSlot || isCoolTimeSlot) //슬롯 사용 가능한 상태 혹은 아이템 쿹라임이면 리턴
        {
            return;
        }

        switch (itemType) //아이템 타입에 따라 결정
        {
            case ItemType.Equip:
                //클릭한 장비를 장착 및 변경
                int dempIdx = inventoryManager.P_EquipChange(idx); //현 슬롯 아이템 번호를 인벤에 넘기고 장착 아이템의 번호를 가져온다
                itemData = inventoryManager.GetItemJson(dempIdx); //가져온 번호를 토대로 Json에 등록된 아이템을 입력
                idx = itemData.idx;
                itemImage.sprite = inventoryManager.GetSprite(dempIdx);
                break;

            case ItemType.Used:
                //소모품 아이템을 1개 소모하여 사용
                itemCount -= 1;
                if (itemCount == 0) //아이템 갯수가 0이 되면 빈 슬롯으로 만들기 => 아이템 데이터 제거
                {
                    itemData = null; //아이템 지우기
                    idx = 0;
                    itemCount = 0;
                    itemImage.sprite = null; //아이템 이미지 삭제
                    itemImage.gameObject.SetActive(false); //이미지 오브젝트 비활성화
                    dragSlot.gameObject.SetActive(false); //드래그 슬롯 비활성화
                    objItemCount.SetActive(false); //아이템 갯수 창 비활성화
                    itemType = ItemType.Null; //아이템이 존재하지 않으므로 Null로 설정
                }
                curUseCoolTime = setUseCoolTime; //쿨타임 설정
                isCoolTimeSlot = true;
                break;

            case ItemType.Null:
                Debug.Log("슬롯 내 아이템이 존재하지 않습니다.");
                break;
        }   
    }

    /// <summary>
    /// 슬롯에 아이템 추가
    /// 03.26) 외부에서 아이템을 획득하는 코드를 사용하기 때문에 public으로 교체
    /// 04.13) 아이템 정보를 Item.sc => Item.json으로 변경
    /// </summary>
    public void P_AddItem(ItemData _itemData, Sprite _nameSprite)
    {
        if(inventoryManager == null) inventoryManager = InventoryManager.Instance;

        itemData = _itemData; //아이템 데이터 넣기
        idx = itemData.idx; //아이템 번호 넣기
        itemImage.sprite = _nameSprite; //아이템 이미지 넣기
        itemImage.gameObject.SetActive(true); //아이템 이미지 오브젝트 활성화

        if (itemData.nameType == ItemType.Used.ToString()) //아이템이 소모품일 경우
        {
            objItemCount.SetActive(true); //아이템 갯수 창 활성화
            textCount.text = itemCount.ToString(); //아이템 갯수를 string으로 변형
        }

        CheckItemType(itemData.nameType);
    }

    /// <summary>
    /// json에 설정한 타입을 불러와서 슬롯 내 아이템 종류에 저장
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

    public ItemData P_GetItemData()
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
    /// 소모품 아이템의 갯수를 늘리는 함수
    /// </summary>
    /// <param name="_count"></param>
    public void P_SetItemCount(int _count)
    {
        itemCount += _count;
        if (itemCount >= maxItemCount) //아이템 갯수 숫자가 최대 이상일 경우
        {
            isUsedFull = true;
        }
    }

    /// <summary>
    /// 드래그 정보 가져오기
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

    /// <summary>
    ///  
    /// </summary>
    public GameObject P_GetActiveSlot()
    {
        return checkImage;
    }
}
