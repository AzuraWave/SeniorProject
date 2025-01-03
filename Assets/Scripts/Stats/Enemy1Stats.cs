using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy1Stats", menuName = "Stats/Enemy1Stats")]
public class Enemy1Stats : ScriptableObject
{
    public string characterName = "Enemy1";
    public int health = 100;
    public int healthMAX = 100;
    public int BasicAttackDamage = 10;
    public int PostureMAX = 100;
    public int PostureRegen = 10;
    public int Posture;
    public int PostureDamage = 20;
    public float speed = 5f;
    public float jumpForce = 5f;
    public float parryWindow = 0.5f;
    public float DetectionRange;

    public void TakeHealthDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, healthMAX);

        if (health <= 0)
        {
            OnDeath();
        }
    }

    public void TakePostureDamage(int damage)
    {
        Posture -= damage;
        Posture = Mathf.Clamp(Posture, 0, PostureMAX);
    }

    private void OnDeath()
    {
        Debug.Log($"{characterName} has died!");
        // Additional death logic, e.g., triggering an animation or resetting the level.
    }

    public void setHealth()
    {
        health = healthMAX;
    }
    public void setPosture()
    {
        Posture = PostureMAX;
    }
}
