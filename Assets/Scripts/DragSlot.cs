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
    [SerializeField] private int idx = -1; //������ ��ȣ => -1�� �� ������ �ǹ���
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
    public void P_SetDragItem(int _idx, Sprite _itemImage)
    {
        idx = _idx; //������ ���� �ޱ�
        itemImage.sprite = _itemImage; //������ �̹����� ����
    }

    public void P_ReSetDragItem()
    {
        idx = -1;
        itemImage.sprite = null;
    }

    /// <summary>
    /// ������ ���� ��������
    /// </summary>
    /// <returns></returns>
    public int P_GetItemIdx()
    {
        return idx;
    }

    public Sprite P_GetItemSprite()
    {
        return itemImage.sprite;
    }
}
