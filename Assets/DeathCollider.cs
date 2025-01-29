using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeathCollider : MonoBehaviour
{

        public PlayerController player;
    
    private void Update() {
        if(player.transform.position.y < -10){
            player.stats.health = 0;
        }
    }
}
