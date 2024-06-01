using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{
    private float curHP;
    private float maxHP;
    private Transform trsEnemy; //현 오브젝트를 가진 몬스터의 Transform값
    private Camera cam;
    [SerializeField] private float setYPosition;
    [SerializeField] private Image imgMidHP; //실시간으로 줄어드는 연출용 체력
    [SerializeField] private Image imgFrontHP; //바로 확인할 수 있는 실제 체력

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
    /// 체력바 위치 조정
    /// </summary>
    private void CheckUIPosition()
    {
        Vector3 setPosition = new Vector3(0, setYPosition, 0);
        transform.position = trsEnemy.position + setPosition;
    }

    /// <summary>
    /// 체력바 회전 조정
    /// </summary>
    private void CheckUIRotation()
    {
        //Vector3 direction = cam.transform.position - transform.position;
        //transform.rotation = Quaternion.LookRotation(direction);

        transform.forward = cam.transform.forward; //현 오브젝트의 앞방향을 캠 방향을 기준으로 설정
    }

    private void CheckEnemyHP()
    {
        imgMidHP.fillAmount = curHP / maxHP;
        imgFrontHP.fillAmount = curHP / maxHP;

        //각 체력 이미지들의 체력바를 float로 치환
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
    /// Enemy스크립트에서 설정된 필요한 몬스터 정보를 현 스크립트에 실시간으로 넘겨서 UI표시 및 저장
    /// (현재 체력, 최대 체력)
    /// </summary>
    public void P_CurrectEnemyInformation(float _curHP, float _maxHP, float _positionY, Transform _trsEnemy)
    {
        curHP = _curHP;
        maxHP = _maxHP;
        setYPosition = _positionY;
        trsEnemy = _trsEnemy;
    }
}
