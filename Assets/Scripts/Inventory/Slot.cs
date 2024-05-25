using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

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

        if (isCoolTimeSlot) //만약 아이템 쿨타임이 발생한다면
        {
            curUseCoolTime -= Time.unscaledDeltaTime; //쿨다운
                                              //(05.11 버그 => TimeScale이 1이 아니면 -= Time.deltaTime 코드가 제대로 작동 안됨)
                                              //게다가 인벤토리가 비활성화 상태에도 Update가 작동이 안됨
            if (curUseCoolTime < 0)
            {
                isCoolTimeSlot = false;
            }
            
            //05.11) 슬롯 스크립트 혼자서는 TimeScale, 오브젝트 비활성화로 인한 코드 중지 라는 헛점으로 인해 쿨타임 관리가 힘듦
            //그렇다면 모든 슬롯의 컴포넌트를 가진 InventoryManager가 직접 각 슬롯의 쿨타임을 관리해야함
            //하지만 여기서 생기는 문제
            //1. 인벤매니저 혼자서 어떻게 많은 슬롯의 쿨타임을 각각 체크하여 관리하는가
            //2. GamePause기능으로 TimeScale = 0f일 때 -= timeScale 이 작동하지 않는다. 그러면 어떻게 쿨타임을 인벤 매니저가 관리하는가

            //==> 강사님 의견 : 아이템 회복을 인벤토리에서 먹지 말고 퀵슬롯을 만들어서 관리하는 식으로 설계를 하는 것을 추천함
            //===> 실시간 전투 컨셉의 게임에서 인벤토리로 게임을 멈추고 회복하는 것은 게임성에 문제가 될 수 있는 가능성을 보여주심
            //     오히려 인벤토리를 켰을 때 쿨타임을 멈춰서 게임 취지에 맞는 밸런스를 만드는 것을 추천하심
            //따라서 슬롯이 아닌 동일한 아이템의 쿨타임을 관리해주는 CoolTimeManager 스크립트를 만들어서 관리하는 것을 추천

            //==> 피드백 후 내 생각 : 현 게임은 원신의 기능을 만들려는 목적을 토대로 기획하여 만들 기능의 영향을 깊게 생각 안했었음
            //                      모작을 하는 것도 좋지만 만들 기능이 게임 밸런스에 어떠한 영향을 끼칠 수 있는지 생각할 수 있게 해주는
            //                      느낌을 받았음 그리고 인벤에서 회복하는 것보다 퀵슬롯으로 피관리 하는 것이 게임성에 더 좋을 것 같다고
            //                      느껴짐 하지만 아이템 중복 입력 방지를 목적으로 만드는 쿨타임은 괜찮다고 피드백 받아 나중에 쓸만 할듯
        }
        
        //마우스 오른쪽 버튼으로 아이템 사용
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            UseSlot();
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

    /// <summary>
    /// 1)마우스가 슬롯 위에 있는 상태에서 인벤토리를 끄고 마우스를 치우고 나서 다시 인벤토리를 키면
    /// 그 슬롯이 활성화 상태로 유지되는 버그가 생기기 때문에 외부에서 강제로 끄게 만든다.
    /// -현 코드를 슬롯의 정보를 가진 InventoryManager 스크립트에 넘기고
    /// -GameManager에서 for문으로 슬롯을 체크 하여 강제로 끄게 하기
    /// </summary>
    public void P_SlotOff()
    {
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
        //만약 드래그 슬롯의 아이템이 퀵슬롯 아이템이면서 현 슬롯의 아이템의 종류가 장비일 경우
        if (dragSlot.P_GetIsQuickItem() && itemType == (ItemType)Enum.Parse(typeof(ItemType), "Equip"))
        {
            return;
        }

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

            //드래그 중인 아이템이 소모품이면서 퀵슬롯이 아닌 경우
            else if (dragItemData.nameType == "Used" && !dragSlot.P_GetIsQuickItem())
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

            //드래그 중인 아이템이 퀵슬롯의 소모품일 경우
            else if (dragItemData.nameType == "Used" && dragSlot.P_GetIsQuickItem())
            {
                //그냥 아이템 교체
                ChangeItem();
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
        textCount.text = itemCount.ToString();
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
