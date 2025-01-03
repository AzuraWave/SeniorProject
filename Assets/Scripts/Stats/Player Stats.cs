using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Stats/PlayerStats")]
public class CharacterStats : ScriptableObject
{
    public string characterName = "Samurai";
    public int health;
    public int healthMAX = 100;
    public int BasicAttackDamage = 10;
    public int PostureMAX = 100;
    public int PostureRegen = 10;
    public float Posture;
    public int PostureDamage = 20;
    public float speed = 5f;
    public float acceleration = 1.5f;
    public float jumpForce = 5f;
    public float parryWindow = 0.5f;
    public float drag = 0.9f;
    public int healCooldown = 5;

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


}
