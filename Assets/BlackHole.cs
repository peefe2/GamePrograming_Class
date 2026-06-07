using UnityEngine;
using System.Collections;

public class BlackHole : MonoBehaviour
{
    public float pullRadius = 10f;      // 당기는 범위
    public float pullForce = 50f;       // 당기는 힘
    public float lifetime = 6f;         // 지속 시간
    public float rotationSpeed = 90f;

    private Transform player;
    private Rigidbody playerRb;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerRb = playerObj.GetComponent<Rigidbody>();
        }

        // 블랙홀은 바닥에 바로 배치
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 100f))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y + 0.5f, transform.position.z);
        }

        StartCoroutine(LifetimeRoutine());
    }

    void Update()
    {
        // 천천히 회전
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        // 범위 안에 있으면 플레이어 당기기
        if (player != null && playerRb != null)
        {
            float dist = Vector3.Distance(transform.position, player.position);
            if (dist < pullRadius && dist > 0.5f)
            {
                Vector3 pullDir = (transform.position - player.position).normalized;
                float forceMagnitude = pullForce * (1f - dist / pullRadius);
                playerRb.AddForce(pullDir * forceMagnitude, ForceMode.Force);
            }
        }
    }

    IEnumerator LifetimeRoutine()
    {
        // 서서히 커졌다가
        float growDuration = 1f;
        float elapsed = 0f;
        Vector3 targetScale = transform.localScale;
        transform.localScale = Vector3.zero;

        while (elapsed < growDuration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, elapsed / growDuration);
            yield return null;
        }

        // 유지
        yield return new WaitForSeconds(lifetime);

        // 서서히 사라짐
        elapsed = 0f;
        float shrinkDuration = 1f;
        while (elapsed < shrinkDuration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(targetScale, Vector3.zero, elapsed / shrinkDuration);
            yield return null;
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, pullRadius);
    }
}
