using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class InteractOnTrigger : MonoBehaviour
{
    public int maxTrigger;
    public LayerMask layers;
    public UnityEvent OnEnter, OnExit;
    Collider col;
    int currentTrigger;

    private void OnEnable()
    {
        currentTrigger = 0;
    }

    void Reset()
    {
        layers = LayerMask.NameToLayer("Everything");
        col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (0 != (layers.value & 1 << other.gameObject.layer) && currentTrigger <= maxTrigger)
        {
            if (maxTrigger > 0) currentTrigger++;
            ExecuteOnEnter(other);
        }
    }

    protected virtual void ExecuteOnEnter(Collider other)
    {
        OnEnter.Invoke();
    }

    void OnTriggerExit(Collider other)
    {
        if (0 != (layers.value & 1 << other.gameObject.layer))
        {
            ExecuteOnExit(other);
        }
    }

    protected virtual void ExecuteOnExit(Collider other)
    {
        OnExit.Invoke();
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position + new Vector3(0, 10, 0), "InteractionTrigger", true);
    }
}