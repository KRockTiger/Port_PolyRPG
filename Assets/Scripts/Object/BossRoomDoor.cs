using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomDoor : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private bool isOpen;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        isOpen = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!isOpen) //����������
            {
                isOpen = true;
                animator.Play("OpenDoor");
            }

            else //����������
            {
                isOpen = false;
                animator.Play("ClossDoor");
            }
        }
    }
}
