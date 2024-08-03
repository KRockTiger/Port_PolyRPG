using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerAttack PlayerAttack { get { return playerAttack; } }

    [SerializeField] private bool isTest; //테스트를 위해 메인메뉴 입력키 막기

    [SerializeField] private InventoryManager inventoryManager;

    [Header("UI오브젝트")]
    [SerializeField] private GameObject inventory; //인벤토리 오브젝트
    [SerializeField] private GameObject mainMenu; //메인메뉴
    
    [SerializeField] private Button inventoryExitButton; //인벤토리 나가기 버튼
    [SerializeField] private Button continueButton; //이어하기 버튼
    [SerializeField] private Button retryButton; //다시하기 버튼
    [SerializeField] private Button gameExitButton; //게임 나가기 버튼
    [SerializeField] private PlayerMove playerMove; //플레이어 움직임 제어
    [SerializeField] private PlayerAttack playerAttack; //플레이어 공격 제어
    [SerializeField, Tooltip("보일땐 True, 안보일 땐 False")] private bool isCursor; //커서 사용 유무
    [SerializeField] private bool isInventory; //인벤토리 활성화 유무
    [SerializeField] private bool isMainMenu; //메인 메뉴 활성화 유무
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
        mainMenu.SetActive(false);
    }

    private void Update()
    {
        PauseGame();
        SetCursor();
        InputMainMenu();
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
            isInventory = inventory.activeSelf;

            if (!inventory.activeSelf) //인벤토리 끌 때
            {
                //for문으로 활성화 되어 있는 슬롯을 탐색하여 끄게 만들기
                inventoryManager.P_CheckSlots();
            }
        }

        //인벤토리가 켜져있는 상태에서 Esc버튼을 누를 경우 끄기(나중에 UI 설계할 때 수정 필수)
        if (Input.GetKeyDown(KeyCode.Escape) && isInventory)
        {
            inventory.SetActive(false);
            isInventory = false;
            isCursor = false; 
            isPause = false;
            return; //혹여 인벤토리 꺼진 후 바로 메인 메뉴 오픈 판정이 될 수 있기 때문에 예외 처리
        }

        inventoryExitButton.onClick.RemoveAllListeners(); //람다식에 대해 더 알아보기

        inventoryExitButton.onClick.AddListener(() =>
        {
            B_ExitInventory();
        });
    }

    /// <summary>
    /// 메인 메뉴 조작
    /// </summary>
    private void InputMainMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isInventory && !isTest) //인벤토리가 켜져있을 경우 인벤토리 먼저 조작
        {
            mainMenu.SetActive(!mainMenu.activeSelf);
            isMainMenu = mainMenu.activeSelf;
            isCursor = mainMenu.activeSelf;
            isPause = mainMenu.activeSelf;
        }

        continueButton.onClick.AddListener(() =>
        {
            B_ContinueButton();
        });
    }

    private void B_ExitInventory()
    {
        inventory.SetActive(false);
        isCursor = false;
        isPause = false;
    }

    private void B_ContinueButton()
    {
        mainMenu.SetActive(false);
        isCursor = false;
        isPause = false;
    }

    private void B_RetryButton()
    {
        Debug.Log("다시 하기");
    }

    private void B_GameExitButton()
    {
        Debug.Log("게임 종료");
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
