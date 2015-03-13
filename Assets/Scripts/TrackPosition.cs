using UnityEngine;
using System.Collections;

public class TrackPosition : MonoBehaviour
{
        public string address;
        public int channel;

        void Start ()
        {
                MakeRelativeEqual.toTrack = this.transform;
        }

        void Update ()
        {
                transform.localPosition = VRPN.vrpnTrackerPos (address, channel);
        }
}
