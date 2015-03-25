using UnityEngine;
using System.Collections;

public class ActionLever : MonoBehaviour
{
        public GameObject target;
        public string message;

        // Update is called once per frame
        void Update ()
        {
                if (Network.isServer && hingeJoint.angle <= hingeJoint.limits.min) {
                        target.BroadcastMessage (message);
                }
        }
}
