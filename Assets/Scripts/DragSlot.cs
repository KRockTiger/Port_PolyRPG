using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 아이템을 드래그할 때 사용하기 위한 스크립트
/// </summary>
public class DragSlot : MonoBehaviour
{
    public static DragSlot Instance; //싱글톤을 사용해서 인벤토리 내의 아이템을 넘길 수 있게 해줌
    //private Slot dragSlot; //드래그 슬롯 스크립트에 슬롯 스크립트를 연결해보기 위해 만듦

    [SerializeField] private Item item;
    [SerializeField] private Image itemSprite;

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
    public void P_SetDragItem(Item _item)
    {
        item = _item; //아이템 정보 받기
        itemSprite.sprite = _item.P_GetItemSprite(); //아이템 이미지를 저장
    }

    public void P_ReSetDragItem()
    {
        item = null;
        itemSprite.sprite = null;
    }

    /// <summary>
    /// 아이템 정보 가져오기
    /// </summary>
    /// <returns></returns>
    public Item P_GetItem()
    {
        return item;
    }
}
