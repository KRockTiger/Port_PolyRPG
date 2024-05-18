using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        Equip,
        Used,
    }

    [SerializeField] private int idx;
    [SerializeField] private ItemType itemType;

    public int P_GetItemIdx()
    {
        return idx;
    }

    public ItemType P_GetItemType()
    {
        return itemType;
    }
}
