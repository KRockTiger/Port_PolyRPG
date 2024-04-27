using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �������� �巡���� �� ����ϱ� ���� ��ũ��Ʈ
/// </summary>
public class DragSlot : MonoBehaviour
{
    public static DragSlot Instance; //�̱����� ����ؼ� �κ��丮 ���� �������� �ѱ� �� �ְ� ����
    //private Slot dragSlot; //�巡�� ���� ��ũ��Ʈ�� ���� ��ũ��Ʈ�� �����غ��� ���� ����

    //[SerializeField] private Item item;
    private ItemJson itemData; //������ ����
    private int itemCount;
    [SerializeField] private int idx = 0; //������ ��ȣ => 0�� �� ������ �ǹ���
    [SerializeField] private Image itemImage;

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
    public void P_SetDragItem(ItemJson _itemJson, int _itemCount, Sprite _itemImage)
    {
        //idx = _idx; //������ ���� �ޱ�
        itemData = _itemJson;
        idx = itemData.idx;
        itemCount = _itemCount;
        itemImage.sprite = _itemImage; //������ �̹����� ����
    }

    public void P_ReSetDragItem()
    {
        itemData = null;
        idx = 0;
        itemCount = 0;
        itemImage.sprite = null;
    }

    /// <summary>
    /// ������ ���� ��������
    /// </summary>
    /// <returns></returns>
    public ItemJson P_GetItemData()
    {
        return itemData;
    }

    /// <summary>
    /// ������ ��ȣ ��������
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

    public Sprite P_GetItemSprite()
    {
        return itemImage.sprite;
    }
}
