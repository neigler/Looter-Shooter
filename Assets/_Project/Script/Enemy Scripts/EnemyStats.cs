using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] public int currentHealth;
    private int maxHealth;

    private void Start()
    {
        maxHealth = currentHealth;
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
}
