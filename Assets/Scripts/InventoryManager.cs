using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] private DragSlot dragSlot;

    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject itemSlots;
    [SerializeField] private Slot[] slots;

    [SerializeField] private Item addItem; //치트로 추가할 임의의 아이템으로 추후 삭제 예정

    public enum jsonType
    {
        Item,
    }
    [SerializeField] List<TextAsset> listJsons; //json파일 등록
    [SerializeField] List<ItemJson> listJsonItem; //저장된 아이템의 json파일(확인용)

    [SerializeField] List<Sprite> listSpr;

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
        listJsonItem = JsonConvert.DeserializeObject<List<ItemJson>>(json);

        //Sprite spr = P_GetSprite(1);
    }

    private void Start()
    {
        slots = itemSlots.GetComponentsInChildren<Slot>();
        dragSlot = DragSlot.Instance;

        //임의로 아이템 넣기 => json으로 아이템을 잘 넣을 수 있는지 확인 후 삭제
        //P_InputGetItem(0);
        //P_InputGetItem(1);
    }

    private void Update()
    {
        CheckSlots();
    }

    private void CheckSlots()
    {
        //if (Input.GetKeyDown(KeyCode.B) && inventory.activeSelf == true) //인벤토리 오브젝트가 비활성화 상태일 때
        //{
        //    int count = slots.Length; //슬롯 개수 확인

        //    for (int iNum = 0; iNum < count; iNum++)
        //    {
        //        //모든 슬롯의 체크 이미지 오브젝트의 활성화 여부를 확인
        //        if (slots[iNum].P_GetActiveSlot().activeSelf == true)
        //        {
        //            //만약 켜져있으면 false로 비활성화
        //            slots[iNum].P_GetActiveSlot().SetActive(false);
        //        }
        //    }
        //}

        if (dragSlot == null)
        {
            dragSlot = DragSlot.Instance;
        }

        if (Input.GetKeyDown(KeyCode.B) && inventory.activeSelf == false)
        {
            int count = slots.Length; //슬롯 개수 확인

            for (int iNum = 0; iNum < count; iNum++)
            {
                //모든 슬롯의 체크 이미지 오브젝트의 활성화 여부를 확인
                if (slots[iNum].P_GetActiveSlot().activeSelf == true)
                {
                    //만약 켜져있으면 false로 비활성화
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
    }

    /// <summary>
    /// 키를 입력하여 임의의 아이템을 인벤토리에 저장
    /// -치트용 코드이므로 아이템 획득이 잘 작동되면 추후 삭제 예정
    /// </summary>
    public void P_InputGetItem(int _idx)
    {
        int count = slots.Length; //슬롯 개수 확인

        for (int iNum = 0; iNum < count; iNum++)
        {
            //아이템 번호를 이용하여 빈 슬롯 확인
            if (slots[iNum].P_GetItemIdx() == -1) //아이템 번호가 -1이면 빈 슬롯
            {
                slots[iNum].P_AddItem(GetItemJson(_idx), GetSprite(_idx));
                return; //아이템이 추가되면 리턴하여 멈추게 하기
            }
        }
    }

    public string P_GetItemType(int _idx)
    {
        return listJsonItem[_idx].nameType;
    }

    private ItemJson GetItemJson(int _idx)
    {
        ItemJson data = listJsonItem.Find(x => x.idx == _idx);
        
        if (data == null)
        {
            Debug.LogError("존재하지 않은 아이템입니다.");
            return null;
        }
        
        return data;
    }

    private Sprite GetSprite(int _idx)
    {
        ItemJson data = listJsonItem.Find(x => x.idx == _idx);
        if (data == null)
        {
            Debug.LogError($"올바른값이 들어오지 않았습니다.\n idx = {_idx}");
            return null;
        }

        return listSpr.Find(x => x.name == data.nameSprite);
    }
}
