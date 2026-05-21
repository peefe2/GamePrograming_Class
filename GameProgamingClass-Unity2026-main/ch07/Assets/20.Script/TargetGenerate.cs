using UnityEngine;

public class TargetGenerate : MonoBehaviour
{
    public GameObject targetPrefab;
    public float minDistance = 10f;

    Transform[] targetPositions;
    private void Start()
    {
        targetPositions = GetComponentsInChildren<Transform>();
    }
    public void GenerateTarget(Vector3 playerPos)
    {

        int index;
        do
        {
            index = Random.Range(1, targetPositions.Length);
        } while (Vector3.Distance(playerPos, targetPositions[index].position) < minDistance);
        Vector3 position = targetPositions[index].position;
        GameObject target = Instantiate(targetPrefab, position, Quaternion.identity);
        target.transform.SetParent(transform);
    }
}
