using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculate : MonoBehaviour
{
    [Header("������ ��� ������ ����")]
    [SerializeField, Tooltip("���� ���ݷ��� �ƴ� ��뿡�� �޴� ���ݷ�")] float attackPoint; //���ݷ�
    [SerializeField, Tooltip("���� ����")] float defendPoint; //����
    [SerializeField, Tooltip("���Ƿ� ������ �����(���� ������)")] float defendConstant; //�����

    [Header("����� ���� ������(Ȯ�ο����� ����X)")]
    [SerializeField, Tooltip("���°� ������� ���� ���(Ȯ�ο�)")] float defendPercent; //�����
    [SerializeField, Tooltip("�� ��ġ��� ���Ͽ� �޴� ������(Ȯ�ο�)")] float damage; //������

    [Header("�� ������ ���Ŀ� ����� ����")]
    [SerializeField] float resistPoint; //���׷�
    [SerializeField] float piercePoint; //�����
    [SerializeField, Range(0, 1)] float piercePercent; //�����
    [SerializeField] float power; //���ݷ�

    [Header("�� ������ ��� Ȯ��")]
    [SerializeField] float answerDamage;

    private void Update()
    {
        //�⺻���� ������ ��� ����
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //�����(���������) = ���� / (����� + �����)
            defendPercent = defendPoint / (defendPoint + defendConstant);

            //������ = ���ݷ� * (1 - �����)
            damage = attackPoint * (1 - defendPercent);

            Debug.Log($"����� ����� : {defendPercent}");
            Debug.Log($"����� ������ : {damage}");
        }

        //�� ������ ��� ����
        //���ط� = ĳ���� hp ���ҷ� = {100/(100 + ���׷� - ������) * ����}
        //����� => (���׷� * �����(%) + �����(����))�� ���
        if (Input.GetKeyDown(KeyCode.R))
        {
            answerDamage = 100 / (100 + resistPoint - (resistPoint * piercePercent + piercePoint)) * power;
            Debug.Log($"������ : {answerDamage}");
        }
    }
}
