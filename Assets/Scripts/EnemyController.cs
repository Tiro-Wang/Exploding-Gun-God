using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float enemyHp = 100f;
    [SerializeField] private Animator animator;

    [SerializeField] private Transform projectile;
    [SerializeField] private Transform projectilePoint;
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
    public void Shoot()
    {
        Rigidbody rb = Instantiate(projectile, projectilePoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        //给子弹施加力
        rb.AddForce(gameObject.transform.forward * 30f,ForceMode.Impulse);
        rb.AddForce(gameObject.transform.up * 7f,ForceMode.Impulse);
    }
}
