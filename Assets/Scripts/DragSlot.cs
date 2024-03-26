using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// �������� �巡���� �� ����ϱ� ���� ��ũ��Ʈ
/// </summary>
public class DragSlot : MonoBehaviour
{
    public static DragSlot Instance; //�̱����� ����ؼ� �κ��丮 ���� �������� �ѱ� �� �ְ� ����
    //private Slot dragSlot; //�巡�� ���� ��ũ��Ʈ�� ���� ��ũ��Ʈ�� �����غ��� ���� ����

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
    /// �巡�׸� ������ �� �巡�� ���Կ� ������ ������ �ѱ��.
    /// </summary>
    public void P_SetDragItem(Item _item)
    {
        item = _item; //������ ���� �ޱ�
        itemSprite.sprite = _item.P_GetItemSprite(); //������ �̹����� ����
    }

    public void P_ReSetDragItem()
    {
        item = null;
        itemSprite.sprite = null;
    }

    /// <summary>
    /// ������ ���� ��������
    /// </summary>
    /// <returns></returns>
    public Item P_GetItem()
    {
        return item;
    }
}
