using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private InventoryManager inventoryManager;

    [SerializeField] private PlayerMove playerMove; //플레이어 움직임 제어
    [SerializeField] private PlayerAttack playerAttack; //플레이어 공격 제어
    public PlayerAttack PlayerAttack { get { return playerAttack; } }
    [SerializeField] private GameObject inventory; //인벤토리 오브젝트
    [SerializeField] private Button inventoryExitButton; //인벤토리 나가기 버튼
    [SerializeField,Tooltip("보일땐 True, 안보일 땐 False")] private bool isCursor; //커서 사용 유무
    [SerializeField] private bool isPause; //게임 정지 유무

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
    }

    private void Start()
    {
        inventoryManager = InventoryManager.Instance;
        Cursor.lockState = CursorLockMode.Locked; //게임 시작 시 마우스 커서 잠금
        inventory.SetActive(false);
    }

    private void Update()
    {
        PauseGame();
        SetCursor();
        InputInventory();
    }

    /// <summary>
    /// 특정 기능을 사용하기 위한 포즈 기능
    /// </summary>
    private void PauseGame()
    {
        switch (isPause)
        {
            case false:
                Time.timeScale = 1f; //게임 실행
                playerMove.P_SetIsMoving(true); //무빙 제어 해제
                playerAttack.P_SetIsAttacking(true); //공격 제어 해제
                break;

            case true:
                Time.timeScale = 0f; //게임 정지
                playerMove.P_SetIsMoving(false); //무빙 제어
                playerAttack.P_SetIsAttacking(false); //공격 제어
                break;
        }
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
            isPause = inventory.activeSelf;

            if (!inventory.activeSelf) //인벤토리 끌 때
            {
                //for문으로 활성화 되어 있는 슬롯을 탐색하여 끄게 만들기
                inventoryManager.P_CheckSlots();
            }
        }

        //인벤토리가 켜져있는 상태에서 Esc버튼을 누를 경우 끄기(나중에 UI 설계할 때 수정 필수)
        if (Input.GetKeyDown(KeyCode.Escape) && inventory.activeSelf)
        {
            inventory.SetActive(false);
            isCursor = false; 
            isPause = false;
        }

        inventoryExitButton.onClick.RemoveAllListeners(); //람다식에 대해 더 알아보기

        inventoryExitButton.onClick.AddListener(() =>
        {
            B_ExitInventory();
        });
    }

    private void B_ExitInventory()
    {
        inventory.SetActive(false);
        isCursor = false;
        isPause = false;
    }

    private void Menu()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            
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
