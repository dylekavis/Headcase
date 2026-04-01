using System;
using UnityEngine;

public class HealthManager : MonoBehaviour, IHealth
{
    public event Action<int> OnDamageTaken;
    
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

        OnDamageTaken?.Invoke(damageAmount);

        HitStopManager.Instance.Stop(0.1f);
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
    }
}
