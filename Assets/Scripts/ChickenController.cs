using UnityEngine;
using System.Collections;

public class ChickenController : MonoBehaviour
{

        public float speed = 0.5f;
        CharacterController controller;

        // Use this for initialization
        void Start ()
        {
                controller = (CharacterController)GetComponent<CharacterController> ();
        }
	
        // Update is called once per frame
        void Update ()
        {
                controller.SimpleMove (transform.forward * speed);
        }
}
