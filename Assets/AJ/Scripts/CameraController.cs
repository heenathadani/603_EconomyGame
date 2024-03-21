using System.Collections;
using System.Collections.Generic;
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
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 8f, Vector3.down, 100f);
        System.Array.Sort(hits, (a, b) => a.point.y.CompareTo(b.point.y));
        if (hits.Length > 0)
        {
            float dot = Mathf.Min(Vector3.Dot(hits[0].normal, Vector3.up), 1);
            if (Mathf.Approximately(dot, 1f))
            {
                pos.y = hits[0].point.y;
            }  
            else if (hits.Length > 1)
            {
                float halfpi = Mathf.PI / 2f;
                pos.y = Mathf.Lerp(hits[0].point.y, hits[1].point.y, Mathf.Acos(dot));
            }
            pos.y += 28f;
        }

        // Move the camera
        if (inputDir != Vector3.zero)
            pos += Time.deltaTime * cameraSpeed * (Quaternion.Euler(0, 45, 0) * inputDir.normalized);

        transform.position = pos;
    }
}
