using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyAnimation enemyAnimation;
    private CharacterController controller;

    [Header("���� Ȯ��")]
    [SerializeField, Tooltip("���� ������ ��� True�� ǥ��")] private bool isBoss;

    [Header("�⺻ ����")]
    private float turnSpeed; //ȸ�� �ӵ��� ���� ����
    private float gravity = -9.81f; //�߷°�
    private Vector3 velocity; //�߷� ���� ����
    [SerializeField] private bool isChase; //���� ����
    [SerializeField] private bool isGround; //���� ����
    [SerializeField] private bool isHitting; //�ǰ� ���� ==> �ǰ� ���� ��� �ణ�� �׷α� �ð��� ����
    [SerializeField] private bool isAttacking; //���� �� ����
    [SerializeField] private bool attackAble; //���� ���� ����
    [SerializeField] private bool isMoving; //�̵� ����
    [SerializeField] private Transform groundChecker; //���� ���� ��Ҵ� ���� �Ǵ��� Transform
    [SerializeField] private float groundDistance; //�߰� ������ �Ÿ�
    [SerializeField] private LayerMask groundMask; //�ٴ������� ���� ���̾��ũ
    [SerializeField] private GameObject prfHPImage; //ü�¹� �̹��� ������
    [SerializeField] private GameObject objHPImage; //ü�¹� �̹��� ������Ʈ

    [Header("ĳ���� ����")]
    [SerializeField] private float setHP; //������ ü��
    [SerializeField] private float curHP; //���� ü��
    [SerializeField] private float moveSpeed; //�̵� �ӵ�
    [SerializeField] private float startSearchRange; //Ž�� ����
    [SerializeField] private float endSearchRange; //Ž�� �ߴ� ����
    [SerializeField] private float attackRange; //���� ����
    [SerializeField] private float setAttackTimer; //������ ���� Ÿ�̸�
    [SerializeField] private float curAttackTimer; //���� ���� Ÿ�̸�
    [SerializeField] private bool goAttack; //���� �߻� ���� ����
    [SerializeField] private bool isTimer; //Ÿ�̸� ������

    [Header("ü�¹� ����")]
    [SerializeField, Range(0, 5)] private float objHPUIPositionY; //ü�¹� ��ġ�� ���ϱ� ���� �ѱ� ��

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Attack")
        {
            Hit();
            enemyAnimation.P_SetTrigger_Hit();
        }
    }

    private void Awake()
    {
        enemyAnimation = GetComponent<EnemyAnimation>();
        controller = GetComponent<CharacterController>();
        curHP = setHP; //���� ü�� ����
    }

    private void Start()
    {
        goAttack = true;
        objHPImage = Instantiate(prfHPImage, gameObject.transform); //�� ������Ʈ���� �ڽ����� ����
        //objHPImage.SetActive(false); //ü�¹� ��Ȱ��ȭ
    }

    private void Update()
    {
        Gravity();
        SearchPlayer();
        Attacking();
        HPPassToUI();
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
        if (isHitting || isAttacking) { return; } //�ǰ��� Ȥ�� �������� ��� ����

        GameObject player = GameObject.FindGameObjectWithTag("Player"); //�÷��̾� Ž��
        
        //�÷��̾���� �Ÿ� ����
        float distance = Vector3.Distance(player.transform.position, transform.position);
    
        if (distance <= startSearchRange) //������ Ž�� �������� �Ÿ��� �����ٸ�
        {
            isChase = true; //���� ���� Ű��
            objHPImage.SetActive(true); //ü�¹� Ȱ��ȭ

            if (distance <= attackRange) //���� �������� �����ٸ�
            {
                attackAble = true; //���� ���� ��ȯ
            }

            else //���� �������� �ָ�
            {
                attackAble = false; //������ ���� ��ȯ
            }
        }

        else if (distance >= endSearchRange) //������ Ž�� �ߴ� ���� ���� �Ÿ��� �ָ�
        {
            isChase = false; //���� ���� ����
            objHPImage.SetActive(false); //ü�¹� ��Ȱ��ȭ
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
        if (!isChase) { return; } //���� ���°� �ƴҰ�� ����

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

    /// <summary>
    /// ���� ü�� ������ ������ UI������Ʈ�� �Ѱܼ� ǥ��
    /// </summary>
    private void HPPassToUI()
    {
        EnemyHP sc = objHPImage.GetComponent<EnemyHP>();
        sc.P_CurrectEnemyInformation(curHP, setHP, objHPUIPositionY, transform);
    }

    /// <summary>
    /// ���� ���·� ��ȯ
    /// ���� �������� ��Ÿ�� ������ �Ϲ����� ���� �ڵ�
    /// </summary>
    private void Attacking()
    {
        //if (isTimer) //Ÿ�̸Ӱ� �۵� ������ �����ϰ��
        //{
        //    curAttackTimer -= Time.deltaTime; //���� ��Ÿ���� ��� �پ��
        //}

        //if (curAttackTimer <= 0f)
        //{
        //    goAttack = true; //���� �߻� ���·� ��ȯ
        //    isTimer = false; //Ÿ�̸� ����
        //}

        if (!attackAble) { return; }

        if (goAttack)
        {
            StartCoroutine(C_SetAttackTimer());
            enemyAnimation.P_GoAttack();
        }
    }

    /// <summary>
    /// �ǰ� ����
    /// </summary>
    private void Hit()
    {
        Debug.Log("�¾ҽ��ϴ�!");
        isAttacking = false; //���� ĵ�� ������ �޾ƾ� ��

        if (isBoss) { return; } //���� ������ ��� ����
        StartCoroutine(C_SetIsHitting());
    }

    /// <summary>
    /// �ǰ� �� 1�ʵ��� �׷α� ���¸� ����
    /// </summary>
    /// <returns></returns>
    private IEnumerator C_SetIsHitting()
    {
        isHitting = true;
        yield return new WaitForSeconds(1f);
        isHitting = false;
    }

    private IEnumerator C_SetAttackTimer()
    {
        goAttack = false; //���� �߻� �Ұ��� ����
        yield return new WaitForSeconds(setAttackTimer);
        goAttack = true; //���� �߻� ���� ����
    }

    public void P_SetIsAttacking(bool _isAttacking)
    {
        isAttacking = _isAttacking;
    }

    public bool P_GetIsAttacking()
    {
        return isAttacking;
    }

    public bool P_GetIsChase()
    {
        return isChase;
    }

    public bool P_GetAttackAble()
    {
        return attackAble;
    }
}
