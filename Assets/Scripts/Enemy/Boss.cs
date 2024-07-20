using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private BossAnimation bossAnimation;
    private CharacterController controller;

    [SerializeField] private bool isTest = false; //�׽�Ʈ ����

    [Header("�⺻ ����")]
    private float turnSpeed; //ȸ�� �ӵ��� ���� ����
    private float gravity = -9.81f; //�߷°�
    private Vector3 velocity; //�߷� ���� ����
    [SerializeField] private bool isCombo; //�޺� ���� ����
    [SerializeField] private bool isChase; //���� ����
    [SerializeField] private bool isGround; //�� üũ
    [SerializeField] private bool isHitting; //�ǰ� ���� ==> �ǰ� ���� ��� �ణ�� �׷α� �ð��� ����
    [SerializeField] private bool isAttacking; //���� �� ����
    [SerializeField] private bool attackAble; //���� ���� ����
    [SerializeField] private bool isMoving; //�̵� ����
    [SerializeField] private Transform groundChecker; //���� ���� ��Ҵ� ���� �Ǵ��� Transform
    [SerializeField] private float groundDistance; //�߰� ������ �Ÿ�
    [SerializeField] private LayerMask groundMask; //�ٴ������� ���� ���̾��ũ

    [Header("������Ʈ ����")]
    [SerializeField] private GameObject prfHPImage; //ü�¹� �̹��� ������
    [SerializeField] private GameObject objHPImage; //ü�¹� �̹��� ������Ʈ
    [SerializeField] private GameObject objDamageUI; //������ UI ������
    [SerializeField] private GameObject objChopTrigger; //��� Ʈ���� ������
    [SerializeField] private BoxCollider attackTrigger; //���� Ʈ����
    [SerializeField] private MeshCollider weaponCollider; //���� �޽� �ݸ���
    [SerializeField] private BoxCollider weaponBoxCollider; //���� �ڽ� �ݸ���
    [SerializeField] private bool useWeaponBoxCollider = false; //���� �ݸ��� �� �ڽ� �ݸ��� ��� ����

    [Header("������Ʈ ��ġ ����")]
    [SerializeField, Range(0, 5)] private float objHPUIPositionY; //ü�¹� ��ġ�� ���ϱ� ���� �ѱ� ��
    [SerializeField, Range(0, 5)] private float objDamageUIPositionY; //������ ������Ʈ ��ġ�� ���ϱ� ���� �ѱ� ��
    [SerializeField] private Transform trsChopTrigger; //��� Ʈ���� ��ġ ������Ʈ

    [Header("ĳ���� ����")]
    [SerializeField] private float setHP; //������ ü��
    [SerializeField] private float curHP; //���� ü��
    [SerializeField] private float attackPoint; //���ݷ�
    [SerializeField] private float defendPoint; //����
    [SerializeField] private float moveSpeed; //�̵� �ӵ�
    [SerializeField] private float startSearchRange; //Ž�� ����
    [SerializeField] private float endSearchRange; //Ž�� �ߴ� ����
    [SerializeField] private float attackRange; //���� ����
    [SerializeField] private float setAttackTimer; //������ ���� Ÿ�̸�
    [SerializeField] private float curAttackTimer; //���� ���� Ÿ�̸�
    [SerializeField] private bool goAttack; //���� �߻� ���� ����
    [SerializeField] private bool isTimer; //Ÿ�̸� ������
    [SerializeField] private bool isDie; //ĳ���� ��� ó��

    [Header("���� ���� ����")]
    [SerializeField] private List<BossPattern> patterns;

    private WaitForSeconds waitTime = new WaitForSeconds(0.5f); //���� ��� �ð�
    private WaitForSeconds setTriggerTime = new WaitForSeconds(0.1f); //Ʈ���� Ȱ��ȭ �ð�

    private void Awake()
    {
        bossAnimation = GetComponent<BossAnimation>();
        controller = GetComponent<CharacterController>();
        curHP = setHP; //���� ü�� ����
    }

    private void Start()
    {
        goAttack = true;
        //objHPImage = Instantiate(prfHPImage, gameObject.transform); //�� ������Ʈ���� �ڽ����� ����
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) //���� ��ų �׽�Ʈ�� �ڵ��̹Ƿ� ���� ����
        {
            ComboAttack();
        }

        Gravity();

        if (isTest) { return; }
        SearchPlayer();
    }

    private void Gravity()
    {
        isGround = Physics.CheckSphere(groundChecker.position, groundDistance, groundMask);

        if (isGround)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    /// <summary>
    /// �÷��̾� Ž��
    /// </summary>
    private void SearchPlayer()
    {
        if (isHitting || isAttacking || isDie || isTest) { return; } //�ǰ��� Ȥ�� �������� ��� Ȥ�� ����� ��� ����

        GameObject player = GameObject.FindGameObjectWithTag("Player"); //�÷��̾� Ž��

        if (player == null) { return; }

        //�÷��̾���� �Ÿ� ����
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance <= startSearchRange) //������ Ž�� �������� �Ÿ��� �����ٸ�
        {
            isChase = true; //���� ���� Ű��
            //objHPImage.SetActive(true); //ü�¹� Ȱ��ȭ

            if (distance <= attackRange) //���� �������� �����ٸ�
            {
                attackAble = true; //���� ���� ��ȯ
                //ComboAttack();
            }

            else //���� �������� �ָ�
            {
                attackAble = false; //������ ���� ��ȯ
            }
        }

        else if (distance >= endSearchRange) //������ Ž�� �ߴ� ���� ���� �Ÿ��� �ָ�
        {
            isChase = false; //���� ���� ����
            //objHPImage.SetActive(false); //ü�¹� ��Ȱ��ȭ
            attackAble = false; //������ ���� ��ȯ,
                                //������ ���� �� �������� ����� �� �����Ǵ� ������ ��ħ
        }

        ChasePlayer(player);
    }

    /// <summary>
    /// �÷��̾� ����
    /// </summary>
    private void ChasePlayer(GameObject _objPlayer)
    {
        if (!isChase || isCombo) { return; } //���� ���°� �ƴҰ�� ����

        //���� ����
        Vector3 direction = (_objPlayer.transform.position - transform.position).normalized;

        //������ ������ ������ ȸ���� ����
        Quaternion angle = Quaternion.LookRotation(direction);

        //�� ������ ȸ������ y���� �����ͼ� ȸ���� �ε巴�� �� �� �ֵ��� ����
        float smoothTurnY = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, angle.eulerAngles.y, ref turnSpeed, 0.1f);

        //ȸ�� ����
        transform.rotation = Quaternion.Euler(0f, smoothTurnY, 0f);

        //������Ʈ�� ������ ����
        Vector3 targetDirection = Quaternion.Euler(0f, angle.eulerAngles.y, 0f) * Vector3.forward;

        if (attackAble) { return; } //���� ������ ��� ����

        //������Ʈ �̵�
        controller.Move(targetDirection * moveSpeed * Time.deltaTime);
    }

    private void ComboAttack()
    {
        bossAnimation.P_AnimPlayComboAttack();
    }

    public bool P_GetIsChase()
    {
        return isChase;
    }

    public void P_SetIsCombo(bool _isCombo)
    {
        isCombo = _isCombo;
    }

    /// <summary>
    /// ���� �ݸ����� Ȱ��ȭ�� �����ϴ� public �Լ�
    /// </summary>
    /// <param name="_isCollider"></param>
    public void P_SetWeaponCollider(bool _isCollider)
    {
        if (useWeaponBoxCollider) //���� �ڽ� �ݸ����� ����� ��� ==> �ڽ� �ݸ����� �� Ŀ�� �׷��� ������ �� ����
        {
            weaponBoxCollider.enabled = _isCollider;
        }

        else //�޽� �ݸ����� ����� ���
        {
            weaponCollider.enabled = _isCollider;
        }
    }

    //public IEnumerator PC_SetChopTrigger()
    //{
    //    patterns[0].attackTrigger.SetActive(true);
    //    yield return setTriggerTime;
    //    patterns[0].attackTrigger.SetActive(false);
    //}

    public void PC_SetChopTrigger()
    {
        //Ʈ���� ������ ��ȯ �ڵ� �ۼ�
        GameObject obj = Instantiate(objChopTrigger, trsChopTrigger.transform.position, Quaternion.identity, transform);
    
        Destroy(obj, 0.2f);
    }
}
