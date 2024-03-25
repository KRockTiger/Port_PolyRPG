using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
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
        dragSlot.P_SetDragItem(item);

    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }

    private void Update()
    {
        CheckItem();
    }

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
    /// 슬롯에 마우스를 올린 채로 인벤토리를 끄고 마우스를 치운 다음 다시 키면
    /// 마우스가 위에 없어서 체크 오브젝트가 켜져있는 현상이 있으므로 방지하기 위해 사용
    /// </summary>
    public GameObject P_GetActiveSlot()
    {
        return checkImage;
    }

}
