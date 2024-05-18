using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlot : MonoBehaviour
{
    public static QuickSlot Instance;

    [SerializeField] private Image itemImage;
    [SerializeField] private KeyCode useKey; //Äü½½·Ô »ç¿ë Å°

    private void Awake()
    {
        #region ½Ì±ÛÅæ
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
        if (Input.GetKeyDown(useKey)) //Z¸¦ »ç¿ë
        {
            //½½·Ô ³» ¾ÆÀÌÅÛ »ç¿ë
        }
    }
}
