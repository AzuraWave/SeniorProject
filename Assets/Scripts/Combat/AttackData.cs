using UnityEngine;

[System.Serializable]
public class AttackData
{
    public bool canParry;
    public bool canBlock;
    public int healthDamage;
    public int postureDamage;
    public float range;
    public float height;
    public float knockbackPower;
    public Vector2 knockbackDirection;

    public AttackData(){
        
    }

    public AttackData(bool canParry, bool canBlock, int healthDamage, int postureDamage, float range, float height, float knockbackPower = 0, Vector2 knockbackDirection = default)
    {
        this.canParry = canParry;
        this.canBlock = canBlock;
        this.healthDamage = healthDamage;
        this.postureDamage = postureDamage;
        this.range = range;
        this.height = height;
        this.knockbackPower = knockbackPower;
        this.knockbackDirection = knockbackDirection;
    }
}
