using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// �������� �巡���� �� ����ϱ� ���� ��ũ��Ʈ
/// </summary>
public class DragSlot : MonoBehaviour
{
    public static DragSlot Instance; //�̱����� ����ؼ� �κ��丮 ���� �������� �ѱ� �� �ְ� ����

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
    /// �巡�׸� ������ �� �巡�� ���Կ� ������ ������ �ѱ��.
    /// </summary>
    public void P_SetDragItem(Item _item)
    {
        item = _item; //������ ���� �ޱ�
    }
}
