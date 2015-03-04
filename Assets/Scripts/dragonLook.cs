using UnityEngine;
using System.Collections;

public class dragonLook : MonoBehaviour
{
        public Transform headPos;
        public Transform target;
        public AnimationCurve speedControl;

        private Animator animator;
        private float val = 0f;

        private float dval;
        // Use this for initialization
        void Start ()
        {
                animator = GetComponent<Animator> ();
        }
	
        // Update is called once per frame
        void Update ()
        {
                if (animator) {
                        Vector3 relative = headPos.InverseTransformPoint (target.position);
                        float speed = speedControl.Evaluate (Mathf.Clamp (Mathf.Abs (relative.z / 100), 0, speedControl.length));
                        if (relative.z > 0) {
                                dval = - speed;
                        } else {
                                dval = speed;
                        }
                        if (val < 0) {
                                val = 0;
                        }
                        if (val > 1) {
                                val = 1;
                        }

                        val += dval * Time.deltaTime;

                        animator.SetFloat ("Rightleft", val);

                }       
        }       
}
