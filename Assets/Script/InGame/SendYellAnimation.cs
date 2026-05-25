using System.Collections;
using TMPro;
using UnityEngine;

public class SendYellAnimation : MonoBehaviour
{
    [SerializeField] private TMP_Text yellText;
    [SerializeField] private Transform yellPos;
    [SerializeField] private float animationDuration = 1.0f;
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private Vector2 direction;

    public void Play()
    {
        var target = Instantiate(yellText, yellPos.position, yellPos.rotation, yellPos.transform);
        StartCoroutine(PlayAnimationAsync(target.transform));
    }

    private IEnumerator PlayAnimationAsync(Transform targetObject)
    {
        float time = 0f;
        Vector2 startPosition = targetObject.position;

        while (time < animationDuration)
        {
            time += Time.deltaTime;
            float t = time / animationDuration;
            float curveValue = animationCurve.Evaluate(t);
            targetObject.position = startPosition + direction * curveValue;
            yield return null;
        }

        Destroy(targetObject.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (yellPos != null)
        {
            Vector2 startPosition = yellPos.position;
            Vector2 endPosition = startPosition + direction;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(startPosition, endPosition);
        }
    }
}