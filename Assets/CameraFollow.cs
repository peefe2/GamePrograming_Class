using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 12, -15);
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        if (target != null)
        {
            // [업그레이드] 플레이어의 속도에 따라 카메라가 살짝 출렁이게 함
            Rigidbody targetRb = target.GetComponent<Rigidbody>();
            Vector3 dynamicOffset = offset;

            if (targetRb != null)
            {
                // 속도가 빠를수록 카메라가 더 뒤로 멀어짐 (속도감 상승!)
                dynamicOffset.z -= targetRb.linearVelocity.magnitude * 0.2f;
            }

            Vector3 desiredPosition = target.position + dynamicOffset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // 카메라가 항상 플레이어를 쳐다보게 함
            transform.LookAt(target.position + Vector3.up * 1f);
        }
    }
}