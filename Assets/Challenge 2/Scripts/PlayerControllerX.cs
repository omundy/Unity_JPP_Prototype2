using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerX : MonoBehaviour
{
    public GameObject dogPrefab;
    public InputAction fireAction;
    float spamTime = 1f;
    bool canTrigger = true;

    // Start is called before the first frame update
    void Start()
    {
        fireAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        // On spacebar press, send dog
        if (fireAction.triggered && canTrigger)
        {
            canTrigger = false;
            Invoke("ResetSpamTrigger", spamTime);
            Instantiate(dogPrefab, transform.position, dogPrefab.transform.rotation);
        }
    }

    void ResetSpamTrigger() => canTrigger = true;
}
