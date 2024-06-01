using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{
    private float curHP;
    private float maxHP;
    private Transform trsEnemy; //�� ������Ʈ�� ���� ������ Transform��
    private Camera cam;
    [SerializeField] private float setYPosition;
    [SerializeField] private Image imgMidHP; //�ǽð����� �پ��� ����� ü��
    [SerializeField] private Image imgFrontHP; //�ٷ� Ȯ���� �� �ִ� ���� ü��

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        CheckUIPosition();
        CheckUIRotation();
        CheckEnemyHP();
    }

    /// <summary>
    /// ü�¹� ��ġ ����
    /// </summary>
    private void CheckUIPosition()
    {
        Vector3 setPosition = new Vector3(0, setYPosition, 0);
        transform.position = trsEnemy.position + setPosition;
    }

    /// <summary>
    /// ü�¹� ȸ�� ����
    /// </summary>
    private void CheckUIRotation()
    {
        //Vector3 direction = cam.transform.position - transform.position;
        //transform.rotation = Quaternion.LookRotation(direction);

        transform.forward = cam.transform.forward; //�� ������Ʈ�� �չ����� ķ ������ �������� ����
    }

    private void CheckEnemyHP()
    {
        imgMidHP.fillAmount = curHP / maxHP;
        imgFrontHP.fillAmount = curHP / maxHP;

        //�� ü�� �̹������� ü�¹ٸ� float�� ġȯ
        float amountMid = imgMidHP.fillAmount;
        float amountFront = imgFrontHP.fillAmount;

        if (amountMid > amountFront)
        {
            imgMidHP.fillAmount -= Time.deltaTime;

            if (imgMidHP.fillAmount <= imgFrontHP.fillAmount)
            {
                imgMidHP.fillAmount = imgFrontHP.fillAmount;
            }
        }
    }

    /// <summary>
    /// Enemy��ũ��Ʈ���� ������ �ʿ��� ���� ������ �� ��ũ��Ʈ�� �ǽð����� �Ѱܼ� UIǥ�� �� ����
    /// (���� ü��, �ִ� ü��)
    /// </summary>
    public void P_CurrectEnemyInformation(float _curHP, float _maxHP, float _positionY, Transform _trsEnemy)
    {
        curHP = _curHP;
        maxHP = _maxHP;
        setYPosition = _positionY;
        trsEnemy = _trsEnemy;
    }
}
