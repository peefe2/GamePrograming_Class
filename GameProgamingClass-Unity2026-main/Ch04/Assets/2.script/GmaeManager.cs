using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GmaeManager : MonoBehaviour
{
    //public GameObject flag;
    //public GameObject car;
    public Transform flag;
    public Transform car;

    public TextMeshProUGUI distance;

    void Start()
    {
        //car = GameObject.Find("car");
    }


    void Update()
    {
        float length = flag.position.x - car.position.x;
        distance.text = "°Å¸® : " + length.ToString("F2") + "M";
    }
}
