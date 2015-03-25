using UnityEngine;
using System.Collections;

public class ResetPosition : MonoBehaviour
{
        Vector3 initPos;
        Quaternion initRot;
        // Use this for initialization
        void Start ()
        {
                initPos = transform.localPosition;
                initRot = transform.localRotation;
        }
	
        // Update is called once per frame
        void Reset ()
        {
                transform.localPosition = initPos;
                transform.localRotation = initRot;
                rigidbody.velocity = new Vector3 (0, 0, 0);
        }
}
