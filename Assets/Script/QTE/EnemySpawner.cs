using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("生成するEnemy")]
    public GameObject enemyPrefab;

    [Header("生成間隔")]
    public float spawnInterval = 5f;

    [Header("生成位置")]
    public Transform spawnPoint;

    [Header("参照")]
    public Transform playerTransform;

    public PlayerController player;

    public QTEManager qteManager;

    private float timer;

    void Update()
    {
        // Player死亡時停止
        if (player.isDead) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnEnemy();

            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        GameObject enemyObj =
            Instantiate(
                enemyPrefab,
                spawnPoint.position,
                Quaternion.identity);

        Enemy enemy =
            enemyObj.GetComponent<Enemy>();

        // 参照セット
        enemy.player = playerTransform;

        enemy.qteManager = qteManager;

        Debug.Log("Enemy生成");
    }
}