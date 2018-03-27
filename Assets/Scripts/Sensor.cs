using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class Sensor : MonoBehaviour
{
    public Text infoText;
	public Text pluginInfo;

    private AndroidJavaObject pluginObject = null;
	private AndroidJavaObject sensorpluginObject = null;
    private AndroidJavaObject activityContext = null;
    private float alpha = 0, beta = 0, gamma = 0;

    private Vector3 mOrientation;
    private Quaternion mQuat;
	private Quaternion mQuatAndro;

	private Vector3 mOrientationAndro;

    void Start()
    {
        mOrientation = new Vector3(0, 0, 0);
        mQuat = new Quaternion();
        if (Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
            }

            using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.mirage.androidplugin.AndroidPlugin"))
            {
                if (pluginClass != null)
                {
                    pluginObject = pluginClass.CallStatic<AndroidJavaObject>("getInstance");
                    pluginObject.Call("setContext", activityContext);
					//pluginInfo.text = "" + pluginObject.CallStatic<string>("HelloWorld");
                }
            }

			using (AndroidJavaClass sensorpluginClass = new AndroidJavaClass("com.example.androidsensorplugin.AndroidPlugin"))
			{
				pluginInfo.text = "sensorpluginClass";
				if (sensorpluginClass != null)
				{
					pluginInfo.text = "sensorpluginClass not null";
					sensorpluginObject = sensorpluginClass.CallStatic<AndroidJavaObject>("getInstance");
					sensorpluginObject.Call("setContext", activityContext);
					pluginInfo.text = pluginInfo.text + " " + sensorpluginObject.Call<string>("getPluginName");
				}
			}

        }
    }

    void Update()
    {
        if (pluginObject == null)
            return;

        alpha = pluginObject.Call<float>("getAzimut"); // Alfa
        beta = pluginObject.Call<float>("getPitch"); // beta
        gamma = pluginObject.Call<float>("getRoll"); // Gamma

		float azimut1 = sensorpluginObject.Call<float>("getAzimut"); // Alfa
		float pitch1 = sensorpluginObject.Call<float>("getPitch"); // beta
		float roll1 = sensorpluginObject.Call<float>("getRoll"); // Gamma

        mQuat.Set(pluginObject.Call<float>("getQuatX"),
            pluginObject.Call<float>("getQuatY"),
            pluginObject.Call<float>("getQuatZ"),
            pluginObject.Call<float>("getQuatW"));

		mQuatAndro.Set (azimut1, pitch1, roll1, 0);

        mOrientation.Set((float)(alpha * 180 / Math.PI),(float) (beta * 180 / Math.PI),(float) (gamma * 180 / Math.PI));
		mOrientationAndro.Set((float)(azimut1 * 180 / Math.PI),(float) (pitch1 * 180 / Math.PI),(float) (roll1 * 180 / Math.PI));

//        infoText.text = "alpha: " + alpha * 180 / Math.PI + "\n" +
//            "beta: " + beta * 180 / Math.PI+ "\n" +
//            "gamma: " + gamma * 180 / Math.PI;

        infoText.text = "alpha: " + alpha + "\n" +
            "beta: " + beta + "\n" +
            "gamma: " + gamma;
    }

    public Vector3 getOrientation()
    {
        return mOrientation;
    }

	public Vector3 getOrientationAndro()
	{
		return mOrientationAndro;
	}

    public Quaternion getQuaternion()
    {
//        return new Quaternion(0, beta, gamma, alpha);
//        mQuat.Set(alpha, beta, gamma, 0);
        return normalizeQuat(mQuat);
//        return getQuaternion(beta, gamma, alpha);
    }

	public Quaternion getQuaternionAndro(){
		return normalizeQuat(mQuatAndro);
	}

    public Quaternion getQuaternion(float x, float y, float z)
    {
        float degToRad = (float)Math.PI / 180;

        float _x, _y, _z;
        float _x_2, _y_2, _z_2;
        float cX, cY, cZ, sX, sY, sZ;

//        _z = z * degToRad;
//        _x = x * degToRad;
//        _y = y * degToRad;

        _z = z;
        _x = x;
        _y = y;

        _z_2 = _z / 2;
        _x_2 = _x / 2;
        _y_2 = _y / 2;

        cX = (float)Math.Cos(_x_2);
        cY = (float)Math.Cos(_y_2);
        cZ = (float)Math.Cos(_z_2);
        sX = (float)Math.Sin(_x_2);
        sY = (float)Math.Sin(_y_2);
        sZ = (float)Math.Sin(_z_2);

        Quaternion quaternion = new Quaternion(
            sX * cY * cZ - cX * sY * sZ, // X
            cX * sY * cZ + sX * cY * sZ, // Y
            cX * cY * sZ + sX * sY * cZ, // Z
            cX * cY * cZ - sX * sY * sZ  // W
            );

//        return quaternion;
        return normalizeQuat(quaternion);
    }

    private Quaternion normalizeQuat(Quaternion quaternion)
    {
        float len = (float)Math.Sqrt( quaternion.x * quaternion.x + quaternion.y * quaternion.y + quaternion.z * quaternion.z + quaternion.w * quaternion.w );

			if ( len == 0 ) {

				quaternion.x = 0;
				quaternion.y = 0;
				quaternion.z = 0;
				quaternion.w = 1;

			} else {

				len = 1 / len;

				quaternion.x *= len;
				quaternion.y *= len;
				quaternion.z *= len;
				quaternion.w *= len;

			}

			return quaternion;
    }

}
