using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverCollision : MonoBehaviour
{

    public Boss boss;

    public PauseMenu pauseMenu;
    public Collider2D arenaCollider;
    public Cinemachine.CinemachineConfiner cinemachine;
    public bool battleStarted = false;

    public GameObject BossUI;
    private void Start() {
        BossUI.SetActive(false);
    }
    private void Update() {
        if(boss.didDie){
            startMenu();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !battleStarted) {
            boss.gameObject.SetActive(true);
            MusicManager.instance.PlayMusic("Battle");
            battleStarted = true;
            arenaCollider.enabled = true;
            cinemachine.m_BoundingShape2D = arenaCollider;
            BossUI.SetActive(true);
        }
    }
    public void startMenu (){
        StartCoroutine(endScreen());
    }

    public IEnumerator endScreen(){
        yield return new WaitForSeconds(5);
        pauseMenu.endScreen();
    }
}
