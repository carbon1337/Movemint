using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public Slider sfxSlider, musicSlider;
    
    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Awake() {
        if(sfxSlider != null) {
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        }
        if(musicSlider != null) {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        }
    }

    public void GoToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToLevelSelect() {
        SceneManager.LoadScene("LevelSelect");
    }

    public void GoToNextLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ToggleOptionsMenu() {
        //if the menu is off, turn on, if the menu is on, turn off.
    }

    public void ToggleLeaderboardMenu() {
        //if the menu is off, turn on, if the menu is on, turn off.
    }
    public void ClearData() {
        PlayerPrefs.DeleteAll();
    }

    public void SafetyCheck() {
        // activate a panel to make sure the player wants to do said action.
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void PlaySelectSound() {
        AudioManager.instance.PlaySFX("MenuSelect", false, 1);
    }

    public void MusicVolume() {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
    }

    public void SFXVolume() {
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
    }
}
