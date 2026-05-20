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
    [SerializeField] private BackGroundMover backGroundMover;

    public static event Action OnStart;

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
        backGroundMover.SetSpeed(currentVelocity);
        gameUI.UpdateVelocityText(currentVelocity);
    }

    private void SpeedUp(bool isMistaken)
    {
        currentVelocity += additiveSpeed * (isMistaken ? 1 : noMistakeMultiply);
        backGroundMover.SetSpeed(currentVelocity);
        gameUI.UpdateVelocityText(currentVelocity);
    }

    private void QTECheck(bool isSucceed)
    {
        if (!isSucceed)
        {
            currentVelocity = defaultMoveSpeed;
            backGroundMover.SetSpeed(currentVelocity);
            gameUI.UpdateVelocityText(currentVelocity);
        }
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