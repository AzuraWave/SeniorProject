using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{
    public string Name { get; set; }
    public int HealthDamage { get; set; }
    public int PostureDamage { get; set; }
    public float KnockbackForce { get; set; }
    public float AttackRange { get; set; }
    public WeaponType WeaponType { get; set; }

    public Attack(string name, int healthDamage, int postureDamage,  WeaponType weaponType, float attackRange, float knockbackForce = 0 )
    {
        Name = name;
        HealthDamage = healthDamage;
        PostureDamage = postureDamage;
        KnockbackForce = knockbackForce;
        AttackRange = attackRange;
        WeaponType = weaponType;
    }
}

// Enum for weapon types
public enum WeaponType
{
    Melee,
    Ranged
}
