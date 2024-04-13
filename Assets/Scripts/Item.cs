using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField] private int idx;

    public int P_GetItemIdx()
    {
        return idx;
    }
}
