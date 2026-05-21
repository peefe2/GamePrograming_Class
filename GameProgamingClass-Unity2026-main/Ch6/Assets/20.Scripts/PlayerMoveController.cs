using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMoveController : MonoBehaviour
{
    public float jumpForce;
    public float walkForce = 30;
    public float maxWalkSpeed = 1f;
    public float key;


    public Animator animator;
    public Sprite[] walkSprites;
    public Sprite jumpSprite;
    public float animtionPeriod = 0.1f;
    float time = 0;
    int idx = 0;
    SpriteRenderer sr;

    Rigidbody2D rb;
    void Start()
    {
        Application.targetFrameRate = 60;
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        key = 0;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            key = 1;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            key = -1;
        }
        if(key!=0)
        {
            transform.localScale = new Vector3(key, 1, 1);
        }
        if (Input.GetMouseButtonDown(0) && rb.linearVelocityY == 0)
        {
            rb.AddForce(transform.up * jumpForce);
            animator.SetBool("isJump", true);
        }

        if (Mathf.Abs(rb.linearVelocityX) < Mathf.Abs(maxWalkSpeed))
        {
            rb.AddForce(transform.right * walkForce * key);
        }
        time += Time.deltaTime;

        if (rb.linearVelocityY != 0)
        {
            animator.SetBool("isJump", true);
            //sr.sprite = jumpSprite;
        }
        else if (time > animtionPeriod)
        {
            animator.SetBool("isJump", false);
            time = 0;
            sr.sprite = walkSprites[idx];
            idx++;
            if (idx >= walkSprites.Length)
                idx = 0;
        }

        //time += time.deltatime;

        //if (rb.linearvelocityy != 0)
        //{
        //    sr.sprite = jumpsprite;
        //}
        //else if (time > animtionperiod)
        //{
        //    time = 0;
        //    sr.sprite = walksprites[idx];
        //    idx++;
        //    if (idx >= walksprites.length)
        //        idx = 0;
        //}
        animator.speed = Mathf.Abs(rb.linearVelocityX);

        if (transform.position.y <= -8)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("flag"))
        {
            SceneManager.LoadScene("ClearScene");
            Debug.Log("win");
        }
    }
}
