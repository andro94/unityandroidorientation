using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;


public class RotationController : MonoBehaviour
{
    // Gyroscope-controlled camera for iPhone  Android revised 2.26.12
    // Stereoskopix FOV2GO Copyright (c) 2011 Perry Hoberman
    // Perry Hoberman <hoberman@bway.net>
    //
    // Usage:
    // Attach this script to main camera.
    // Note: Unity Remote does not currently support gyroscope.
    // Use Landscape Left for correct orientation
    //
    // This script uses three techniques to get the correct orientation out of the gyroscope attitude:
    // 1. creates a parent transform (camParent) and rotates it with eulerAngles
    // 2. for Android (Samsung Galaxy Nexus) only: remaps gyro.Attitude quaternion values from xyzw to wxyz (quatMap)
    // 3. multiplies attitude quaternion by quaternion quatMult
 
    // Also creates a grandparent (camGrandparent) which can be rotated to change heading
    // This node allows an arbitrary heading to be added to the gyroscope reading
    // so that the virtual camera can be facing any direction in the scene, no matter which way the phone is actually facing
    // Option for touch input - horizontal swipe controls heading

    private Sensor mSensor;
    private Quaternion worldQuat = new Quaternion((float)-Math.Sqrt(0.5), 0f, 0f, (float)Math.Sqrt(0.5));
    private int mWorldChoice = 0;

    public Text infoText;
    static bool gyroBool;
    private Gyroscope gyro;
    private Quaternion quatMult;
    private Quaternion quatMap;
    // camera grandparent node to rotate heading
    private GameObject camGrandparent;
    private float heading = 0;
 
    // mouse/touch input
    public bool touchRotatesHeading = true;
    private Vector2 screenSize;
    private Vector2 mouseStartPoint;
    private float headingAtTouchStart;
    //@script AddComponentMenu ("stereoskopix/s3d Gyro Cam")

	// Transformacion de los cubos
	public Transform cubo1;
	public Transform cuboAndro;
 
    void Awake()
    {
        mSensor = GetComponent<Sensor>();

      /*  // find the current parent of the camera's transform
        var currentParent = transform.parent;
        // instantiate a new transform
        var camParent = new GameObject ("camParent");
        // match the transform to the camera position
        camParent.transform.position = transform.position;
        // make the new transform the parent of the camera transform
        transform.parent = camParent.transform;
        // instantiate a new transform
        camGrandparent = new GameObject ("camGrandParent");
        // match the transform to the camera position
        camGrandparent.transform.position = transform.position;
        // make the new transform the grandparent of the camera transform
        camParent.transform.parent = camGrandparent.transform;
        // make the original parent the great grandparent of the camera transform
        camGrandparent.transform.parent = currentParent;
       
        // check whether device supports gyroscope
        #if UNITY_3_4
        gyroBool = Input.isGyroAvailable;
        #endif
        #if UNITY_3_5
        gyroBool = SystemInfo.supportsGyroscope;
        #endif

        #if UNITY_IPHONE
                camParent.transform.eulerAngles = Vector3(90,90,0);
                if (Screen.orientation == ScreenOrientation.LandscapeLeft) {
                    quatMult = Quaternion(0,0,0.7071,0.7071);
                } else if (Screen.orientation == ScreenOrientation.LandscapeRight) {
                    quatMult = Quaternion(0,0,-0.7071,0.7071);
                } else if (Screen.orientation == ScreenOrientation.Portrait) {
                    quatMult = Quaternion(0,0,1,0);
                } else if (Screen.orientation == ScreenOrientation.PortraitUpsideDown) {
                    quatMult = Quaternion(0,0,0,1);
                }
        #endif

		camParent.transform.eulerAngles = new Vector3(-90, 0, 0);

		*/
        
		#if UNITY_ANDROID
        
        if (Screen.orientation == ScreenOrientation.LandscapeLeft)
        {
            quatMult = new Quaternion(0, 0, 0.7071f, -0.7071f);
        }
        else if (Screen.orientation == ScreenOrientation.LandscapeRight)
        {
            quatMult = new Quaternion(0, 0, -0.7071f, -0.7071f);
        }
        else if (Screen.orientation == ScreenOrientation.Portrait)
        {
            quatMult = new Quaternion(0, 0, 0, 1);
        }
        else if (Screen.orientation == ScreenOrientation.PortraitUpsideDown)
        {
            quatMult = new Quaternion(0, 0, 1, 0);
        }
        #endif
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
   
        if (gyroBool) {
            gyro = Input.gyro;
            gyro.enabled = true;
            
        } else {
            #if UNITY_EDITOR
                print("NO GYRO");
            #endif
            infoText.text = "NO GYRO";
        }

//        infoText.text = "from rotation script: " + mSensor.getOrientation();
    }
 
    void Start() {
        screenSize.x = Screen.width;
        screenSize.y = Screen.height;
    }
 
    void Update () {
        if (gyroBool) {
         /*   #if UNITY_IPHONE
                quatMap = gyro.attitude;
            #endif
            #if UNITY_ANDROID
                quatMap = new Quaternion(gyro.attitude.w,gyro.attitude.x,gyro.attitude.y,gyro.attitude.z);
            #endif
            transform.localRotation = quatMap * quatMult;*/
        }
        else
        {

//            transform.localRotation = worldQuat * mSensor.getQuaternion();
           // transform.localRotation = mSensor.getQuaternion() * quatMult;
            
            /*infoText.text = "worldChice: " + mWorldChoice
                + "\nworldQuat * mSensor.getQuaternion(): " + worldQuat * mSensor.getQuaternion()
                + "\nmSensor.getQuaternion() * quatMult: " + mSensor.getQuaternion() * quatMult;*/

			cubo1.transform.localRotation = mSensor.getQuaternion ();
			cuboAndro.transform.localRotation = mSensor.getQuaternionAndro ();

			infoText.text = "quat: " + mSensor.getQuaternion()
				+ "\n quatAndro: " + mSensor.getQuaternionAndro();

        }
        #if (UNITY_IPHONE || UNITY_ANDROID) && !UNITY_EDITOR
            if (touchRotatesHeading) {
                GetTouchMouseInput();
            }
//            camGrandparent.transform.localEulerAngles.y = heading;
            camGrandparent.transform.localEulerAngles.Set(camGrandparent.transform.localEulerAngles.x, 
            heading, 
            camGrandparent.transform.localEulerAngles.z);
        #endif

    }
 
    void GetTouchMouseInput() {
        if(Input.GetMouseButtonDown(0)) {
            mouseStartPoint = Input.mousePosition;
            headingAtTouchStart = heading;
        } else if (Input.GetMouseButton(0)) {
            Vector2 delta;
            var mousePos = Input.mousePosition;
            delta.x = (mousePos.x - mouseStartPoint.x)/screenSize.x;
            delta.y = (mousePos.y - mouseStartPoint.y)/screenSize.y;
            heading = (headingAtTouchStart+delta.x*100);
            heading = heading%360;
        }
    }

    public void changeWorldQuat()
    {
        mWorldChoice = (mWorldChoice + 1)%3;
        switch (mWorldChoice)
        {
            case 0: worldQuat = new Quaternion((float)Math.Sqrt(0.5), (float)-Math.Sqrt(0.5), 0f, 0f);
                break;
            case 1: worldQuat = new Quaternion((float)Math.Sqrt(0.5), 0f, (float)-Math.Sqrt(0.5), 0f);
                break;
            case 2: worldQuat = new Quaternion((float)Math.Sqrt(0.5), 0f, 0f, (float)-Math.Sqrt(0.5));
                break;
        }
    }


//    void Start()
//    {
//        Input.compensateSensors = true;
//        Input.gyro.enabled = true;
//    }
//
//    void FixedUpdate()
//    {
//        transform.Rotate(0, -Input.gyro.rotationRateUnbiased.y, 0);
//    }

}
