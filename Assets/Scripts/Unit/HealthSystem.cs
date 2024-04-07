using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField]private int healthMax = 100;
    public event EventHandler OnDeath;
    public static event EventHandler OnAnyDeath;
    public event EventHandler OnDamaged;
    private void Awake()
    {
        health = healthMax;
    }
    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        OnDamaged?.Invoke(this,EventArgs.Empty);
        if (health < 0)
        {
            health = 0;
            
        }
        if (health == 0)
        {
            Die();
        }
        
    }
    private void Die()
    {
        OnDeath?.Invoke(this, EventArgs.Empty);
        OnAnyDeath?.Invoke(this, EventArgs.Empty);
    }
    public float GetHPNormalized()
    {
        return (float)health / healthMax;
    }
    public int GetHealth()
    {
        return health;
    }
    public int GetHealthMax()
    {
        return healthMax;
    }
}
