using UnityEngine;
using System.Collections;

public class TrackerVicon : MonoBehaviour
{

    public string host;
    public string obj;
    public bool trackPosition;
    public bool trackRotation;

    // Update is called once per frame
    void Update()
    {
        if (trackPosition)
        {
            transform.position = ViconVRPN.vrpnTrackerPos(host, obj, 0);
        }
        if (trackRotation)
        {
            transform.rotation = ViconVRPN.vrpnTrackerQuat(host, obj, 0);
        }
    }
}
