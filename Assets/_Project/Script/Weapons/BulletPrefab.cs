using System;
using UnityEngine;

public class BulletPrefab : MonoBehaviour
{
    [Header("Destroy Layermasks")]
    [SerializeField] private string wall = "Obstacle";

    [Header("Enemy Layermasks")]
    [SerializeField] private String enemy = "Enemy";

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag(wall))
        {
            Destroy(this.gameObject);
        }
    }
}
