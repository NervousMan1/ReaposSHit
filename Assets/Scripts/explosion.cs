using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    Collider2D[] inExplosionRadius = null;
    [SerializeField] private int explosionDamage = 2; 
    [SerializeField] private float explosionRadius = 5f;


    public void Explode()
    {
        inExplosionRadius = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D col in inExplosionRadius)
        {
            col.GetComponent<Health>().TakeDamage(explosionDamage);
        }
    }

    private void OnDrawGizmos()
    {
       Gizmos.color = Color.green;
       Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
