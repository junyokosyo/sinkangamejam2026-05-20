using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("生成するEnemy")] public GameObject enemyPrefab;

    [Header("生成間隔")] public float spawnInterval = 5f;

    [Header("生成位置")] public Transform spawnPoint;

    [Header("参照")] public Transform playerTransform;

    [SerializeField] private float enemySpeed = 20f;

    public PlayerController player;

    public QTEManager qteManager;

    private float timer;
    private bool _isActive;

    private void Start()
    {
        InGameManager.OnStart += () => _isActive = true;
    }
    
    public void SetEnemySpeed(float speed)
    {
        enemySpeed = speed;
    }

    void Update()
    {
        if (!_isActive)
        {
            return;
        }

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

        Carmovement move = enemyObj.GetComponent<Carmovement>();
        
        move.speed = enemySpeed;


        Debug.Log("Enemy生成");
    }
}