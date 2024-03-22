using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Item item; //���Կ� �� ������
    [SerializeField] private Image itemImage; //������ �̹���
    [SerializeField] private GameObject checkImage; //���� �̹���, ���� �� Ŀ���� ������ ���� ����

    public void OnPointerEnter(PointerEventData eventData)
    {
        //���콺�� ���� ���� �÷��θ� ���� �̹��� Ȱ��ȭ
        checkImage.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //���콺�� ���� ������ ������ ���� �̹��� ��Ȱ��ȭ
        checkImage.SetActive(false);
    }

    private void Update()
    {
        CheckItem();
    }

    private void CheckItem()
    {
        if (item != null) //���Կ� �������� ������ ���
        {
            itemImage.gameObject.SetActive(true); //������ �̹��� ������Ʈ Ȱ��ȭ
            itemImage.sprite = item.P_GetItemSprite(); //����� �������� ��������Ʈ�� ��������
        }

        else
        {
            itemImage.gameObject.SetActive(false); //������ �̹��� ������Ʈ ��Ȱ��ȭ
            itemImage.sprite = null; //������ �̹��� ����
        }
    }

    /// <summary>
    /// ���Կ� ���콺�� �ø� ä�� �κ��丮�� ���� ���콺�� ġ�� ���� �ٽ� Ű��
    /// ���콺�� ���� ��� üũ ������Ʈ�� �����ִ� ������ �����Ƿ� �����ϱ� ���� ���
    /// </summary>
    public GameObject P_GetActiveSlot()
    {
        return checkImage;
    }
}
