using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject spawnLocation;
    [SerializeField] private float spawnMinimum;
    [SerializeField] private float spawnMaximum;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("SpawnEnemy");
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(spawnMinimum, spawnMaximum));
            //Vector3 spawnPosition = transform.position + (Vector3.forward * 20);
            Vector3 spawnPosition = spawnLocation.transform.position;
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            yield return null;
        }
        
        
    }
}
