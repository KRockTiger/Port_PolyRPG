using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;

    [Header("캐릭터 무빙 설정")]
    [SerializeField] private float moveSpeed;

    private void Awake()
    {
        controller = GetComponent<CharacterController>(); //캐릭터 컨트롤러 접근
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Moving();
    }

    /// <summary>
    /// 캐릭터 움직임을 담당
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
