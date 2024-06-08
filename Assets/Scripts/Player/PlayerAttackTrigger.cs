using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackTrigger : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Enemy sc = other.GetComponent<Enemy>();
            sc.P_Hit(playerStats.P_GetAttackPoint());
        }
    }
}
