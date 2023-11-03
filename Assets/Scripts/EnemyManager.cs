using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public EnemyController EnemyPrefab;
    [SerializeField, Tooltip("EnemyData for spawning enemies")]
    private EnemyData _enemyData;
    public List<EnemyController> enemyObjectPool = new List<EnemyController>();

    private void Start()
    {
        
    }

    public void spawnEnemy()
    {
        EnemyController enemy;
        if (enemyObjectPool.Count > 0)
        {
            enemy = enemyObjectPool[0];
            enemy.gameObject.SetActive(true);
            enemyObjectPool.RemoveAt(0);
        }
        else
        {
            enemy = Instantiate(EnemyPrefab);
            enemy.EnemyManager = this;
        }
        enemy.setEnemyData(_enemyData);
    }

}
