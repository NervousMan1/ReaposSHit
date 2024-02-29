using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

public class Explosion : Health
{
    Collider2D[] inExplosionRadius = null;
    [SerializeField] private int explosionDamage = 2;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private GameObject explosionEffect;


    public void Explode()
    {
        Instantiate(explosionEffect, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1f), Quaternion.identity );
        inExplosionRadius = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D col in inExplosionRadius)
        {
            if(col.gameObject.TryGetComponent(out Health damaged))
            damaged.TakeDamage(explosionDamage);
        }
        Destroy(gameObject);
    }

    protected override void Kill()
    {
        base.Kill();
        Explode();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
