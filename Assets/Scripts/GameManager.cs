using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject inventory;
    [SerializeField,Tooltip("보일땐 True, 안보일 땐 False")] private bool isCursor;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //게임 시작 시 마우스 커서 잠금
        inventory.SetActive(false);
    }

    private void Update()
    {
        SetCursor();
        InputInventory();
    }

    /// <summary>
    /// 마우스 커서 전환
    /// </summary>
    private void SetCursor()
    {
        if (isCursor) //true일 땐 보이게 하기
        {
            Cursor.lockState = CursorLockMode.None;
        }

        else //false일 땐 끄게 하기
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
    /// 인벤토리 조작
    /// </summary>
    private void InputInventory()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            inventory.SetActive(!inventory.activeSelf); //인벤토리 상태에 따라 결정
            isCursor = inventory.activeSelf; //인벤토리가 보일 때 마우스 커서 온
        }
    }

}
#region 게임 빌드 화면 비율 셋팅 예시
    //private void SetReSolution()
    //{
    //    float targetRatio = 9f / 16f; //fhd
    //    float ratio = (float)Screen.width / (float)Screen.height;
    //    float scaleHeight = ratio / targetRatio; //비율
    //    float fixedWidth = (float)Screen.width / scaleHeight;
        
    //    Screen.SetResolution((int)fixedWidth, Screen.height, true);
    //}
#endregion
