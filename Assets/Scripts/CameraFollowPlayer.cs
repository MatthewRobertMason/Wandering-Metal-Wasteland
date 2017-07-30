using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour 
{
    public GameObject player = null;
    public float minCameraDistance = 0;
    public float maxCameraDistance = 105;

	// Update is called once per frame
	void Update () 
    {
        
        if ((player.transform.position.y > minCameraDistance) && (player.transform.position.y < maxCameraDistance))
        {
            this.transform.position = new Vector3(0.0f, player.transform.position.y, -10.0f);
            //this.transform.Translate(this.transform.position - player.transform.position);
        }
        /*
        if (player.transform.position.y < minCameraDistance)
        {
            this.transform.position = new Vector3(0.0f, minCameraDistance);
        }
        if (player.transform.position.y > maxCameraDistance)
        {
            this.transform.position = new Vector3(0.0f, maxCameraDistance);
        }*/
    }
}
