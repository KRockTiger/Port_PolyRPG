using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    [SerializeField] private InventoryManager inventory;

    [SerializeField] private float pickUpRange; //�������� ȹ���� �� �ִ� �Ÿ�
    [SerializeField] private float shortDistance; //���� ����� �������� �Ÿ�
    [SerializeField] private KeyCode pickUpKeyCode; //ȹ�� Ű
    [SerializeField] private string pickUpText; //ȹ�� �ؽ�Ʈ
    [SerializeField] private GameObject objPickUpText; //ȹ�� �ؽ�Ʈ ������Ʈ

    [SerializeField] private Transform targetItem;

    private void Start()
    {
        inventory = InventoryManager.Instance;
    }

    private void Update()
    {
        SearchItem();
        GetItemUI();
    }

    /// <summary>
    /// ����� �������� ã�� �ڵ�
    /// ���� ã�Ƴ��� �ڵ�� ����
    /// </summary>
    private void SearchItem()
    {
        //������ �±��ִ� ������Ʈ ã��
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");

        shortDistance = Mathf.Infinity; //�ִ� ��ġ�� ����

        Transform nearItem = null; //���� ��ó�� �ִ� �������� ���� ����

        foreach (GameObject item in items)
        {
            //�����۰��� �Ÿ� ����
            float distance = Vector3.Distance(item.transform.position, transform.position);

            if (distance < shortDistance) //������ �Ÿ��� ���� ����� �����۰��� �Ÿ����� ª����
            {
                shortDistance = distance; //�ּ� �Ÿ� �缳��

                nearItem = item.transform;
            }

            if (nearItem != null) //��ó�� �������� ������ ���
            {
                targetItem = nearItem; //Ÿ�� ������ ����
            }

            else //�������� ���� ���
            {
                targetItem = null; //���� ����
            }
        }
    }

    private void GetItemUI()
    {
        if (targetItem != null)
        {
            if (shortDistance <= pickUpRange) //���� ����� �������� �Ⱦ� ���� �����ȿ� ���� ���
            {
                objPickUpText.SetActive(true); //������ �ؽ�Ʈ ������Ʈ Ȱ��ȭ

                if (Input.GetKeyDown(pickUpKeyCode))
                {
                    InputGetItem(targetItem.GetComponent<Item>());
                    //=> ������ �±װ� �ִ� ������Ʈ�� Item ������Ʈ�� �پ� �����Ƿ� ������Ʈ�� �ִ� ������ �����´�.
                }
            }

            else //������ ȹ�� �������� �ָ� ���� ���
            {
                objPickUpText.SetActive(false); //�ؽ�Ʈ ������Ʈ ��Ȱ��ȭ
            }
        }

        else //�ֺ��� �������� ������
        {
            objPickUpText.SetActive(false); //�ؽ�Ʈ ������Ʈ ��Ȱ��ȭ
        }
    }

    /// <summary>
    /// ������ ȹ�� Ű
    /// </summary>
    private void InputGetItem(Item _item)
    {
        inventory.P_InputGetItem(_item);
        //Destroy(_item.gameObject);
        //targetItem = null; //Ÿ�� ����
    }
}
