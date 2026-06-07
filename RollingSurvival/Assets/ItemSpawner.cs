using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject speedUpPrefab;
    public GameObject shieldPrefab;
    public Transform playerTransform;

    public float spawnInterval = 8f;    // 아이템 생성 간격
    public float spawnRange = 12f;

    private float nextSpawnTime = 5f;   // 5초 후 첫 아이템 등장

    void Start()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        if (Time.time >= nextSpawnTime)
        {
            Spawn();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void Spawn()
    {
        float randomX = Random.Range(playerTransform.position.x - spawnRange, playerTransform.position.x + spawnRange);
        float randomZ = Random.Range(playerTransform.position.z - spawnRange, playerTransform.position.z + spawnRange);
        Vector3 spawnPos = new Vector3(randomX, 1f, randomZ);

        // 50% 확률로 스피드업 or 실드
        GameObject prefab = Random.value > 0.5f ? speedUpPrefab : shieldPrefab;
        if (prefab != null)
            Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}
