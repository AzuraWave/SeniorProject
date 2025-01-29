using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectSpawner : MonoBehaviour
{
    
    public GameObject shurikenPrefab;
    public PlayerController player;
    private Transform playerTransform => player.transform;

    public void SpawnShuriken(Vector2 direction)
    {
            
        Vector3 spawnPosition = playerTransform.position + playerTransform.up * 0.5f;  

        
        GameObject shuriken = Instantiate(shurikenPrefab, spawnPosition, playerTransform.rotation);

        
        shuriken.GetComponent<Shuriken>().Initialize(direction);
    }
}
