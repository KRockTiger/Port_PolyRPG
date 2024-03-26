using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    private DragSlot dragSlot;

    [SerializeField] private Item item; //슬롯에 들어갈 아이템
    [SerializeField] private Image itemImage; //아이템 이미지
    [SerializeField] private GameObject checkImage; //슬롯 이미지, 슬롯 위 커서의 유무에 따라 결정

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
        dragSlot.P_ReSetDragItem(); //아이템 지우기
        dragSlot.gameObject.SetActive(false); //드래그 슬롯 비활성화
        Color setAlpha = new Color(1, 1, 1, 1); //기본 알파값 설정
        itemImage.color = setAlpha; //컬러 입히기
    }

    /// <summary>
    /// 엔드 드래그는 자신이 드래그한 오브젝트의 판정으로 발생하고
    /// 온드랍은 마우스가 올려져 있는 오브젝트를 기준으로 마우스 클릭이 때지면 발생한다.
    /// 즉 서로 다른 오브젝트에서 코드가 출력된다.
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
    /// 슬롯에 아이템 추가
    /// </summary>
    private void GetSlotItem(Item _item)
    {
        item = _item;
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
