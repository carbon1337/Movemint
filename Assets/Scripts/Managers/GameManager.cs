using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class GameManager : MonoBehaviour
{
    public LevelData Data;

    private TimerManager TM;
    private WinDetector WD;

    private Canvas deathCanvas;
    private Canvas winCanvas;

    private PlayerController PC;

    public TMP_Text pbText;
    private Image newPBIMG;

    public int whichCoinCollected = 0;
    public bool[] coinsCollected = {false, false, false};
    public Image[] coinIMGs;

    private bool levelCompleted = false;
    public bool levelFailed = false;

    void Awake()
    {
        PC = GameObject.Find("Player").GetComponent<PlayerController>();

        TM = gameObject.GetComponent<TimerManager>();
        WD = GameObject.Find("Player").GetComponent<WinDetector>();

        deathCanvas = GameObject.Find("DeathCanvas").GetComponent<Canvas>();
        winCanvas = GameObject.Find("WinCanvas").GetComponent<Canvas>();

        newPBIMG = GameObject.Find("NewPB").GetComponent<Image>();

        deathCanvas.enabled = false;
        winCanvas.enabled = false;
        newPBIMG.enabled = false;

        coinIMGs[0].enabled = false;
        coinIMGs[1].enabled = false;
        coinIMGs[2].enabled = false;
        
        AudioManager.instance.musicSource.Play();
    }

    public void Update() {
        if(levelCompleted == false && levelFailed == false) {
            if((PC.rb.velocity.x > 0 || PC.rb.velocity.y > 0) && TM.isTimerRunning == false) {
                TM.StartTimer();
            }
        }

    }

    public void Win() {
        TM.StopTimer();
        AudioManager.instance.musicSource.Pause();
        AudioManager.instance.PlaySFX("WinDoor", false, 1f);
        PC.canMove = false;

        levelCompleted = true;


        winCanvas.enabled = true;

        HandleLevelSave(Data.levelID, TM.elapsedTime, coinsCollected);
    }

    public void Lose() {
        TM.StopTimer(); 
        AudioManager.instance.musicSource.Pause();

        levelFailed = true;
        //stop the player from moving
        PC.canMove = false;
        //animate the player death
        
        //Display death menu
        deathCanvas.enabled = true;
    }

    public void CollectCoin(int coinID) {
        //Save the data for which coin collected (1-3)
        whichCoinCollected = coinID;
        //Send that data to a variable to use in levelSave.

        if(whichCoinCollected == 1) {
            coinsCollected[0] = true;
        } else if(whichCoinCollected == 2) {
            coinsCollected[1] = true;
        } else if(whichCoinCollected == 3) {
            coinsCollected[2] = true;
        }

    }

    public void HandleLevelSave(string levelID, float currentLevelTime, bool[] currentCoinsCollected) {
        //Find out which level the player is on
        string levelBestTimeKey = "BestTime_" + levelID;

        string levelCoin1Key = "Coin1Collected_" + levelID;
        string levelCoin2Key = "Coin2Collected_" + levelID;
        string levelCoin3Key = "Coin3Collected_" + levelID;

        if(currentCoinsCollected[0] == true || PlayerPrefs.GetInt(levelCoin1Key) != 0) {
            PlayerPrefs.SetInt(levelCoin1Key, 1);
            coinIMGs[0].enabled = true;
        }
        if(currentCoinsCollected[1] == true || PlayerPrefs.GetInt(levelCoin2Key) != 0) {
            PlayerPrefs.SetInt(levelCoin2Key, 1);
            coinIMGs[1].enabled = true;
        }
        if(currentCoinsCollected[2] == true || PlayerPrefs.GetInt(levelCoin3Key) != 0) {
            PlayerPrefs.SetInt(levelCoin3Key, 1);
            coinIMGs[2].enabled = true;
        }

        float bestTimeForLevel = PlayerPrefs.GetFloat(levelBestTimeKey, 0);

        // assign the personal best value for said level
        if (currentLevelTime < bestTimeForLevel || bestTimeForLevel == 0) {
            bestTimeForLevel = currentLevelTime;
            PlayerPrefs.SetFloat(levelBestTimeKey, bestTimeForLevel);

            newPBIMG.enabled = true;
        }

        //update the correct personal best on highscore event

        pbText.text = PlayerPrefs.GetFloat(levelBestTimeKey).ToString("F2");
    }
}
