using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class ItemGenerator : MonoBehaviour
{
    public GameObject applePrefab;
    public GameObject bombPrefab;

    public float span = 1f;
    float delta = 0f;
    public int ratio = 3; //30% bomb
    private void Update()
    {
        delta += Time.deltaTime;
        GameObject item;
        if(delta>span)
        {
            int dice = Random.Range(0, 10);
            if(dice<ratio)
            {
                item = Instantiate(bombPrefab);
            }
            else
            {
                item = Instantiate(applePrefab);
            }

            //GameObject Item= Instantiate(applePrefab);
            float x = Random.Range(-1, 2);
            float z = Random.Range(-1, 2);
            item.transform.SetParent(transform);
            item.transform.position= new Vector3(x,5,z);
            delta = 0;
        }
    }
}
