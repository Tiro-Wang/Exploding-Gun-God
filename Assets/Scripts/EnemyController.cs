using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float enemyHp = 100f;
    [SerializeField] private Animator animator;
    public void TakeDamage(int damageAmount)
    {
        enemyHp -= damageAmount;
        if (enemyHp <= 0)
        {
            enemyHp = 0;
            animator.SetTrigger("death");
            gameObject.GetComponent<CapsuleCollider>().enabled=false;
        }
        else
        {
            animator.SetTrigger("damage");
        }
    }

}
