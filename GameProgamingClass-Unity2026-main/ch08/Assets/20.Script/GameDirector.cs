using UnityEngine;
using TMPro;

public class GameDirector : MonoBehaviour
{
    public GameObject timeText;
    public GameObject pointText;

    float time = 60.0f;
    int point = 0;

    private void Update()
    {
        if (time < 0) return;
            
        time -= Time.deltaTime;
        timeText.GetComponent<TextMeshProUGUI>().text = "Time : "+time.ToString("F1");

        pointText.GetComponent<TextMeshProUGUI>().text = "point : " + point;

    }

    public void GetApple()
    {
        point += 100;
    }
    public void GetBomb()
    {
        point -= 100;
    }
}
