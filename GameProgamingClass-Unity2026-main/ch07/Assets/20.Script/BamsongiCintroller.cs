using UnityEngine;

public class BamsongiCintroller : MonoBehaviour
{
    public MeshRenderer renderer;
    private void Start()
    {
        Application.targetFrameRate = 60;
        //Shoot(new Vector3(0, 200, 2000));
    }

    private void OnCollisionEnter(Collision collision)
    {
        renderer.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<ParticleSystem>().Play();
        Destroy(gameObject, 0.5f);
    }

    public void Shoot(Vector3 dir)
    {
        GetComponent<Rigidbody>().AddForce(dir);
    }
}
