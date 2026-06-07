using UnityEngine;
using System.Collections;

public class SplitMeteor : MonoBehaviour
{
    public float lifetime = 5f;
    public int splitCount = 2;          // 쪼개질 개수
    public float splitForce = 5f;       // 쪼개질 때 튀는 힘
    public float minSize = 0.5f;
    public float maxSize = 1.8f;
    public float rotationSpeed = 150f;

    private bool isShrinking = false;
    private bool hasSplit = false;
    private Vector3 randomRotationAxis;
    private bool isFragment = false;    // 쪼개진 조각인지

    void Start()
    {
        float size = Random.Range(minSize, maxSize);
        transform.localScale = Vector3.one * size;
        transform.rotation = Random.rotation;
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
        if (collision.gameObject.CompareTag("Ground") && !hasSplit && !isFragment)
        {
            hasSplit = true;
            Split();
            StartCoroutine(ShrinkAndDestroy());
        }
        else if (collision.gameObject.CompareTag("Ground") && isFragment && !isShrinking)
        {
            StartCoroutine(ShrinkAndDestroy());
        }
    }

    void Split()
    {
        for (int i = 0; i < splitCount; i++)
        {
            GameObject fragment = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            fragment.transform.position = transform.position + Vector3.up * 0.5f;
            float fragSize = transform.localScale.x * 0.5f;
            fragment.transform.localScale = Vector3.one * fragSize;

            // 머티리얼 색상 (주황빛 운석 느낌)
            Renderer rend = fragment.GetComponent<Renderer>();
            if (rend != null)
                rend.material.color = new Color(0.7f, 0.3f, 0.1f);

            // 태그 설정
            fragment.tag = gameObject.tag;

            // SplitMeteor 컴포넌트 추가 (조각 표시)
            SplitMeteor sm = fragment.AddComponent<SplitMeteor>();
            sm.isFragment = true;
            sm.lifetime = 4f;

            // Rigidbody 추가
            Rigidbody rb = fragment.AddComponent<Rigidbody>();
            Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 0.5f, Random.Range(-1f, 1f)).normalized;
            rb.AddForce(randomDir * splitForce, ForceMode.Impulse);
        }
    }

    IEnumerator ShrinkAndDestroy()
    {
        isShrinking = true;
        Vector3 initialScale = transform.localScale;
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, elapsed / duration);
            yield return null;
        }

        Destroy(gameObject);
    }

    // 조각에 Collider 콜백 전달용
    public void SetAsFragment() { isFragment = true; }
}
