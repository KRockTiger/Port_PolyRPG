using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject inventory;
    [SerializeField,Tooltip("���϶� True, �Ⱥ��� �� False")] private bool isCursor;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //���� ���� �� ���콺 Ŀ�� ���
        inventory.SetActive(false);
    }

    private void Update()
    {
        SetCursor();
        InputInventory();
    }

    /// <summary>
    /// ���콺 Ŀ�� ��ȯ
    /// </summary>
    private void SetCursor()
    {
        if (isCursor) //true�� �� ���̰� �ϱ�
        {
            Cursor.lockState = CursorLockMode.None;
        }

        else //false�� �� ���� �ϱ�
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            isCursor = !isCursor;

            //if (Cursor.lockState == CursorLockMode.Locked)
            //{
            //    Cursor.lockState = CursorLockMode.None;
            //}

            //else if (Cursor.lockState == CursorLockMode.None)
            //{
            //    Cursor.lockState = CursorLockMode.Locked;
            //}
        }
    }

    /// <summary>
    /// �κ��丮 ����
    /// </summary>
    private void InputInventory()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            inventory.SetActive(!inventory.activeSelf); //�κ��丮 ���¿� ���� ����
            isCursor = inventory.activeSelf; //�κ��丮�� ���� �� ���콺 Ŀ�� ��
        }
    }

}
#region ���� ���� ȭ�� ���� ���� ����
    //private void SetReSolution()
    //{
    //    float targetRatio = 9f / 16f; //fhd
    //    float ratio = (float)Screen.width / (float)Screen.height;
    //    float scaleHeight = ratio / targetRatio; //����
    //    float fixedWidth = (float)Screen.width / scaleHeight;
        
    //    Screen.SetResolution((int)fixedWidth, Screen.height, true);
    //}
#endregion
