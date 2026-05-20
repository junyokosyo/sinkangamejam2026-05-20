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
    private Enemy currentEnemy;

    private void Start()
    {
        InGameManager.OnStart += () => _isActive = true;
        qteManager.OnQTEFinished += SetEnemyActiveByQTE;
    }
    
    public void SetEnemySpeed(float speed)
    {
        enemySpeed = speed;
    }

    private void SetEnemyActiveByQTE(bool isSucceed)
    {
        if (!isSucceed)
        {
            return;
        }

        if (currentEnemy == null)
        {
            return;
        }
        
        // QTE成功時、敵の当たり判定を無効化する
        currentEnemy.hasQTESucceed = true;
    }

    private void Update()
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

    private void SpawnEnemy()
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
        
        currentEnemy = enemy;

        CarMovement move = enemyObj.GetComponent<CarMovement>();
        
        move.speed = enemySpeed;


        Debug.Log("Enemy生成");
    }
}