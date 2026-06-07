using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour
{
    public float lifetime = 5f;
    private bool isShrinking = false;

    [Header("Meteor Settings")]
    public float minSize = 0.8f;
    public float maxSize = 2.2f;
    public float rotationSpeed = 120f;

    private Vector3 randomRotationAxis;

    void Start()
    {
        // 랜덤 크기
        float size = Random.Range(minSize, maxSize);
        transform.localScale = Vector3.one * size;

        // 랜덤 초기 회전
        transform.rotation = Random.rotation;

        // 랜덤 회전 축
        randomRotationAxis = Random.onUnitSphere;

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (!isShrinking)
            transform.Rotate(randomRotationAxis * rotationSpeed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !isShrinking)
        {
            StartCoroutine(ShrinkAndDestroy());
        }
    }

    IEnumerator ShrinkAndDestroy()
    {
        isShrinking = true;

        // 2초 대기
        yield return new WaitForSeconds(2f);

        // 서서히 사라짐
        Vector3 initialScale = transform.localScale;
        float duration = 1.0f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, elapsed / duration);
            yield return null;
        }

        Destroy(gameObject);
    }
}
