using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject enemyPrefab;        // 기본 운석
    public GameObject chaserPrefab;       // 추격자
    public GameObject lightningPrefab;    // 번개
    public GameObject splitMeteorPrefab;  // 분열 운석 (15초~)
    public GameObject blackholePrefab;    // 블랙홀 (30초~)

    public Transform playerTransform;

    [Header("Spawn Settings")]
    public float spawnRate = 1.5f;
    public float minSpawnRate = 0.5f;
    public float difficultyScale = 0.02f;
    public float spawnRange = 15f;
    public float spawnHeight = 10f;

    private float nextSpawnTime = 0f;
    private float survivedTime = 0f;

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

        survivedTime += Time.deltaTime;

        if (spawnRate > minSpawnRate)
            spawnRate -= difficultyScale * Time.deltaTime;

        if (Time.time >= nextSpawnTime)
        {
            Spawn();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void Spawn()
    {
        float randomX = Random.Range(playerTransform.position.x - spawnRange, playerTransform.position.x + spawnRange);
        float randomZ = Random.Range(playerTransform.position.z - spawnRange, playerTransform.position.z + spawnRange);
        Vector3 spawnPos = new Vector3(randomX, spawnHeight, randomZ);

        GameObject prefabToSpawn = PickPrefab();
        if (prefabToSpawn == null) prefabToSpawn = enemyPrefab;

        if (prefabToSpawn == lightningPrefab)
            spawnPos.y = spawnHeight * 1.5f;

        Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
    }

    GameObject PickPrefab()
    {
        float rand = Random.value;

        // 20초 이후
        if (survivedTime >= 20f)
        {
            if (rand < 0.50f) return splitMeteorPrefab;
            return blackholePrefab;
        }
        // 10초 이후 - 블랙홀 + 분열 운석
        else if (survivedTime >= 10f)
        {
            if (rand < 0.50f) return splitMeteorPrefab;
            return blackholePrefab;
        }
        // 10초 이전 - 기본 운석만
        else
        {
            return enemyPrefab;
        }
    }
}
