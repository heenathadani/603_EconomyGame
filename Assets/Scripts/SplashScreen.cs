using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    public GameObject gameObjectToHide;  // Assign the GameObject to hide in the Inspector

    void Start()
    {
        StartCoroutine(ShowForSeconds(gameObjectToHide, 6.0f));
    }

    IEnumerator ShowForSeconds(GameObject targetObject, float delay)
    {
        yield return new WaitForSeconds(delay);  // Wait for the specified delay
        targetObject.SetActive(false);            // Hide the GameObject
    }
}
