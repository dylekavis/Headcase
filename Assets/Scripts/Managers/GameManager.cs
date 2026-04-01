using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public event Action<int> OnDamage;
    
    [SerializeField] int maxHealth = 100;
    [SerializeField] int playerHealth;

    void Awake()
    {
        if (Instance != this && Instance != null) { Destroy(gameObject); return; }
        else Instance = this;

        DontDestroyOnLoad(gameObject);

        playerHealth = maxHealth;
    }
    
    public void Damage(int damage)
    {
        playerHealth -= damage;
    }

    public void Heal(int heal)
    {
        playerHealth += heal;
    }

    public int GetHealth()
    {
        return playerHealth;
    }

    public int GetMaxHealth() => maxHealth;
}
