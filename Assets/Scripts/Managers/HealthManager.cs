using System;
using UnityEngine;

public class HealthManager : MonoBehaviour, IHealth
{
    [SerializeField] int maxHealthAmount = 20;
    [SerializeField] int currentHealth;

    void Start()
    {
        currentHealth = maxHealthAmount;
    }

    public void Damage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
            HandleDeath();
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth >= maxHealthAmount)
            currentHealth = maxHealthAmount;
    }

    void HandleDeath()
    {
        Debug.Log($"{name} has died.");
        Destroy(gameObject);
    }
}
