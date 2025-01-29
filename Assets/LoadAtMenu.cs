using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAtMenu : MonoBehaviour
{
    public SettingsMenu settingsMenu;


    private void Start() {
        settingsMenu.LoadVolume();
    }
}
