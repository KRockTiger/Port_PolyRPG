using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlot : MonoBehaviour
{
    public static QuickSlot Instance;

    [SerializeField] private Image itemImage;
    [SerializeField] private KeyCode useKey; //������ ��� Ű

    private void Awake()
    {
        #region �̱���
        if (Instance == null)
        {
            Instance = this;
        }

        else
        {
            Destroy(gameObject);
        }
        #endregion
    }

    private void Update()
    {
        UseSlot();
    }

    private void UseSlot()
    {
        if (Input.GetKeyDown(useKey)) //Z�� ���
        {
            //���� �� ������ ���
        }
    }
}
