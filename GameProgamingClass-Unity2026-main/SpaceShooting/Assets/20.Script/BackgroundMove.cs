using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    public GameObject backGround1;
    public GameObject backGround2;

    public float moveSpeed;
    private Vector3 moveVec;
    private void Start()
    {
        moveVec = new Vector3(0, moveSpeed, 0);
    }
    private void Update()
    {
        backGround1.transform.position -= moveVec;
        backGround2.transform.position -= moveVec;

        if (backGround1.transform.position.y <= -15)
        {
            backGround1.transform.position = new Vector3(0, 25.5f, 0);
        }        
        if (backGround2.transform.position.y <= -15)
        {
            backGround2.transform.position = new Vector3(0, 25.5f, 0);
        }
    }
}
