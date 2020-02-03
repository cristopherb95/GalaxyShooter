using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _tripleShotPowerupPrefab;
    [SerializeField]
    private GameObject _enemyContainer;

    private bool _stopSpawning = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    // Update is called once per frame
    void Update()
    {
  
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        while(!_stopSpawning)
        {
            var positionToSpawn = new Vector3(Random.Range(-8.0f, 8.0f), 7, 0);
            var newEnemy = Instantiate(_enemyPrefab, positionToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    private IEnumerator SpawnPowerUpRoutine()
    {
        while (!_stopSpawning)
        {
            var positionToSpawn = new Vector3(Random.Range(-8.0f, 8.0f), 7, 0);
            Instantiate(_tripleShotPowerupPrefab, positionToSpawn, Quaternion.identity);
            var time = Random.Range(3, 8);
            yield return new WaitForSeconds(time);
        }
    }

    public void OnPlayerDeath()
    {
        this._stopSpawning = true;
    }
}
