using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUI : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private float moveSpeed; //�����̴� �ӵ�
    [SerializeField] private float setDestroyTime; //������ �ı� �ð�
    [SerializeField] private float remainDestroyTime; //���� �ı� �ð�
    private float damage; //ǥ���� ������ ��

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
    /// ������Ʈ ��ġ ��ȯ
    /// </summary>
    private void CheckUIPosition()
    {
        Vector3 upVector = Vector3.up * moveSpeed * Time.deltaTime;
        transform.position += upVector;
    }

    /// <summary>
    /// ������Ʈ ȸ�� ��ȯ
    /// </summary>
    private void CheckUIRotation()
    {
        transform.forward = cam.transform.forward;
    }

    /// <summary>
    /// ���� �ð��� ������ ������Ʈ �ı�
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
