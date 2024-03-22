using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed = 10f;
    public float groundHeight = 28f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 inputDir = new();
        Vector3 pos = transform.position;
        pos.y = groundHeight;

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
        // We are spherecasting AND raycasting. This allows for smooth interpolation
        // as the camera traverses a potentially bumpy ground.
        if (Physics.SphereCast(transform.position, 8f, Vector3.down, out RaycastHit hit) &&
            Physics.Raycast(transform.position, Vector3.down, out RaycastHit bottomHit))
        {
            float dot = Mathf.Min(Vector3.Dot(hit.normal, Vector3.up), 1);
            pos.y += Mathf.Lerp(hit.point.y, bottomHit.point.y, Mathf.Acos(dot));
        }

        // Move the camera
        if (inputDir != Vector3.zero)
            pos += Time.deltaTime * cameraSpeed * (Quaternion.Euler(0, 45, 0) * inputDir.normalized);

        transform.position = pos;
    }
}
