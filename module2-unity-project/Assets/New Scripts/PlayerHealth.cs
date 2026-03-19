using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour, IHittable
{
    public static Action<int> OnHealthChanged;

    public int maxHealth = 5;
    int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth);
    }

    public void Hit(GameObject source)
    {
        currentHealth--;
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Player died");
        }
    }
}