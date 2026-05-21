using UnityEngine;

public class PlayerMoveRB : MonoBehaviour
{
    public float moveSpeed = 10;
    public float rotationSpeed = 50f;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        float xSpeed = xInput * rotationSpeed * Time.deltaTime;
        float zSpeed = zInput * moveSpeed * Time.deltaTime;

        //transform.Translate(0, 0, zSpeed);
        transform.Rotate(0, xSpeed, 0);
        rb.linearVelocity = zSpeed * transform.forward;
    }
}
