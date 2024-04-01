using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    [SerializeField] private float pickUpRange; //아이템을 획득할 수 있는 거리
    [SerializeField] private float shortDistance; //제일 가까운 아이템의 거리
    [SerializeField] private KeyCode pickUpKeyCode; //획득 키
    [SerializeField] private string pickUpText; //획득 텍스트
    [SerializeField] private GameObject objPickUpText; //획득 텍스트 오브젝트

    [SerializeField] private Transform targetItem;

    private void Update()
    {
        SearchItem();
        GetItemUI();
    }

    /// <summary>
    /// 드랍된 아이템을 찾는 코드
    /// 적을 찾아내는 코드와 같음
    /// </summary>
    private void SearchItem()
    {
        //아이템 태그있는 오브젝트 찾기
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");

        shortDistance = Mathf.Infinity; //최대 수치로 저장

        Transform nearItem = null; //제일 근처에 있는 아이템을 담을 변수

        foreach (GameObject item in items)
        {
            //아이템과의 거리 측정
            float distance = Vector3.Distance(item.transform.position, transform.position);

            if (distance < shortDistance) //측정한 거리가 제일 가까운 아이템과의 거리보다 짧으면
            {
                shortDistance = distance; //최소 거리 재설정

                nearItem = item.transform;
            }

            if (nearItem != null) //근처에 아이템이 존재할 경우
            {
                targetItem = nearItem; //타겟 아이템 설정
            }

            else //존재하지 않을 경우
            {
                targetItem = null; //변수 비우기
            }
        }
    }

    private void GetItemUI()
    {
        if (targetItem != null)
        {
            if (shortDistance <= pickUpRange) //제일 가까운 아이템이 픽업 가능 영역안에 있을 경우
            {
                objPickUpText.SetActive(true); //아이템 텍스트 오브젝트 활성화
            }

            else
            {
                objPickUpText.SetActive(false);
            }
        }
    }
}
