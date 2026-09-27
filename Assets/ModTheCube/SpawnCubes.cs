using UnityEngine;

public class SpawnCubes : MonoBehaviour
{
    public GameObject prefab;
    int spawnRange = 9;

    void Start()
    {
        float x = 2f;
        float z = .5f;
        for (float i = -spawnRange; i < spawnRange; i++)
        {
            Row(new Vector3(0, i, 0));
            x *= -1;
            z *= -1;
        }
    }

    void Row(Vector3 offset)
    {
        for (int j = -spawnRange; j < spawnRange; j++)
        {
            Vector3 pos = new Vector3(-j, offset.y, j - offset.z);
            Instantiate(prefab, pos, prefab.transform.rotation);
        }
    }
}
