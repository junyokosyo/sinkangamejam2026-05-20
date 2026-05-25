using System;
using System.Collections;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    [Header("Parameter")] [SerializeField] private float totalDistance;
    [SerializeField] private float defaultMoveSpeed;
    [SerializeField] private float additiveSpeed = 5f;
    [SerializeField] private float noMistakeMultiply = 1.1f;

    [Header("Binding")] [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform goalPosition;
    [SerializeField] private GameUI gameUI;
    [SerializeField] private TypingWindowManager typingWindowManager;
    [SerializeField] private QTEManager qTEManager;
    [SerializeField] private BackgroundLooper backGroundMover;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private PlayerController player;
    [SerializeField] private SendYellAnimation sendYellAnimation;

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
        player.OnDeath += () => StopGame(false);
        gameUI.OnClear += () => StopGame(true);
        SetSpeed(currentVelocity);

        AudioManager.Instance.PlayBGM(SoundType.InGameBGM);
    }

    private void StopGame(bool isCleared)
    {
        currentVelocity = 0;
        SetSpeed(currentVelocity);
        gameUI.StopTimer();
        enemySpawner.GameEnd();
        qTEManager.GameEnd();

        const float WAIT_TIME = 2.5f;
        StartCoroutine(isCleared ? Clear(WAIT_TIME) : GameOver(WAIT_TIME));
    }

    private IEnumerator GameOver(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        AudioManager.Instance.StopBGM();
        SceneTransition.Instance.SceneLoad(SceneName.Gameover);
    }

    private IEnumerator Clear(float waitTime)
    {
        AudioManager.Instance.PlaySE(SoundType.ResultClearSE);
        player.Clear();
        gameUI.Clear();
        typingWindowManager.Clear();
        yield return new WaitForSeconds(waitTime);
        AudioManager.Instance.StopBGM();
        SceneTransition.Instance.SceneLoad(SceneName.Clear);
    }

    private void SpeedUp(bool isMistaken)
    {
        var value = additiveSpeed * (isMistaken ? 1 : noMistakeMultiply);
        currentVelocity += value;
        gameUI.AddSpeedText(value);
        qTEManager.SpeedUpQTE();
        player.MultiplySpeed(1.1f);
        sendYellAnimation.Play();
        SetSpeed(currentVelocity);
    }

    private void QTECheck(bool isSucceed)
    {
        if (!isSucceed)
        {
            player.ResetSpeed();
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
            // X軸のみをターゲットに移動させる
            var pos = goalPosition.position;
            float targetX = playerTransform.position.x;
            float newX = Mathf.MoveTowards(pos.x, targetX, currentVelocity * Time.deltaTime);
            pos.x = newX;
            goalPosition.position = pos;

            // 目標に到達したらループを抜ける
            if (Mathf.Approximately(newX, targetX))
            {
                _hasReachedGoal = true;
            }
            gameUI.UpdateVelocityText(currentVelocity);
        }
    }
}