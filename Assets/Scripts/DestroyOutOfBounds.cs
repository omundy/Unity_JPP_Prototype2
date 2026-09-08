using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    int topBound = 30;
    int lowerBound = -10;

    void Update()
    {
        if (transform.position.z > topBound)
        {
            Destroy(gameObject);
        }
        else if (transform.position.z < lowerBound)
        {
            Debug.Log("Game Over!");
            Destroy(gameObject);
        }
    }
}
