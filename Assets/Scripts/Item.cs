using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField] private Sprite itemSprite;

    public Sprite P_GetItemSprite()
    {
        return itemSprite;
    }
}
