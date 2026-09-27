using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 
/// https://play.unity.com/en/games/935f8402-ed77-4036-a5a1-f0b410e3db17/fidget
/// 
/// IDEAS 
/// 1. swipe to change rotate
/// 2. swipe and the amount of rotation changes with the swipe
/// 3. a grid, each can be swiped rotation
/// 4. a grid, all of them change rotation with one swipe but the colors greate a gradient across all 
/// 
/// Music: https://www.dronefx.com/v2.0/
/// Sounds: https://sfxr.me/
/// </summary>

public class SwipeDetector : MonoBehaviour
{
    public Camera mainCamera;
    public MeshRenderer Renderer;
    public Color color;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private Quaternion targetRotation;
    private bool isThisCubeSelected = false;
    public AudioSource audioSource;
    public List<AudioClip> swipeSounds = new List<AudioClip>();

    [Header("Rotation Settings")]
    [SerializeField]
    private float rotationSpeed = 10f;

    [SerializeField]
    private float minSwipeDistance = 50f;

    void Start()
    {
        // Initialize target rotation with the cube's starting rotation
        targetRotation = transform.rotation;
        mainCamera = Camera.main; // Cache the main camera reference
        color = new Color(Random.Range(.3f, 1), Random.Range(.3f, 1), Random.Range(.3f, 1), 1);
        StartCoroutine(LerpColorOverTime(GetColor(), 15f));
    }

    void Update()
    {
        DetectMobileSwipe();
        DetectMouseSwipe(); // For testing in Editor

        // Smoothly rotate the cube toward the target rotation over time
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private void DetectMobileSwipe()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // Check if this specific cube was touched
                if (IsTouchOverThisCube(touch.position))
                {
                    startTouchPosition = touch.position;
                    isThisCubeSelected = true;
                }
            }
            else if (touch.phase == TouchPhase.Ended && isThisCubeSelected)
            {
                endTouchPosition = touch.position;
                ProcessSwipe();
                isThisCubeSelected = false; // Reset selection
            }
        }
    }

    private void DetectMouseSwipe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Check if this specific cube was clicked
            if (IsTouchOverThisCube(Input.mousePosition))
            {
                startTouchPosition = Input.mousePosition;
                isThisCubeSelected = true;
            }
        }
        else if (Input.GetMouseButtonUp(0) && isThisCubeSelected)
        {
            endTouchPosition = Input.mousePosition;
            ProcessSwipe();
            isThisCubeSelected = false; // Reset selection
        }
    }

    private bool IsTouchOverThisCube(Vector2 screenPosition)
    {
        // Convert screen position to a 3D Ray
        Ray ray = mainCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        // Perform raycast to see if it hits this object's Collider
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform == this.transform)
            {
                return true;
            }
        }
        return false;
    }

    private void ProcessSwipe()
    {
        Vector2 swipeVector = endTouchPosition - startTouchPosition;

        if (swipeVector.magnitude > minSwipeDistance)
        {
            // Determine if swipe is mostly horizontal or vertical
            if (Mathf.Abs(swipeVector.x) > Mathf.Abs(swipeVector.y))
            {
                // Horizontal Swipes (Rotate around the Y-axis)
                if (swipeVector.x > 0)
                    RotateCube(Vector3.up * 90f); // Swipe Right -> Rotate 90 degrees right
                else
                    RotateCube(Vector3.up * -90f); // Swipe Left  -> Rotate 90 degrees left
            }
            else
            {
                // Vertical Swipes (Rotate around the X-axis)
                if (swipeVector.y > 0)
                    RotateCube(Vector3.right * 90f); // Swipe Up   -> Tumble Forward 90 degrees
                else
                    RotateCube(Vector3.right * -90f); // Swipe Down -> Tumble Backward 90 degrees
            }
            audioSource.PlayOneShot(swipeSounds[Random.Range(0,swipeSounds.Count)]);
            StartCoroutine(LerpColorOverTime(GetColor(), 0.3f));
        }
    }

    Color GetColor()
    {
        float r = Random.Range(.1f, .7f);
        float g = Random.Range(.1f, .7f);
        float b = Random.Range(.1f, .7f);
        return new Color(r, g, b, 1);
    }

    IEnumerator LerpColorOverTime(Color targetColor, float duration)
    {
        float timeElapsed = 0f;
        Color startColor = Renderer.material.color;

        while (timeElapsed < duration)
        {
            // Calculate progress fraction (0 to 1)
            float t = timeElapsed / duration;

            // Apply the interpolated color
            Renderer.material.color = Color.Lerp(startColor, targetColor, t);

            timeElapsed += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure it snaps perfectly to the target color at the end
        Renderer.material.color = targetColor;
    }

    private void RotateCube(Vector3 rotationAngles)
    {
        // Add the new rotation angle to the current target rotation
        targetRotation *= Quaternion.Euler(rotationAngles);
    }




}
