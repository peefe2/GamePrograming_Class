using UnityEngine;

public class TargetController : MonoBehaviour
{
    GameObject player;
    TargetGenerate tg;
    private void Start()
    {
        player = GameObject.Find("Player");
        tg = GameObject.FindObjectOfType<TargetGenerate>();
    }
    private void Update()
    {
        transform.LookAt(player.transform);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Bamsongi"))
        {
            tg.GenerateTarget(player.transform.position);
            Destroy(gameObject);
        }
    }
}
