using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {
    public IEnumerator Shake(float duration, float magnitude) {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration) {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            // 게임 오버 시 Time.timeScale이 0이 되더라도 흔들림이 유지되도록 unscaledDeltaTime 사용
            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
