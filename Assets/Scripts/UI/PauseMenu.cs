using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject PauseMenuUI;
    public GameObject GameOverUI;
    
    public PlayerController player;
    void Update()
    {
        if(player.isAlive && Input.GetKeyDown(KeyCode.Escape)){
            if(GameIsPaused){
                Resume();
            } else{
                Pause();
            }
        }

        if (player.isAlive == false){
            MusicManager.instance.StopMusic();
            endScreen();
        }
    }

    public void endScreen(){
        GameOverUI.SetActive(true);
            Time.timeScale = 0f;
            GameIsPaused = true;
    }

    public void Resume(){
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause(){
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu(){
        Time.timeScale = 1f;
        GameIsPaused = false;
        player.isAlive = true;
        MusicManager.instance.PlayMusic("Background1");
        
        SceneManager.LoadScene(0);
    }

    public void QuitGame(){
        Application.Quit();
    }

    public void Restart(){
        Time.timeScale = 1f;
        GameIsPaused = false;
        GameOverUI.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    } 
}
