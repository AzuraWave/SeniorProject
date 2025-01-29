using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObjectSpawner : MonoBehaviour
{
    public GameObject shurikenPrefab;
    public Enemy Enemy;
    private Transform enemyTransform => Enemy.transform;

    public void SpawnShuriken(Vector2 direction)
    {
            
        Vector3 spawnPosition = enemyTransform.position + enemyTransform.up * 0.5f;  

        
        GameObject shuriken = Instantiate(shurikenPrefab, spawnPosition, enemyTransform.rotation);

        
        shuriken.GetComponent<Shuriken>().Initialize(direction);
    }
}
