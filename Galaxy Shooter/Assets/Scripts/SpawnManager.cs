using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject[] _powerups;
    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private float _enemySpawnTime = 4.0f;

    private bool _stopSpawning = false;

    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
  
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while(!_stopSpawning)
        {
            var positionToSpawn = new Vector3(Random.Range(-9.0f, 9.0f), 7, 0);
            var newEnemy = Instantiate(_enemyPrefab, positionToSpawn, Quaternion.identity);
            var enemyComponent = newEnemy.GetComponent<Enemy>();
            newEnemy.transform.parent = _enemyContainer.transform;

            SetEnemySpawnTimeAndSpeed(enemyComponent);
            yield return new WaitForSeconds(_enemySpawnTime);
        }
    }

    private IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (!_stopSpawning)
        {
            var positionToSpawn = new Vector3(Random.Range(-8.0f, 8.0f), 7, 0);
            int randomPowerUp = Random.Range(0, 3);
            Instantiate(_powerups[randomPowerUp], positionToSpawn, Quaternion.identity);
            var time = Random.Range(3, 8);
            yield return new WaitForSeconds(time);
        }
    }

    public void OnPlayerDeath()
    {
        this._stopSpawning = true;
    }

    void SetEnemySpawnTimeAndSpeed(Enemy enemyComponent)
    {
        var playerScore = _player.getPlayerScore();
        if (playerScore >= 250)
        {
            _enemySpawnTime = 0.7f;
            enemyComponent.IncreaseSpeed(2);
        }
        else if (playerScore >= 150)
        {
            _enemySpawnTime = 1.0f;
            enemyComponent.IncreaseSpeed(2);
        }
        else if (playerScore >= 100)
        {
            _enemySpawnTime = 2.0f;
            enemyComponent.IncreaseSpeed(2);
        }
        else if (playerScore >= 50)
        {
            _enemySpawnTime = 3.0f;
            enemyComponent.IncreaseSpeed(2);
        }
    }
}
