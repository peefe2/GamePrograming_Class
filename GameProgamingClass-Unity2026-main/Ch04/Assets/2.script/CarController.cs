using UnityEngine;

public class CarController : MonoBehaviour
{
    float speed = 0.1f;
    Vector2 startPos;


    void Start()
    {
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            this.startPos = Input.mousePosition;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            Vector2 endPos = Input.mousePosition;
            float swipLen= endPos.x- startPos.x;
            speed = swipLen / 1000;
            GetComponent<AudioSource>().Play();
        }
        transform.Translate(speed, 0, 0);
        speed *= 0.99f;
    }
}
