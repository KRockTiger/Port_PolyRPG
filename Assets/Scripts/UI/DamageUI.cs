using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUI : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private float moveSpeed; //움직이는 속도
    [SerializeField] private float setDestroyTime; //설정할 파괴 시간
    [SerializeField] private float remainDestroyTime; //남은 파괴 시간
    private float damage; //표시할 데미지 값

    private void Awake()
    {
        cam = Camera.main;
        remainDestroyTime = setDestroyTime;
    }

    private void Update()
    {
        CheckUIPosition();
        CheckUIRotation();
        OverTimeToDestroy();
    }

    /// <summary>
    /// 오브젝트 위치 변환
    /// </summary>
    private void CheckUIPosition()
    {
        Vector3 upVector = Vector3.up * moveSpeed * Time.deltaTime;
        transform.position += upVector;
    }

    /// <summary>
    /// 오브젝트 회전 변환
    /// </summary>
    private void CheckUIRotation()
    {
        transform.forward = cam.transform.forward;
    }

    /// <summary>
    /// 일정 시간이 지나면 오브젝트 파괴
    /// </summary>
    private void OverTimeToDestroy()
    {
        remainDestroyTime -= Time.deltaTime;

        if (remainDestroyTime <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
