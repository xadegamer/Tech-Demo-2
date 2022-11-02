using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDamager : Damager
{
	[SerializeField] Color color;
	[SerializeField] float attackRange = 1f;

	[Header("Constant Attack")]
	[SerializeField] float constantDamageDelay;
	[SerializeField] bool constantAttack = false;
	[SerializeField] bool constantKnockBackAttack = false;
	float timer;

    private void OnEnable()
    {
		timer = 0;
	}

    private void Update()
	{
		if(constantAttack || constantKnockBackAttack)
        {
			if (timer <= 0)
			{
				if (constantAttack) Attack(false);
				if (constantKnockBackAttack) Attack(true);
				timer = constantDamageDelay;
			}
			else timer -= Time.deltaTime;
		}
	}

	public override void Attack(bool knockBack)
	{
		Collider[] colInfo = Physics.OverlapSphere(transform.position, attackRange, targetLayer);
		if (colInfo.Length != 0)
		{
			hasHit = true;

			foreach (Collider collider in colInfo)
			{
				if (collider.TryGetComponent(out Damageable damagableTarget))
				{
					int currentCriticalDamage = RandomCriticalDamage();

					if (ragedDamage) currentCriticalDamage = (int)(damage * damageMultiplier) - damage;

					damageInfo.damageAmount = damage + currentCriticalDamage;
					damageInfo.direction = transform.forward;

					hasHit = damagableTarget.TakeDamage(damageInfo);
				}
			}
		}
		else hasHit = false;

		if (hasHit) OnHit?.Invoke();
	}

	public override void ToggleConstantAttack(bool newState) { constantAttack = newState; }
	public override void ToggleConstantKockBackAttack(bool newState) { constantKnockBackAttack = newState; }

	void OnDrawGizmos()
	{
		Gizmos.color = color;
		Gizmos.DrawWireSphere(transform.position, attackRange);
	}
}
