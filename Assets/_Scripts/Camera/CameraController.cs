using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 5f, -10f);
    public float xSmoothTime = 0.1f;
    public float ySmoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;

    public Transform target;

    public bool cameraFollowsY = true;

    private float cameraStartPosY = 0f;

    void Awake()
    {
        // Initialize camera start pos for clamp
        cameraStartPosY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 targetPosition = new Vector3();
        Vector3 cameraPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        if(cameraFollowsY)
        {
            // Initialize camerYClamped
            float cameraYClamped = 0f;

            // Calulate the new clamped cameraY value
            cameraYClamped = Mathf.Clamp(cameraYClamped,  target.position.y, cameraStartPosY);

            // Update target position with clamped cameraY
            targetPosition = new Vector3(target.position.x, cameraYClamped, transform.position.z);
        }
        else
        {
            // Locks the y axis if camera follows y is off
            targetPosition = new Vector3(target.position.x, transform.position.y, transform.position.z);
        }
        
        // Seperate damping for y and x values
        cameraPosition.x = Mathf.SmoothDamp(transform.position.x, targetPosition.x, ref velocity.x, xSmoothTime);
        cameraPosition.y = Mathf.SmoothDamp(transform.position.y, targetPosition.y, ref velocity.y, ySmoothTime);

        transform.position = cameraPosition;

    }
}
