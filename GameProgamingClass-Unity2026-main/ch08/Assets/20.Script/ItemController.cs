using UnityEngine;

public class ItemController : MonoBehaviour
{
    public float dropSpeed = -1f;

    private void Update()
    {
        transform.Translate(0, dropSpeed * Time.deltaTime, 0);
        if (transform.position.y < -5f)
        {
            Destroy(gameObject);
        }
    }
}
