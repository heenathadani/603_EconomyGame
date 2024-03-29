using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class RNAComponent : MonoBehaviour
{
    public delegate void OnCollectedHandler(int amount);
    public event OnCollectedHandler OnCollected;

    public int amount = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Unit u) && u.Hostility == Hostility.Friendly)
        {
            GameObject.FindWithTag("RNABank").GetComponent<RNABank>().AddRNA(amount);

            // Remove and notify listeners
            OnCollected?.Invoke(amount);
            GetComponent<Unit>().Destroy();
        }
    }
}
