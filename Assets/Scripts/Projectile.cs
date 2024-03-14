using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject impactEffect;
    private PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();//≤È’“Ω≈±æ
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject impact = Instantiate(impactEffect, transform.position, Quaternion.identity);
        Destroy(impact, 2f);
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                StartCoroutine(player.TakeDamage(10));
            }
        }
        Destroy(gameObject,2f);

    }
}
