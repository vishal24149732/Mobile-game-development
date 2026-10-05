using UnityEngine;
using UnityEngine.InputSystem;

public class control : MonoBehaviour
{
    private PlayerInput inputActions;
    //public float mag = 1f;
    //private Vector2 dir;
    //// Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
    //    inputActions = new PlayerInput();
    //    inputActions.player.Enable();
    //    inputActions.player.touch.canceled += ProcessTouchComplete;
    //    inputActions.player.swipe.performed += ProcessSwipeDelta;
    //}

    //private void ProcessSwipeDelta(InputAction.CallbackContext context)
    //{
    //    dir = context.ReadValue<Vector2>();
    //}

    //private void ProcessTouchComplete(InputAction.CallbackContext context)
    //{
    //    Debug.Log("Touch complete");
    //    if (Mathf.Abs(dir.magnitude) < mag) return;
    //    Debug.Log("Swipe detected");

    //    Vector3 position = Vector3.zero;

    //    if (dir.x > 0)
    //    {
    //        Debug.Log("right");
    //        position.x = 10;
    //    }

    //    if (dir.y > 0)
    //    {
    //        Debug.Log("top");
    //        position.y = 10;
    //    }

    //    if (dir.x < 0)
    //    {
    //        Debug.Log("left");
    //        position.x = -10;

    //    }

    //    if (dir.y < 0)
    //    {
    //        Debug.Log("right");
    //        position.y = -10;
    //    }

    //    transform.position += position;
    //}
    //// Update is called once per frameplayuer
    //void Update()
    //{

    //}

    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered
    Vector3 pos = Vector3.zero;
    private Quaternion initialRotation;
    private bool gyroR = true;

    void Start()
    {
        dragDistance = Screen.height * 1 / 100; //dragDistance is 1% height of the screen
        Input.gyro.enabled = true;
        Debug.Log("gyro enabled");
        initialRotation = GyroToUnity(Input.gyro.attitude);
        Screen.orientation = ScreenOrientation.Portrait;
    }

    void Update()
    {

        Quaternion currentRotation =
        GyroToUnity(Input.gyro.attitude);
        Quaternion relativeRotation =
        Quaternion.Inverse(initialRotation) * currentRotation;
        Vector3 angles = relativeRotation.eulerAngles;

        float roll = angles.x;

        if (roll > 180f)
            roll -= 360f;
        if (gyroR)
        {

            if (roll > 10f)
            {
                Debug.Log("GYRO RIGHT");

                transform.position += Vector3.right * 5f * Time.deltaTime;
                //gyroR = false;
            }
            else if (roll < -10f)
            {
                Debug.Log("GYRO LEFT");

                transform.position += Vector3.left * 5f * Time.deltaTime;
                //gyroR = false;
            }
        }

        if (Mathf.Abs(roll) < 5f)
        {
            gyroR = true;
        }

        pos.y = -0.01f;
        transform.position += pos; 
        if (Input.touchCount == 1) // user is touching the screen with a single touch
        {
            Touch touch = Input.GetTouch(0); // get the touch
            if (touch.phase == UnityEngine.TouchPhase.Began) //check for the first touch
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == UnityEngine.TouchPhase.Moved) // update the last position based on where they moved
            {
                lp = touch.position;
            }
            else if (touch.phase == UnityEngine.TouchPhase.Ended) //check if the finger is removed from the screen
            {
                lp = touch.position;  //last touch position. Ommitted if you use list
                Vector3 position = Vector3.zero;

                //Check if drag distance is greater than 20% of the screen height
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance )
                {//It's a drag
                 //check if the drag is vertical or horizontal
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y) )
                    {   //If the horizontal movement is greater than the vertical movement...
                        if ((lp.x > fp.x) )  //If the movement was to the right)
                        {   //Right swipe
                            Debug.Log("Right Swipe");
                            position.x = 5;
                        }
                        else
                        {   //Left swipe
                            Debug.Log("Left Swipe");
                            position.x = -5;
                        }
                    }
                    else
                    {   //the vertical movement is greater than the horizontal movement
                        if (lp.y > fp.y)  //If the movement was up
                        {   //Up swipe
                            Debug.Log("Up Swipe");
                            //position.y = 10;
                        }
                        else
                        {   //Down swipe
                            Debug.Log("Down Swipe");
                            //position.y = -10;
                        }
                    }

                    transform.position += position;
                }
                else
                {   //It's a tap as the drag distance is less than 20% of the screen height
                    Debug.Log("Tap");
                }
            }
        }
    }

    private Quaternion GyroToUnity (Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
}
