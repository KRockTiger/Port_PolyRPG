using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private DragSlot dragSlot;

    [SerializeField] private Item item; //슬롯에 들어갈 아이템
    [SerializeField] private Image itemImage; //아이템 이미지
    [SerializeField] private GameObject checkImage; //슬롯 이미지, 슬롯 위 커서의 유무에 따라 결정

    [SerializeField] private bool isDragging; //드래그 중인 상태일 경우 true

    private void Start()
    {
        dragSlot = DragSlot.Instance;
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
        if (item != null) //슬롯에 아이템이 있을 때만 실행
        {
            isDragging = true;
            dragSlot.gameObject.SetActive(true); //드래그 슬롯 활성화
            dragSlot.P_SetDragItem(item); //드래그 슬롯에 아이템 넣기
            Color setAlpha = new Color(1, 1, 1, 0.5f); //반투명 설정
            itemImage.color = setAlpha; //컬러 입히기
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //드래그 슬롯에 아이템이 존재할 때만 적용
        if (dragSlot.P_GetItem() != null)
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
        if (dragSlot.P_GetItem() != null)
        {
            item = dragSlot.P_GetItem(); //현 슬롯에 아이템을 넣기
            dragSlot.P_ReSetDragItem(); //아이템 지우기
            dragSlot.gameObject.SetActive(false); //드래그 슬롯 비활성화
        }

        else //아이템이 없는 경우
        {
            item = null; //아이템 지우기
            dragSlot.gameObject.SetActive(false); //드래그 슬롯 비활성화
        }
    }

    /// <summary>
    /// 현 스크립트를 가진 오브젝트 위에 마우스를 땔 때
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrop(PointerEventData eventData)
    {
        //드래그 슬롯에 아이템이 존재할 경우
        if (dragSlot.P_GetItem() != null)
        {
            //아이템 교체
            ChangeItem();
        }
    }

    private void Update()
    {
        CheckItem();
    }

    /// <summary>
    /// 슬롯 내 아이템 확인
    /// -추후 Update로 상시 확인이 아닌 아이템을 넣거나 빼는 순간에만 사용할 수 있도록 수정 필요
    /// </summary>
    private void CheckItem()
    {
        if (item != null) //슬롯에 아이템이 존재할 경우
        {
            itemImage.gameObject.SetActive(true); //아이템 이미지 오브젝트 활성화
            itemImage.sprite = item.P_GetItemSprite(); //저장된 아이템의 스프라이트를 가져오기
        }

        else
        {
            itemImage.gameObject.SetActive(false); //아이템 이미지 오브젝트 비활성화
            itemImage.sprite = null; //아이템 이미지 비우기
        }
    }

    /// <summary>
    /// 드래그로 아이템 자리 교체
    /// A 슬롯 : 드래그 한 슬롯
    /// B 슬롯 : 드롭할 슬롯
    /// </summary>
    private void ChangeItem()
    {
        Item tempItem = item; //B 슬롯의 아이템을 임의의 변수에 저장
        item = dragSlot.P_GetItem(); //B슬롯에 드래그한 아이템을 저장
        dragSlot.P_ReSetDragItem(); //드래그 슬롯에 있는 아이템 삭제

        if (tempItem != null) //드롭한 슬롯에 아이템이 있으면
        {
            dragSlot.P_SetDragItem(tempItem); //임의로 저장된 아이템을 드래그 슬롯에 저장
        }
    }

    /// <summary>
    /// 슬롯에 아이템 추가
    /// 03.26) 외부에서 아이템을 획득하는 코드를 사용하기 때문에 public으로 교체
    /// </summary>
    public void P_AddItem(Item _item)
    {
        item = _item;
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
    public Item P_GetItem()
    {
        return item;
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
