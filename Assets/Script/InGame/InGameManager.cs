using System;
using System.Collections;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    [Header("Parameter")]
    [SerializeField] private float totalDistance;
    [SerializeField] private float defaultMoveSpeed;
    [SerializeField] private float additiveSpeed = 5f;
    [SerializeField] private float noMistakeMultiply = 1.1f;
    
    [Header("Binding")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform goalPosition;
    [SerializeField] private GameUI gameUI;
    [SerializeField] private TypingWindowManager typingWindowManager;
    [SerializeField] private QTEManager qTEManager;
    [SerializeField] private BackgroundLooper backGroundMover;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private PlayerController player;

    public static event Action OnStart;
    public static event Action OnClear;

    private bool _hasReachedGoal;
    private float currentVelocity;

    private void GameStart()
    {
        OnStart?.Invoke();
        StartCoroutine(MoveGoalPosition());
        typingWindowManager.GameStart();
    }

    private void Start()
    {
        // ゴールまでの距離を計算
        goalPosition.position = playerTransform.position + playerTransform.right * totalDistance;
        currentVelocity = defaultMoveSpeed;
        gameUI.OnCountDownComplete += GameStart;
        qTEManager.OnQTESwitched += typingWindowManager.SetQTE;
        qTEManager.OnQTEFinished += QTECheck;
        typingWindowManager.OnTypingComplete += SpeedUp;
        SetSpeed(currentVelocity);
    }

    private void SpeedUp(bool isMistaken)
    {
        var value = additiveSpeed * (isMistaken ? 1 : noMistakeMultiply);
        currentVelocity += value;
        qTEManager.successCount++;
        player.PlayerSpeed *= 1.1f;
        SetSpeed(currentVelocity);
    }

    private void QTECheck(bool isSucceed)
    {
        if (!isSucceed)
        {
            player.PlayerSpeed = 1f;
            currentVelocity = defaultMoveSpeed;
            SetSpeed(currentVelocity);
        }
    }

    private void SetSpeed(float speed)
    {
        backGroundMover.SetSpeed(speed);
        gameUI.UpdateVelocityText(speed);
        enemySpawner.SetEnemySpeed(speed * 1.5f);
    }

    private IEnumerator MoveGoalPosition()
    {
        while (!_hasReachedGoal)
        {
            yield return null;
            goalPosition.position = Vector3.MoveTowards(
                goalPosition.position,
                playerTransform.position,
                currentVelocity * Time.deltaTime
            );
            gameUI.UpdateVelocityText(currentVelocity);
        }
    }
}