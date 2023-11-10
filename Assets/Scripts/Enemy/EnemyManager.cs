using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public EnemyController EnemyPrefab;
    [SerializeField, Tooltip("EnemyData for spawning enemies")]
    private EnemyData _enemyData;
    public List<EnemyController> enemyObjectPool = new List<EnemyController>();
    [SerializeField, Tooltip("How far away the enemy should spawn, 0.5 for on the edge")]
    private float _bufferDistance = 0.6f;

    private void Start()
    {
        
    }
    public void SpawnEnemy()
    {
        EnemyController enemy;
        if (enemyObjectPool.Count > 0)
        {
            enemy = enemyObjectPool[0];
            enemyObjectPool.RemoveAt(0);
        }
        else
        {
            enemy = Instantiate(EnemyPrefab);
            enemy.EnemyManager = this;
        }
        enemy.SetEnemyData(_enemyData);
        enemy.transform.position = getRandomSpawnPosition();

        // have to set active after moving
        // or else it'll hit bullets from previous spot
		enemy.gameObject.SetActive(true);
	}


    float time = 0;
	private void Update()
	{
        time += Time.deltaTime;
        if (time > 0.1)
        {
            time = 0;
            SpawnEnemy();
        }
        if (Input.GetButtonDown("Fire2"))
            SpawnEnemy();
	}

    /// <summary>
    /// Gets a random position based on the current camera's viewport
    /// </summary>
    /// <returns>A random world position in a square's border shape</returns>
	private Vector2 getRandomSpawnPosition()
    {
        int randomTries = 10;
        Vector2 spawnPosition;
        do
        {

            float randomRadian = Random.Range(0, Mathf.PI * 2);
            float cornerDistance = Mathf.Sqrt(_bufferDistance * _bufferDistance * 2);
            float xValue = Mathf.Sin(randomRadian) * cornerDistance;
            float yValue = Mathf.Cos(randomRadian) * cornerDistance;
            xValue = Mathf.Clamp(xValue, -_bufferDistance, _bufferDistance) + 0.5f;
            yValue = Mathf.Clamp(yValue, -_bufferDistance, _bufferDistance) + 0.5f;
            spawnPosition = Camera.main.ViewportToWorldPoint(new Vector2(xValue, yValue));
            if (!Physics2D.OverlapPoint(spawnPosition))
                break;
        } while (randomTries > 0);
        return spawnPosition;
	}
}
