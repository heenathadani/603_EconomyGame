using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 inputDir = new();

        // handle input
        if (Input.GetKey(KeyCode.LeftArrow))
            inputDir.x = -1;
        if (Input.GetKey(KeyCode.RightArrow))
            inputDir.x = 1;
        if (Input.GetKey(KeyCode.DownArrow))
            inputDir.z = -1;
        if (Input.GetKey(KeyCode.UpArrow))
            inputDir.z = 1;
        
        // Handle camera height from ground
        if (Physics.SphereCast(transform.position, 4f, Vector3.down, out RaycastHit hit))
        {
            inputDir.y = Vector3.Dot(hit.normal, Vector3.up);
        }

        // Move the camera
        if (inputDir != Vector3.zero)
            transform.position += Time.deltaTime * cameraSpeed * (Quaternion.Euler(0, 45, 0) * inputDir.normalized);
    }
}
