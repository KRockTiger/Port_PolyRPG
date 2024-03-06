using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //���� ���� �� ���콺 Ŀ�� ���
    }

    private void Update()
    {
        SwitchCursor();
    }

    /// <summary>
    /// ���콺 Ŀ�� ��ȯ
    /// </summary>
    private void SwitchCursor()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }

            else if (Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
