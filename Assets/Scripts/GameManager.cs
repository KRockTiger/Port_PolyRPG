using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //게임 시작 시 마우스 커서 잠금
    }

    private void Update()
    {
        SwitchCursor();
    }

    /// <summary>
    /// 마우스 커서 전환
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
