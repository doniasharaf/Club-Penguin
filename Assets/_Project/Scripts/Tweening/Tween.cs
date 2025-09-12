using System;
using System.Collections;
using UnityEngine;

public class Tween : MonoBehaviour
{
    private static Tween instance;
    private void Awake() => instance = this;

    public static void Rotate(Transform target, Quaternion start, Quaternion end, float duration, Func<float, float> easing = null, Action onComplete = null)
    {
        instance.StartCoroutine(instance.RotateRoutine(target, start, end, duration, easing, onComplete));
    }

    private IEnumerator RotateRoutine(Transform target, Quaternion start, Quaternion end, float duration, Func<float, float> easing, Action onComplete)
    {
        float time = 0f;
        easing ??= EaseLinear;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            t = easing(t);

            target.rotation = Quaternion.Slerp(start, end, t);
            yield return null;
        }

        target.rotation = end;
        onComplete?.Invoke();
    }

    public static float EaseLinear(float t) => t;
    public static float EaseOutCubic(float t) => 1f - Mathf.Pow(1f - t, 3f);
    public static float EaseInOutQuad(float t) => t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f;
}
