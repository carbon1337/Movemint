using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    private GameObject player; // Reference to the player GameObject
    private GameManager GM;

    void Awake() 
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("bruh");
            GM.Lose();
        }
    }
}
