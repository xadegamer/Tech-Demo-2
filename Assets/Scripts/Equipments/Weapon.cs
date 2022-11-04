using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IEquipment
{
    [SerializeField] private Animator animator;

    public void Use()
    {
        animator.Play("Attack");
    }
}
