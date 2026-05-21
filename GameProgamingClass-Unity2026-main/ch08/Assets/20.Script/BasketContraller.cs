using UnityEngine;

public class BasketContraller : MonoBehaviour
{
    public AudioClip appleSE;
    public AudioClip bombSE;

    private AudioSource aud;

    public GameDirector GD;

    private void Start()
    {
        Application.targetFrameRate = 60;
        aud=GetComponent<AudioSource>();
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                float x = Mathf.RoundToInt(hit.point.x);
                float z = Mathf.RoundToInt(hit.point.z);
                transform.position = new Vector3(x, 0, z);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Apple")
        {
            aud.PlayOneShot(appleSE);
            GD.GetApple();
            Debug.Log("»ç°ú Àâ¾Ò´Ù");
        }
        else if(other.gameObject.tag == "Bomb")
        {
            aud.PlayOneShot(bombSE);
            GD.GetBomb();
            Debug.Log("ÆøÅº Àâ¾Ò´Ù");
        }

        Destroy(other.gameObject);
    }
}
