using UnityEngine;
using System.Collections;

public class ChickenController : MonoBehaviour
{

        public float speed = 0.5f;
        CharacterController controller;

        public static float rotateSpeedMax = 5f;
        public float rotateSpeed = 0f;
        public float rotateAcceloration = 0f;
        public static float rotateAccelorationMax = 0.2f;

        private Animator animator;

        // Use this for initialization
        void Start ()
        {
                animator = (Animator)GetComponent<Animator> ();
                controller = (CharacterController)GetComponent<CharacterController> ();
        }
	
        // Update is called once per frame
        void Update ()
        {
                Vector3 forward = transform.right;
                
                animator.SetBool ("ForwardBlocked", Physics.Raycast
                                  (transform.position + new Vector3 (0f, 0.3f, 0f), forward, 0.5f));
                if (Network.isServer) {

                        controller.SimpleMove (forward * speed * animator.GetFloat ("WalkSpeed"));

                        rotateAcceloration = Mathf.Clamp (rotateAcceloration + Random.Range (-1f, 1f) * Time.deltaTime, 
                                                 - rotateAccelorationMax, rotateAccelorationMax);

                        rotateSpeed = Mathf.Clamp (rotateSpeed + rotateAcceloration * Time.deltaTime,
                                          - rotateSpeedMax, rotateSpeedMax);

                        transform.Rotate (Vector3.up * Time.deltaTime * rotateSpeed);
                }
        }
}
