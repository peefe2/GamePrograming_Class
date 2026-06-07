using UnityEngine;

public class ChaserObstacle : MonoBehaviour
{
    private Transform player;
    public float chaseSpeed = 8f;
    private bool isLanded = false; 

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isLanded = true;
        }
    }

    void Update()
    {
        if (isLanded && player != null)
        {
            // 플레이어의 위치를 향해 방향 설정 (Y값은 현재 유지)
            Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);   
            
            // 플레이어를 바라보게 함
            transform.LookAt(targetPosition);

            // 플레이어 방향으로 이동
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, chaseSpeed * Time.deltaTime);
        }
    }
}
