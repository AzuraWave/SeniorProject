using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Handler : MonoBehaviour
{

    public SettingsMenu settingsMenu;
    void Start()
    {
        if (MusicManager.instance != null)
        {
            if (MusicManager.instance.currentTrack != "Background1")
            {
                MusicManager.instance.PlayMusic("Background1");
            }
        }
        settingsMenu.LoadVolume();
        Time.timeScale = 1f;
    }

}
