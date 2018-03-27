using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartScript : MonoBehaviour
{
    public Text infoText;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void myAction()
    {
        infoText.GetComponent<Text>().text = "clicked";
    }
}
