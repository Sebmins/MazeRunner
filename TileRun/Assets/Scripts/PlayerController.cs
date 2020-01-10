using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject tileSpawner;
    public Animator ani;
    public static bool isTile;

    private Vector3 startPos;
    private Vector3 endPos;
    public float lerpTime = 1f;
    private float currentLerpTime = 0;

    void Start() {
        startPos = new Vector3(0,0,0);
        endPos = transform.position; 
        ani = GetComponent<Animator>();
    }
    public static bool infiteRun;

    void Update() {
        if (Timer.gameOver == true)
            return;

        if (infiteRun == true)
        {
            run();
        }

        if (FloorHandler.update == false)
        {
            swipeUp = false;
            //ani.SetBool("isRunning", false);
            //infiteRun = false;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            return;
        }

        checkPosition();
        mobileInput();

        if (rotationProgress < 1 && rotationProgress >= 0)
        {
            rotationProgress += Time.deltaTime * 5;
            // Debug.Log(rotationProgress);
            // Here we assign the interpolated rotation to transform.rotation
            // It will range from startRotation (rotationProgress == 0) to endRotation (rotationProgress >= 1)
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, rotationProgress);
        }
        //else if (Input.GetKey("up") && isTile == true) {
        else if ((swipeUp == true || Input.GetKey("up")) && isTile == true)
        {
            infiteRun = true;
        }


        if (Input.GetKeyUp("left") && rotationProgress >= 1 && infiteRun == false)
        {
            //transform.Rotate(0, -90, 0);
            StartRotating(-90);
        }
        if (Input.GetKeyUp("right") && rotationProgress >= 1 && infiteRun == false)
        {
            //transform.Rotate(0, 90, 0);
            StartRotating(90);
        }
    }

    Quaternion startRotation;
    Quaternion endRotation;
    float rotationProgress = 1;

    // Call this to start the rotation
    private void StartRotating(float yRotation)
    {
        // Here we cache the starting and target rotations
        startRotation = transform.rotation;
        endRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + yRotation, transform.rotation.eulerAngles.z);

        // This starts the rotation, but you can use a boolean flag if it's clearer for you
        rotationProgress = 0;
    }


    private void run()
    {
        //endPos = transform.position + transform.forward;
        currentLerpTime += Time.deltaTime;
        float Prec = currentLerpTime / lerpTime;
        transform.position = Vector3.Lerp(startPos, endPos, Prec);
        ani.SetBool("isRunning", true);

        //if (transform.position == endPos && isTile == true && Input.GetKey("up"))
        if (transform.position == endPos && isTile == true && (swipeUp == true ||  Input.GetKey("up")))
        {
            startPos = transform.position;
            endPos = transform.position + transform.forward;
            currentLerpTime = 0;
        }
        else if(transform.position == endPos)
        {
            ani.SetBool("isRunning", false);
            infiteRun = false;
        }

    }
    

    public LayerMask mask;
    private void checkPosition()
    {
        Vector3 vec = Quaternion.AngleAxis(0,transform.forward) * -transform.up;
        Ray ray = new Ray(transform.position + transform.forward + transform.up, vec);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 50, mask, QueryTriggerInteraction.Ignore))
        {
            isTile = true;
            Debug.DrawLine(ray.origin, hitInfo.point, Color.red);
        }
        else
        {
            isTile = false;
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 10, Color.green);
        }
    }

    private int minSwipeRecognition = 9999;
    private Vector2 swipePosLastFrame;
    private Vector2 swipePosCurrentFrame;
    private Vector2 currentSwipe;
    private bool swipeUp;
    private void mobileInput()
    {
        minSwipeRecognition = 10000;
        if (Input.GetMouseButton(0) && swipeUp == false)
        {

            swipePosCurrentFrame = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            if (swipePosLastFrame != Vector2.zero)
            {
                // Calculate the swipe direction
                currentSwipe = swipePosCurrentFrame - swipePosLastFrame;
                
                if (currentSwipe.sqrMagnitude < minSwipeRecognition)
                {
                    // Minium amount of swipe recognition
                    return;
                }

                currentSwipe.Normalize(); // Normalize it to only get the direction not the distance (would fake the balls speed)

                // Left/Right swipe
                if (currentSwipe.y > -0.5f && currentSwipe.y < 0.5f && rotationProgress >= 1)
                {
                    StartRotating(currentSwipe.x > 0 ? 90 : -90);
                }
                // Up/Down swipe
                else if (currentSwipe.x > -0.5f && currentSwipe.x < 0.5f && rotationProgress >= 1 && isTile == true)
                {
                    swipeUp = true;
                }
                
            }
            swipePosLastFrame = swipePosCurrentFrame;
        }

        if (Input.GetMouseButtonUp(0))
        {
            swipeUp = false;
            swipePosLastFrame = Vector2.zero;
            currentSwipe = Vector2.zero;
        }
    }
}
