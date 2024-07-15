using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private bool _stopSpawning = false;
    [SerializeField]
    private GameObject[] _powerups;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSpawn()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3f);
        while (_stopSpawning == false)
        {
            float randomX = Random.Range(-8f, 8f);

            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(randomX, 7f, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(1.5f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3f);
        while (_stopSpawning == false)
        {
            float randomX = Random.Range(-8f, 8f);
            int randomPowerup = Random.Range(0, 3);
            Instantiate(_powerups[randomPowerup], new Vector3(randomX, 7f, 0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(6, 15));
        }
    }

    public void onPlayerDeath()
    {
        _stopSpawning = true;
    }
}
