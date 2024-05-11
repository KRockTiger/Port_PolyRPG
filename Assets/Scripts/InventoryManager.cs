using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] private DragSlot dragSlot;

    [SerializeField] private int maxSlotItemCount; //하나의 슬롯에 들어갈 수 있는 최대 아이템 갯수
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject itemSlots;
    [SerializeField] private Slot[] slots;
    [SerializeField] private EquipSlot equipSlot;

    [SerializeField] private Item addItem; //치트로 추가할 임의의 아이템으로 추후 삭제 예정

    public enum jsonType
    {
        Item,
    }
    [SerializeField] List<TextAsset> listJsons; //json파일 등록
    [SerializeField] List<ItemData> listJsonItem; //저장된 아이템의 json파일(확인용)

    [SerializeField] List<Sprite> listSpr;
    [SerializeField] List<GameObject> listObjEquip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        else
        {
            Destroy(gameObject);
        }
        string json = listJsons[(int)jsonType.Item].text;
        listJsonItem = JsonConvert.DeserializeObject<List<ItemData>>(json);

        //Sprite spr = P_GetSprite(1);
    }

    private void Start()
    {
        slots = itemSlots.GetComponentsInChildren<Slot>();
        equipSlot.P_StartSetBasicItem();
        
        //dragSlot = DragSlot.Instance;

        //임의로 아이템 넣기 => json으로 아이템을 잘 넣을 수 있는지 확인 후 삭제
        //P_InputGetItem(0);
        //P_InputGetItem(1);
    }

    /// <summary>
    /// GameManager에서 인벤토리를 비활성화 할 때 만약 켜져있는 슬롯이 있으면 강제로 끄게 하기
    /// </summary>
    public void P_CheckSlots()
    {
        int count = slots.Length; //슬롯 개수 확인

        for (int iNum = 0; iNum < count; iNum++)
        {
            //모든 슬롯의 체크 이미지 오브젝트의 활성화 여부를 확인
            if (slots[iNum].P_GetIsClickSlot() == true)
            {
                //만약 켜져있으면 비활성화
                slots[iNum].P_UnIsClickSlot(); //슬롯 클릭 비활성화
                slots[iNum].P_GetActiveSlot().SetActive(false);
            }

            //드래그 중인 슬롯 확인
            if (slots[iNum].P_GetIsDragging())
            {
                //드래그 중이였던 아이템일 경우 되돌리기
                slots[iNum].P_ReSetSlotItem();
            }
        }

        dragSlot.P_ReSetDragItem();
        dragSlot.gameObject.SetActive(false);
    }

    /// <summary>
    /// 키를 입력하여 임의의 아이템을 인벤토리에 저장
    /// -치트용 코드이므로 아이템 획득이 잘 작동되면 추후 삭제 예정
    /// </summary>
    public void P_InputGetItem(int _idx, int _count = 1)
    {
        int count = slots.Length; //슬롯 개수 확인
        ItemData itemData = GetItemJson(_idx); //아이템 정보 가져오기

        if (itemData.nameType != "Used")
        //획득한 아이템이 소모품이 아닐 경우
        {
            //아이템 번호를 이용하여 슬롯의 아이템 유무 및 종류 확인하기
            for (int iNum01 = 0; iNum01 < count; iNum01++)
            {
                if (slots[iNum01].P_GetItemIdx() == 0)
                //아이템 번호가 -1이면 빈 슬롯
                {
                    slots[iNum01].P_AddItem(GetItemJson(_idx), GetSprite(_idx));
                    return; //아이템이 추가되면 리턴하여 멈추게 하기
                }
            }
        }

        else if (itemData.nameType == "Used")
        //슬롯에 들어가는 아이템이 소모품일 경우
        {
            //슬롯 전체 확인
            for (int iNum02 = 0; iNum02 < count; iNum02++)
            {
                if (itemData.idx == slots[iNum02].P_GetItemIdx())
                //슬롯에 동일한 아이템이 존재할 경우
                {
                    if (!slots[iNum02].P_GetIsUsedFull())
                    //동일한 아이템이 존재하지만 아이템 갯수가 최대치가 아닌 경우
                    {
                        //임의로 계산한 슬롯의 아이템 갯수와 획득할 아이템 갯수의 합
                        //하나의 슬롯이 최대로 가질 수 있는 소모품 갯수를 임의의 최대 기준으로 등록 설정
                        int sumItemCount = slots[iNum02].P_GetItemCount() + _count;

                        if (sumItemCount <= maxSlotItemCount)
                        //기존 슬롯에 있는 아이템 갯수와 획득한 아이템 갯수의 합이 최대 이하일 경우
                        {
                            slots[iNum02].P_SetItemCount(_count); //제시된 아이템의 수만큼 증가시키기
                            return;
                        }

                        else if (sumItemCount > maxSlotItemCount) //갯수의 합이 최대 초과일 경우                    
                        {
                            int overCount = sumItemCount - maxSlotItemCount;
                            for (int iNum03 = 0; iNum03 < count; iNum03++)
                            {
                                if (slots[iNum03].P_GetItemIdx() == 0)
                                //아이템 번호가 0이면 빈 슬롯
                                {
                                    slots[iNum03].P_AddItem(GetItemJson(_idx), GetSprite(_idx)); //슬롯에 아이템 추가
                                    slots[iNum03].P_SetItemCount(overCount); //갯수 증가
                                    return;
                                }
                            }
                        }
                    }
                }
            }

            //위에서 return이 되지 않으면 슬롯에 동일한 아이템이 존재하지 않으므로 새로 추가
            for (int iNum04 = 0; iNum04 < count; iNum04++)
            {
                if (slots[iNum04].P_GetItemIdx() == 0)
                //아이템 번호가 -1이면 빈 슬롯
                {
                    slots[iNum04].P_AddItem(GetItemJson(_idx), GetSprite(_idx)); //슬롯에 아이템 추가
                    slots[iNum04].P_SetItemCount(_count); //갯수 증가
                    return;
                }
            }
        }
    }

    public string P_GetItemType(int _idx)
    {
        return listJsonItem[_idx].nameType;
    }

    public ItemData GetItemJson(int _idx)
    {
        ItemData data = listJsonItem.Find(x => x.idx == _idx);
        
        if (data == null)
        {
            Debug.LogError("존재하지 않은 아이템입니다.");
            return null;
        }
        
        return data;
    }

    public Sprite GetSprite(int _idx)
    {
        ItemData data = listJsonItem.Find(x => x.idx == _idx);
        if (data == null)
        {
            Debug.LogError($"올바른값이 들어오지 않았습니다.\n idx = {_idx}");
            return null;
        }

        return listSpr.Find(x => x.name == data.nameSprite);
    }

    public GameObject GetEquip(int _idx)
    {
        ItemData data = listJsonItem.Find(x => x.idx == _idx);
        
        if (data == null || data.nameType != "Equip")
        {
            Debug.Log("올바른 오브젝트가 아닙니다.");
            return null;
        }

        return listObjEquip.Find(x => x.name == data.nameObject);
    }

    /// <summary>
    /// 장비 교체 함수
    /// </summary>
    public int P_EquipChange(int _idx)
    {
        ItemData dempData = equipSlot.P_GetItemData();
        int dempIdx = dempData.idx;
        equipSlot.P_SetItemData(_idx);
        return dempIdx;
    }
}
