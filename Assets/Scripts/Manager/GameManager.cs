using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private InventoryManager inventoryManager;

    [SerializeField] private PlayerMove playerMove; //�÷��̾� ������ ����
    [SerializeField] private PlayerAttack playerAttack; //�÷��̾� ���� ����
    public PlayerAttack PlayerAttack { get { return playerAttack; } }
    [SerializeField] private GameObject inventory; //�κ��丮 ������Ʈ
    [SerializeField] private Button inventoryExitButton; //�κ��丮 ������ ��ư
    [SerializeField,Tooltip("���϶� True, �Ⱥ��� �� False")] private bool isCursor; //Ŀ�� ��� ����
    [SerializeField] private bool isPause; //���� ���� ����

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
        Cursor.lockState = CursorLockMode.Locked; //���� ���� �� ���콺 Ŀ�� ���
        inventory.SetActive(false);
    }

    private void Update()
    {
        PauseGame();
        SetCursor();
        InputInventory();
    }

    /// <summary>
    /// Ư�� ����� ����ϱ� ���� ���� ���
    /// </summary>
    private void PauseGame()
    {
        switch (isPause)
        {
            case false:
                Time.timeScale = 1f; //���� ����
                playerMove.P_SetIsMoving(true); //���� ���� ����
                playerAttack.P_SetIsAttacking(true); //���� ���� ����
                break;

            case true:
                Time.timeScale = 0f; //���� ����
                playerMove.P_SetIsMoving(false); //���� ����
                playerAttack.P_SetIsAttacking(false); //���� ����
                break;
        }
    }

    /// <summary>
    /// ���콺 Ŀ�� ��ȯ
    /// </summary>
    private void SetCursor()
    {
        if (isCursor) //true�� �� ���̰� �ϱ�
        {
            Cursor.lockState = CursorLockMode.None;
        }

        else //false�� �� ���� �ϱ�
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
    /// �κ��丮 ����
    /// </summary>
    private void InputInventory()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            inventory.SetActive(!inventory.activeSelf); //�κ��丮 ���¿� ���� ����
            isCursor = inventory.activeSelf; //�κ��丮�� ���� �� ���콺 Ŀ�� ��
            isPause = inventory.activeSelf;

            if (!inventory.activeSelf) //�κ��丮 �� ��
            {
                //for������ Ȱ��ȭ �Ǿ� �ִ� ������ Ž���Ͽ� ���� �����
                inventoryManager.P_CheckSlots();
            }
        }

        //�κ��丮�� �����ִ� ���¿��� Esc��ư�� ���� ��� ����(���߿� UI ������ �� ���� �ʼ�)
        if (Input.GetKeyDown(KeyCode.Escape) && inventory.activeSelf)
        {
            inventory.SetActive(false);
            isCursor = false; 
            isPause = false;
        }

        inventoryExitButton.onClick.RemoveAllListeners(); //���ٽĿ� ���� �� �˾ƺ���

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
#region ���� ���� ȭ�� ���� ���� ����
    //private void SetReSolution()
    //{
    //    float targetRatio = 9f / 16f; //fhd
    //    float ratio = (float)Screen.width / (float)Screen.height;
    //    float scaleHeight = ratio / targetRatio; //����
    //    float fixedWidth = (float)Screen.width / scaleHeight;
        
    //    Screen.SetResolution((int)fixedWidth, Screen.height, true);
    //}
#endregion
