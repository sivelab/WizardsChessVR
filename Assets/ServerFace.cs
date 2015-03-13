using UnityEngine;
using System.Collections;

public class ServerFace : MonoBehaviour
{
    public GameObject FacePrefab;
    private GameObject FaceInstance;

    void OnConnectedToServer ()
    {
        FaceInstance = (GameObject)Network.Instantiate (FacePrefab, transform.position, transform.rotation, 0);
    }

    void OnServerInitialized ()
    {
        FaceInstance = (GameObject)Network.Instantiate (FacePrefab, transform.position, transform.rotation, 0);
    }
	
    void Update ()
    {
        if (FaceInstance != null) {
            FaceInstance.transform.position = transform.position;
            FaceInstance.transform.rotation = transform.rotation;
        } else {
            Debug.Log ("no face");
        }
    }
}
