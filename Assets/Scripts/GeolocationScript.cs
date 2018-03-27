using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GeolocationScript : MonoBehaviour
{

    public Text infoText;
    private int mWaitingSeconds = 20;

    public void Start()
    {

        // turn on location services, if available 
        Input.location.Start(10,0.1f);
        Invoke("secondTimeout", 1);
    }

    public void Update()
    {
        //Do nothing if location services are not available
        if (Input.location.isEnabledByUser)
        {
            // Wait until service initializes
            if (mWaitingSeconds < 0)
            {
                infoText.text = "Time is up";
                return;
            }

            if (Input.location.status == LocationServiceStatus.Failed)
            {
                infoText.text = "Unable to determine device location";
            }
            else
            {
               infoText.text = "Latitude: " + Input.location.lastData.latitude +
                                "\nLongitude: " + Input.location.lastData.longitude +
                                "\nAltitude: " + Input.location.lastData.altitude
                                + "\nHAccuracy: " + Input.location.lastData.horizontalAccuracy
                                + "\nTimestamp: " + Input.location.lastData.timestamp; 
            }
        }
        else
            infoText.text = "gps off";
    }

    private void secondTimeout()
    {
        mWaitingSeconds -= 1;
        Invoke("secondTimeout", 1);
    }

}
