using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 5f, -10f);
    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;

    public Transform target;

    public bool cameraFollowsY = true;

    private float cameraStartPosY = 0f;

    void Awake()
    {
        //Initialize camera start pos for clamp
        cameraStartPosY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

        if(cameraFollowsY)
        {
            //Initialize camerYClamped
            float cameraYClamped = 0f;

            //Calulate the new clamped cameraY value
            cameraYClamped = Mathf.Clamp(cameraYClamped,  targetPosition.y, cameraStartPosY);

            //Update target position with clamped cameraY
            targetPosition = new Vector3(target.position.x, cameraYClamped, transform.position.z);
        }
        
        //Move camera accordingly
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
