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

    [SerializeField] private bool isAttack; //�÷��̾��� ������ ���
    [SerializeField] private bool isGround; //�÷��̾ ���� �ִ� �� ����
    [SerializeField] private bool isNext; //���� ���� �����ϰ� �ϴ� Ʈ����
    [SerializeField] private bool nextAttack; //���� ���� ���

    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
    }

    private void Update()
    {
        Attack();
        SetIsAttack();
        GetIsGround();
        SearchEnemy();
    }

    /// <summary>
    /// �÷��̾��� ������ ����ϴ� �Լ�
    /// </summary>
    private void Attack()
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
                nextAttack = true;
            }
        }
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
    /// PlayerAttack�� isAttack ���� ��������
    /// </summary>
    /// <returns></returns>
    public bool P_GetIsAttack()
    {
        return isAttack;
    }

    public bool P_GetNextAttack()
    {
        return nextAttack;
    }
}
