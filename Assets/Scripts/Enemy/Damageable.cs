using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;

    [Header("Hits")]
    [SerializeField] bool useHits;
    [SerializeField] int curentHit;
    [SerializeField] int maxHit;

    [Header("CoolDown")]
    [SerializeField] bool hasCooldown;
    [SerializeField] float coolDownDuration;

    [Header("HealthEvents")]
    [SerializeField] HealthEvent[] healthEvents;

    [Header("Debug")]
    [SerializeField] bool isInvulnerable = false;
    [SerializeField] bool damageDelay;

    public event EventHandler OnHealthChanged;
    public event EventHandler<DamageInfo> OnHit;
    public event EventHandler OnHitWhileInvulnerable;
    public event EventHandler OnHeal;
    public event EventHandler OnDied;


    WaitForSeconds coolDownTime;

    private void OnEnable()
    {
        ResetTriggerHealthEvents();

        ResetHits();
    }

    private void Start()
    {
        if (hasCooldown) coolDownTime = new WaitForSeconds(coolDownDuration);
    }

    public void SetHealth(int amount)
    {
        currentHealth = maxHealth = amount;
        if (hasCooldown) coolDownTime = new WaitForSeconds(coolDownDuration);
    }

    public bool IsInvulnerable() => isInvulnerable;
    public int GetHealth() { return currentHealth; }
    public int GetMaxHealth() { return maxHealth; }
    public float GetHealthPercent() { return (float)currentHealth / (float)maxHealth; }

    public bool TakeDamage(DamageInfo damageInfo)
    {
        if (damageDelay) return false;

        if (useHits)
        {
            ManageHit();
            return false;
        }

        if (isInvulnerable) { OnHitWhileInvulnerable?.Invoke(this, EventArgs.Empty); return false;}

        if (currentHealth > 0)
        {
            if (hasCooldown) StartCoroutine(nameof(DamageDelay));

            currentHealth -= damageInfo.damageAmount;

            if (currentHealth > 0)
            {
                TriggerHealthEvents();
                OnHit?.Invoke(this, damageInfo);
            }
            else
            {
                currentHealth = 0;
                OnDied?.Invoke(this, EventArgs.Empty);
            }

            OnHealthChanged?.Invoke(this, EventArgs.Empty);
        }

        return true;
    }

    public void RestoreHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        OnHeal?.Invoke(this, EventArgs.Empty);
    }
    IEnumerator DamageDelay()
    {
        damageDelay = true;
        yield return coolDownTime;
        damageDelay = false;
    }

    int  CalculateDamage(int damage, int percentage, bool increase)
    {
        float percentageValue = ((float)percentage / 100) * damage;
        return increase ? damage += (int)percentageValue : damage -= (int)percentageValue;
    }

    public void Setvulnerability(bool newState) { isInvulnerable = newState;}

    public void TriggerHealthEvents()
    {
        float healthPercentage = useHits ? GetHitPercent() * 100f : GetHealthPercent() * 100f;

        for (int i = 0; i < healthEvents.Length; i++)
        {
            if (healthEvents[i].done) continue;

            if(healthEvents[i].healthPercentage >= healthPercentage)
            {
                healthEvents[i].OnReach?.Invoke();
                healthEvents[i].done = true;
                return;
            }
        }
    }

    public void ResetTriggerHealthEvents()
    {
        for (int i = 0; i < healthEvents.Length; i++) healthEvents[i].done = false;
    }

    public void ManageHit()
    {
        if(curentHit <= 0) return;
        curentHit--;
        TriggerHealthEvents();
        StartCoroutine(nameof(DamageDelay));

        if (curentHit <= 0)
        {
            curentHit = 0;
            OnDied?.Invoke(this, EventArgs.Empty);
        }
        else OnHit?.Invoke(this, null);
    }

    public void ResetHits()
    {
        if (useHits) curentHit = maxHit;
    }

    public float GetHitPercent() => (float)curentHit / (float)maxHit;
}

[System.Serializable]
public class HealthEvent
{
    [Tooltip("Must be between 100 and 10, Decending...")]
    [Range(100, 10)]
    public float healthPercentage;
    public UnityEvent OnReach;
    [HideInInspector]public bool done;
}

public class DamageInfo
{
    public int damageAmount;
    public Vector3 direction;
}