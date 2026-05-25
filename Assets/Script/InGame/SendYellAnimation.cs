using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SendYellAnimation : MonoBehaviour
{
    [SerializeField] private TMP_Text yellText;
    [SerializeField] private Transform yellPos;
    [SerializeField] private float animationDuration = 1.0f;
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private Vector2 direction;
    [SerializeField] private bool _useCanvasRelative = true;

    private RectTransform _canvasRect;

    private readonly Queue<TMP_Text> textQueue = new();

    private void Start()
    {
        // Textをプーリング
        var canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            _canvasRect = canvas.GetComponent<RectTransform>();
        }
        for (int i = 0; i < 20; i++)
        {
            var target = Instantiate(yellText, yellPos.position, yellPos.rotation, yellPos.transform);
            textQueue.Enqueue(target);
            target.gameObject.SetActive(false);
        }
    }

    public void Play()
    {
        var target = textQueue.Dequeue();
        target.gameObject.SetActive(true);
        StartCoroutine(PlayAnimationAsync(target));
    }

    private IEnumerator PlayAnimationAsync(TMP_Text targetObject)
    {
        float time = 0f;
        var rt = targetObject.GetComponent<RectTransform>();
        Vector2 startAnchored = rt.anchoredPosition;

        // direction をキャンバス比に合わせてピクセル量に変換（必要な場合）
        Vector2 movement = direction;
        if (_useCanvasRelative && _canvasRect != null)
        {
            movement = new Vector2(direction.x * _canvasRect.rect.width, direction.y * _canvasRect.rect.height);
        }

        while (time < animationDuration)
        {
            time += Time.deltaTime;
            float t = time / animationDuration;
            float curveValue = animationCurve.Evaluate(t);
            rt.anchoredPosition = startAnchored + movement * curveValue;
            yield return null;
        }

        rt.anchoredPosition = startAnchored;
        
        targetObject.gameObject.SetActive(false);
        textQueue.Enqueue(targetObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (yellPos != null)
        {
            Vector2 startPosition = yellPos.position;
            Vector2 endPosition;
            if (_useCanvasRelative && _canvasRect != null)
            {
                // ワールド位置に変換して描画
                Vector2 pixelMovement = new Vector2(direction.x * _canvasRect.rect.width, direction.y * _canvasRect.rect.height);
                endPosition = startPosition + pixelMovement;
            }
            else
            {
                endPosition = startPosition + direction;
            }
            Gizmos.color = Color.red;
            Gizmos.DrawLine(startPosition, endPosition);
        }
    }
}