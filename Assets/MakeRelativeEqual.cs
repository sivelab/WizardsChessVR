using UnityEngine;
using System.Collections;

public class MakeRelativeEqual : MonoBehaviour
{
        public Transform localReference;
        public static Transform toTrack;

        void Update ()
        {
                transform.position = transform.position + (toTrack.position - localReference.position);
        }
}
