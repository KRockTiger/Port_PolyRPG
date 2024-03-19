using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //���� Ž���ϱ� ���� ������ ����
    private float shortDistance; //���� ����� ������ �Ÿ�
    private Transform targetEnemy; //��ġ�� ���� ������ �ϱ� �������� Transform�� ���

    private PlayerMove playerMove;

    [Header("�÷��̾� ���� ����")]
    [SerializeField] private float attackRange; //���ݹ���

    [SerializeField] private int attackCycle = 0; //���� ���� Ƚ���� ������ ������ 1�� �����ϸ� ������ ������ 0���� ����
    [SerializeField] private GameObject attackTrigger; //���� ������ ���� Ʈ����
    [SerializeField] private bool isAttack; //�÷��̾��� ������ ���
    [SerializeField] private bool isGround; //�÷��̾ ���� �ִ� �� ����
    [SerializeField,Tooltip("���� ������ ������ Ÿ�̹��� �� True")] private bool isNext; //���� ���� �����ϰ� �ϴ� Ʈ����
    [SerializeField,Tooltip("�ٷ� ���� ������ ������ �� True")] private bool nextAttack; //���� ���� ���

    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        attackTrigger.SetActive(false);
    }

    private void Update()
    {
        InputAttack();
        SetIsAttack();
        GetIsGround();
        SearchEnemy();
    }

    /// <summary>
    /// �÷��̾��� ������ ����ϴ� �Լ�
    /// </summary>
    private void InputAttack()
    {
        //�÷��̾ ���� ����ִ� ���¿��� ���� ���� ���°� �ƴ� �� ���� ==> ���߿� ���� ������ �ְ� �Ǹ� ���� ����
        if (Input.GetKeyDown(KeyCode.Mouse0) && isGround)
        {
            //��ǥ ���Ͱ� ��ó�� ���� ���  �ٷ� ȸ���Ͽ� ����
            if (targetEnemy != null) //���� ��ǥ ���Ͱ� �������� ���� ��� ���� ó��
            {
                //��ǥ ���Ϳ� �� ĳ������ �Ÿ��� ���
                float distance = Vector3.Distance(targetEnemy.transform.position, transform.position);

                if (distance < attackRange) //����� �Ÿ��� ���� ���� �ȿ� �� ���
                {
                    LockOn(); //��ǥ ���͸� ���� ȸ��
                }
            }

            if (!isAttack) //���� ���� �ƴ� ���(���� ������ ù ������ ���)
            {
                isAttack = true;
            }

            else if (isAttack && isNext) //���� �� ���� ������ ���������� ��
            {
                nextAttack = true; //���� ���� ����
                Invoke("I_UnNextAttack", 0.1f); //0.1�ʵڿ� �����Ͽ� ��ġ �ʴ� ���� ������ �̸� ������
            }
        }
    }

    /// <summary>
    /// ���� ������ ���� Ʈ���Ÿ� ��������ִ� �Լ�
    /// -�ִϸ��̼ǿ� �ڷ�ƾ�� �־ ����
    /// </summary>
    private IEnumerator AC_AttackTrigger()
    {
        attackTrigger.SetActive(true); //���� Ʈ���� Ȱ��ȭ�Ͽ� �����ϱ�
        yield return new WaitForSeconds(0.1f);
        attackTrigger.SetActive(false); //���� Ʈ���� ��Ȱ��ȭ�Ͽ� ���ݸ���
    }

    /// <summary>
    /// ������ �ִ� ���͸� Ž��
    /// </summary>
    private void SearchEnemy()
    {
        //"Enemy"�±׸� �� ��� ���͸� ��Ƶ�
        //�ٵ� ���� �ִ� ��� ���͸� ��ġ�ϴ� �ڵ�� ���� ���� ���� ������ �ø���
        //��ɿ� ������ ���� �Ŷ� ������
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        shortDistance = Mathf.Infinity; //ó������ ���� ū ���� �־��

        Transform nearEnemy = null; //���� ����� ���� ���� ���� ����

        foreach (GameObject enemy in enemies)
        {
            //��� ������ �Ÿ��� ���Ѵ�.
            float distance = Vector3.Distance(enemy.transform.position, transform.position);

            if (distance < shortDistance) //���� ���� �Ÿ��� ����� ª�� �Ÿ����� ª�� ���
            {
                shortDistance = distance; //���� ����

                nearEnemy = enemy.transform; //���� ����� �� ��ġ ����
            }

            if (nearEnemy != null) //��ó�� ���� ���� ���
            {
                targetEnemy = nearEnemy; //Ÿ�� ����
            }

            else //���� ���� ���
            {
                targetEnemy = null; //Ÿ���� ���
            }
        }
    }

    /// <summary>
    /// �� ĳ���Ͱ� ���͸� ���� ȸ���ϴ� �Լ�
    /// </summary>
    private void LockOn()
    {
        if (targetEnemy != null) //Ÿ���� ���� �ʾ��� ��
        {
            Vector3 direction = targetEnemy.transform.position - transform.position; //���� ����
            Quaternion lookRotate = Quaternion.LookRotation(direction); //Ư�� �������� ���ư��� ���� ����
            //Vector3 look = lookRotate.eulerAngles;

            //�� ���ʹϾ𿡼� ����� y���� eulerAngle�� �����ͼ� ȸ��
            transform.rotation = Quaternion.Euler(0f, lookRotate.eulerAngles.y, 0f);
        }
    }

    /// <summary>
    /// PlayerMove ��ũ��Ʈ�� isGround ���� ��� ������
    /// </summary>
    private void GetIsGround()
    {
        isGround = playerMove.P_GetIsGround();
    }

    /// <summary>
    /// Invoke�� bool���� ���� ���缭 �����Ű��
    /// </summary>
    private void I_UnNextAttack()
    {
        nextAttack = false;
    }

    /// <summary>
    /// �÷��̾� ���ݿ� ���� PlayerMove��ũ��Ʈ�� IsAttacking ���� ����
    /// </summary>
    private void SetIsAttack()
    {
        playerMove.P_SetIsAttacking(isAttack);
    }

    /// <summary>
    /// PlayerAttack�� isAttack ���� �����ϱ�
    /// </summary>
    public void P_SetIsAttack(bool _isAttack)
    {
        isAttack = _isAttack;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_isNext"></param>
    public void P_SetIsNext(bool _isNext)
    {
        isNext = _isNext;
    }

    /// <summary>
    /// ���� Ƚ�� ����
    /// </summary>
    public void P_SetAttackCycle()
    {
        attackCycle += 1;
    }

    /// <summary>
    /// ���� Ƚ�� �ʱ�ȭ
    /// </summary>
    public void P_ReSetAttackCycle()
    {
        attackCycle = 0;
    }

    /// <summary>
    /// PlayerAttack�� isAttack ���� ��������
    /// </summary>
    /// <returns></returns>
    public bool P_GetIsAttack()
    {
        return isAttack;
    }

    public bool P_GetIsNext()
    {
        return isNext;
    }

    public bool P_GetNextAttack()
    {
        return nextAttack;
    }

    public int P_GetAttackCycle()
    {
        return attackCycle;
    }
}
