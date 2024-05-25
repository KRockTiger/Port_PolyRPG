using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuickItem : MonoBehaviour
{
    [SerializeField] private QuickSlot quickSlot; //������ ����
    [SerializeField] private ItemController itemController; //������ ��Ʈ�ѷ� ����

    [SerializeField] private KeyCode useKey; //������ ��� �ڵ�

    [Header("������ ������ ����")]
    [SerializeField] private ItemData itemData;
    [SerializeField] private Image itemImage;
    [SerializeField] private Image coolBlurImage;
    [SerializeField] private TMP_Text objItemCount;
    [SerializeField] private int itemCount;

    private void Update()
    {
        GetQuickSlotData();
        UseItem();
    }

    /// <summary>
    /// �������� ������ ������ �ǽð����� ������
    /// </summary>
    private void GetQuickSlotData()
    {
        if (quickSlot.P_GetItemIdx() != 0) //���� �����Կ� �������� �����Ѵٸ�
        {
            itemData = quickSlot.P_GetItemData(); //������ ��������
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = quickSlot.P_GetItemSprite(); //������ �̹��� ��������
            itemCount = quickSlot.P_GetItemCount(); //���� ������ ���� ��������
            objItemCount.gameObject.SetActive(true);
            objItemCount.text = itemCount.ToString();
            SetCoolBlur(itemData.nameSmallType); //Ư�� ������ ��Ÿ�� ǥ��
        }

        else //�����Կ� �������� ������
        {
            itemData = null;
            itemImage.gameObject.SetActive(false);
            itemImage.sprite = null;
            itemCount = 0;
            objItemCount.gameObject.SetActive(false);
            coolBlurImage.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// ������ ������ ���
    /// </summary>
    private void UseItem()
    {
        if (Input.GetKeyDown(useKey) && quickSlot.P_GetItemIdx() != 0) //�����Կ� �������� ������ ���¿��� Ű�� ������
        {
            string itemSmallType = itemData.nameSmallType;

            //���� ��Ÿ�� ���� ������ ������ ��� ����
            if (itemController.P_SearchCoolType(itemSmallType))
            {
                Debug.Log("���� ��Ÿ�� ���� �������Դϴ�.");
                return;
            }

            quickSlot.P_SetItemCount(1); //������ 1�� �Ҹ�
            quickSlot.P_SetCount(); //�������� ������ ���� ǥ��

            if (quickSlot.P_GetItemCount() <= 0) //������ �� �������� �� ����� ���
            {
                quickSlot.P_ReSetItemData(); //������ �����
            }

            itemController.P_CoolOn(itemSmallType); //Ư�� ��������(ex.ȸ�� ������)�� ��� ������ ��Ÿ�� ����
        }
    }

    /// <summary>
    /// �������� ����Ͽ� ��Ÿ���� �߻����� �� �������� ���� ���� UI�� ǥ��
    /// </summary>
    private void SetCoolBlur(string _itemSmallType)
    {
        //��Ÿ���� ������ ���� �̹��� ���� ����
        coolBlurImage.fillAmount = itemController.P_FillAmoutCoolTime(_itemSmallType);

        //��Ÿ�� ������ ���� ������Ʈ Ȱ��ȭ ó��
        coolBlurImage.gameObject.SetActive(itemController.P_SearchCoolType(_itemSmallType));
    }
}