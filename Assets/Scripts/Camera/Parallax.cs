using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Parallax : MonoBehaviour
{
    private float length;
    private float startPos;
    public CameraController cam;

    [SerializeField] private float parallaxEffect;

    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<CameraController>();
        startPos = transform.position.x;

        if(gameObject.GetComponent<SpriteRenderer>() != null) {
            length = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
        }

    }

    void Update()
    {
        float temp = (cam.target.position.x * (1 - parallaxEffect));
        float distance = (cam.target.position.x * parallaxEffect);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if (temp > startPos + length)
        {
            startPos += length;
        }
        else if (temp < startPos - length)
        {
            startPos -= length;
        } 
    }
}
