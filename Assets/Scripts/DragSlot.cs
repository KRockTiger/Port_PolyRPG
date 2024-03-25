using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 아이템을 드래그할 때 사용하기 위한 스크립트
/// </summary>
public class DragSlot : MonoBehaviour
{
    public static DragSlot Instance; //싱글톤을 사용해서 인벤토리 내의 아이템을 넘길 수 있게 해줌

    [SerializeField] private Item item;
    [SerializeField] private Sprite itemSprite;

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
    }

    /// <summary>
    /// 드래그를 시작할 때 드래그 슬롯에 아이템 정보를 넘긴다.
    /// </summary>
    public void P_SetDragItem(Item _item)
    {
        item = _item; //아이템 정보 받기
    }
}
