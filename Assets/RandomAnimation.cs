using UnityEngine;
using System.Collections;

public class RandomAnimation : MonoBehaviour
{
        private Animator animator;

        // Use this for initialization
        void Start ()
        {
                animator = GetComponent<Animator> ();
        }
	
        // Update is called once per frame
        void Update ()
        {
                animator.SetFloat ("Random", Random.Range (0f, 1f));
        }
}
