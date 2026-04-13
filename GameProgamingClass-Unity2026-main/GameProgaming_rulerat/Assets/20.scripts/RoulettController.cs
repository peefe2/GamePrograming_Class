using UnityEngine;

public class RoulettController : MonoBehaviour
{
    public float startSpeed = 10f;
    public float decreaseRatio = 0.97f;
    float rotSpeed = 0; // 회전속도
    void Start()
    {
        Application.targetFrameRate = 60;// 프레임 최대값 지정
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            this.rotSpeed = startSpeed;
        }

        transform.Rotate(0, 0, this.rotSpeed);

        this.rotSpeed *= decreaseRatio;
    }
}
