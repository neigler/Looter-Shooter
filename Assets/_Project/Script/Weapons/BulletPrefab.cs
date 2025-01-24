using System;
using UnityEngine;

public class BulletPrefab : MonoBehaviour
{
    [Header("Destroy Layermasks")]
    [SerializeField] private string wall = "Obstacle";

    [Header("Enemy Layermasks")]
    [SerializeField] private String enemy = "Enemy";

    [Header("Particles")]
    [SerializeField] private GameObject enemyHitParticle;
    [SerializeField] private GameObject wallHitParticle;
    [SerializeField] private GameObject footStepParticle;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag(wall))
        {
            AudioManager.PlaySound(SoundType.BULLETHITWALL);
            Instantiate(wallHitParticle, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }

        if (col.gameObject.CompareTag(enemy))
        {
            col.gameObject.GetComponent<EnemyStats>().currentHealth -= GameObject.Find("Weapon Controller").GetComponent<WeaponScript>().currentWeapon.damage;
            col.gameObject.GetComponent<EnemyAI>().state = State.Pursuing;
            AudioManager.PlaySound(SoundType.BULLETHITENEMY);
            Instantiate(enemyHitParticle, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
