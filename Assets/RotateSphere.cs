// RotateObject.cs - 오브젝트에 붙이면 계속 회전
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float speed = 50f;

    void Update()
    {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
    }
}