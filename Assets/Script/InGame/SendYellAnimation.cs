using System.Collections;
using UnityEngine;

public class SendYellAnimation : MonoBehaviour
{
    [SerializeField] private Transform yellImage;
    [SerializeField] private float animationDuration = 1.0f;
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private Vector2 direction;

    public IEnumerator PlayAnimation()
    {
        var go = Instantiate(yellImage, yellImage.position, yellImage.rotation, yellImage.transform);
        float time = 0f;
        Vector2 startPosition = go.position;

        while (time < animationDuration)
        {
            time += Time.deltaTime;
            float t = time / animationDuration;
            float curveValue = animationCurve.Evaluate(t);
            go.position = startPosition + direction * curveValue;
            yield return null;
        }
        
        Destroy(go.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (yellImage != null)
        {
            Vector2 startPosition = yellImage.position;
            Vector2 endPosition = startPosition + direction;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(startPosition, endPosition);
        }
    }
}
