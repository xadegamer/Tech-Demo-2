using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MultipleCallbackOnHit : MonoBehaviour
{
	public enum HitShape {Sphere, Box}

	public delegate void TriggerCB(Collider[] other);

	[SerializeField] private Color color;
	[SerializeField] private float timer = 0.1f;
	[SerializeField] private LayerMask targetLayer;
	[SerializeField] private HitShape hitShape;

	[HideIf("hitShape", HitShape.Box)]
	[BoxGroup("Sphere")]
	[SerializeField] private float attackRange = 1f;

	[HideIf("hitShape", HitShape.Sphere)]
	[BoxGroup("Box")]
	[SerializeField] private Vector3 size;
	[HideIf("hitShape", HitShape.Sphere)]
	[BoxGroup("Box")]
	[SerializeField] private Vector3 Offset;

	public Transform vfxTransfrom;

	TriggerCB Callback;
	bool attacked = false;
	float startTime;

	private void OnEnable()
    {
		startTime = Time.time;

		if (!attacked)
		{
			attacked = true;
			Attack();
		}
	}

	private void Update()
	{
		if (Time.time > startTime + timer)
		{
			attacked = false;
			gameObject.SetActive(false);
		} 
	}

	public void Setup(TriggerCB _callback)
	{
		Callback = _callback;
	}

	public void Attack()
	{
		if (hitShape == HitShape.Sphere)
		{
			Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange, targetLayer);
			if (colliders.Length != 0)
			{
				Callback(colliders);
			}
		}
		else if (hitShape == HitShape.Box)
        {
			Vector3 colliderPos = transform.position +  Offset;

			Collider[] colliders = Physics.OverlapBox(colliderPos, size / 2, transform.localRotation, targetLayer);

			if (colliders.Length != 0)
			{
				Callback(colliders);
			}
		}
	}



#if UNITY_EDITOR
	public virtual void OnDrawGizmos()
	{
		if (hitShape == HitShape.Sphere)
		{
			Gizmos.color = color;
			Gizmos.DrawWireSphere(transform.position, attackRange);
		}
		else if (hitShape == HitShape.Box)
		{
			Gizmos.color = color;
			Gizmos.matrix = transform.localToWorldMatrix;
			Vector3 pos = Offset;
			Gizmos.DrawWireCube(pos, size);
		}
	}
#endif
}
