using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("���ݿ� �¾ҽ��ϴ�.");
            PlayerMove sc = other.GetComponent<PlayerMove>();
            sc.P_SetBounce();
        }
    }
}
