using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{
    public enum ItemType { SpeedUp, Shield }
    public ItemType itemType;

    public float duration = 5f;         // 아이템 효과 지속 시간
    public float lifetime = 8f;         // 아이템이 바닥에 유지되는 시간
    public float bobSpeed = 2f;         // 위아래 둥실거리는 속도
    public float bobHeight = 0.3f;      // 둥실거리는 높이

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // 위아래 둥실거리는 효과
        float newY = startPos.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // 천천히 회전
        transform.Rotate(Vector3.up * 90f * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerBall player = other.GetComponent<PlayerBall>();
            if (player != null)
            {
                SoundManager.Instance?.PlayItemPickup();
                player.ApplyItem(itemType, duration);
                Destroy(gameObject);
            }
        }
    }
}
