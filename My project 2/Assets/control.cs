using UnityEngine;
using UnityEngine.InputSystem;
using CandyCoded.HapticFeedback;
using UnityEngine.SceneManagement;
using UnityEditor.Build.Content;

public class control : MonoBehaviour
{
    private PlayerInput inputActions;
    

    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered
    Vector3 pos = Vector3.zero;
    private Quaternion initialRotation;
    private bool gyroR = true;
    private float smoothTilt = 0f;
    public float sensitivity = 2f;
    private Rigidbody rb;
    private bool roundE = false;
    private bool roundS = false;
    public Camera cam;
    Vector3 pos2;
    Vector3 rot;
    GameObject bul;
    bool animationn = false;
    private Animator animator;
    public bool animfinish = false;
    private bool isFinished = false;
    Vector3 pos3;
    Vector3 pos4;

    void Start()
    {
        dragDistance = Screen.height * 1 / 100; //dragDistance is 1% height of the screen
        Input.gyro.enabled = true;
        Debug.Log("gyro enabled");
        initialRotation = GyroToUnity(Input.gyro.attitude);
        Screen.orientation = ScreenOrientation.Portrait;
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationY |
                         RigidbodyConstraints.FreezeRotationZ;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 acceleration = Input.acceleration;
        Quaternion currentRotation = GyroToUnity(Input.gyro.attitude);
        Quaternion relativeRotation = Quaternion.Inverse(initialRotation) * currentRotation;

        float roll = relativeRotation.eulerAngles.z;


        if (roll > 180f)
            roll -= 360f;

        if (Mathf.Abs(roll) < 2f)
            roll = 0f;

        // Smooth the movement
        smoothTilt = Mathf.Lerp(smoothTilt, roll / 20f, Time.deltaTime * 5f);

        // Move the ball
        transform.position += Vector3.left * smoothTilt * sensitivity * Time.deltaTime;

        //transform.position += Vector3.left * acceleration.x * sensitivity * Time.deltaTime;

        //pos.y = -0.01f;
        //transform.position += pos;

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

        if (!roundE)
        {
            rb.AddForce(Vector3.down * 1f, ForceMode.Force);
        } 
        if (roundS) 
        {
            rb.AddForce(Vector3.forward * 1f , ForceMode.Force);
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("round"))
        {
            Debug.Log("Touched a round object!");
            
            pos2.y = 10f;
            rot.x = 25f;
            HapticFeedback.MediumFeedback();
            roundE = true;
           
            if (!animationn)
            {
                pos3.y = 5;
                transform.position += pos3;
                animator.SetTrigger("Roll");
                animationn = true;
            }
            if (animfinish)
            {
                roundE = true;
                cam.transform.position += pos2;
                cam.transform.rotation = Quaternion.Euler(25f, 0f, 0f);
            }
        }

        if (collision.gameObject.CompareTag("obs"))
        {
            Debug.Log("Hit");
            //Handheld.Vibrate();
            HapticFeedback.LightFeedback();
            
        }

        if (collision.gameObject.CompareTag("finish"))
        {
            Debug.Log("Hit");
            //Handheld.Vibrate();
            HapticFeedback.HeavyFeedback();
            ScoreManager.score++;

            PlayerPrefs.SetInt("PlayerScore", ScoreManager.score);
            PlayerPrefs.Save();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private Quaternion GyroToUnity (Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }

    public void AnimFinish ()
    {
        Debug.Log("Actttt ");
        animfinish = true;
        
        roundS = true;
        cam.transform.position += pos2;
        cam.transform.rotation = Quaternion.Euler(25f, 0f, 0f);
    }

    public void Ground ()
    {
        pos4.y = -4;
        transform.position += pos4;
    }
}
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

//https://sketchfab.com/3d-models/stylized-pangolin-832c908893b14e68a2aaf1aaba0f61df