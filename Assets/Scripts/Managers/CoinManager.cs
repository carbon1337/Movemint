using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public int coinID;
    private GameManager GM;
    private AudioSource coinSound;
    private SpriteRenderer sprite;
    private bool coinCollected = false;

    void Awake() 
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        coinSound = gameObject.GetComponent<AudioSource>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(coinCollected) {
                return;
            }
            
            coinSound.Play();
            GM.CollectCoin(coinID);

            sprite.enabled = false;
            coinCollected = true;
        }
    }
}
