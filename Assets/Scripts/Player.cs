using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;

    [Header("ĳ���� ���� ����")]
    [SerializeField] private float moveSpeed;

    private void Awake()
    {
        controller = GetComponent<CharacterController>(); //ĳ���� ��Ʈ�ѷ� ����
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Moving();
    }

    /// <summary>
    /// ĳ���� �������� ���
    /// </summary>
    private void Moving()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        transform.position += new Vector3(moveX, 0f, moveZ) * Time.deltaTime * moveSpeed;
        animator.SetInteger("RunZ", (int)moveZ);
        animator.SetInteger("RunX", (int)moveX);
    }
}
