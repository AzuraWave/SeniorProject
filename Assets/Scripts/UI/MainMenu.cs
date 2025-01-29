using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    private void Start(){
        MusicManager.instance.PlayMusic("Background1");
    }
    public void PlayGame(){
        SceneManager.LoadSceneAsync(1);
    }

    public void Quit(){
        Application.Quit();
    }
}
