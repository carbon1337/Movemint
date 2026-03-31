using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinDetector : MonoBehaviour
{
    private GameManager GM;

    void Awake() {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Win")) {
            Debug.Log("you won!");

            GM.Win();
        }
    }
}
