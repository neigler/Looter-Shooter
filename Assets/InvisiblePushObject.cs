using UnityEngine;
using System;


public class InvisiblePushObject : MonoBehaviour
{
    [SerializeField] private String enemy = "Enemy";
    [SerializeField] private GameObject enemyHitParticle;
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag(enemy))
        {
            Instantiate(enemyHitParticle, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
