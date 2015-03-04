using UnityEngine;	
using System.Collections;

public class Selector : MonoBehaviour
{
        public GameObject particleTrailPrefab;
        private GameObject particleTrail;
        public Board board;
        public float overlap = 0.2f;
        private int x;
        private int z;

        public GameObject FacePrefab;
        private GameObject FaceInstance;
        private InteractButton interaction;

        void Start ()
        {
                particleTrail = (GameObject)GameObject.Instantiate (particleTrailPrefab);
                board = Board.board;
        }

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
                RaycastHit hit;
                if (Physics.Raycast (new Ray (transform.position, transform.forward), out hit, Mathf.Infinity, 1 << 8)) {

                        InteractButton interact = (InteractButton)hit.transform.GetComponent<InteractButton> ();
                        if (interact != null) {
                                if (interaction != interact) {
                                        if (interaction != null) {
                                                interaction.Unhover ();
                                        }
                                        interaction = interact;
                                        interaction.Hover ();
                                }
                        } else {
                                if (interaction != null) {
                                        interaction.Unhover ();
                                        interaction = null;
                                }
                        }

                        Vector3 intersect = transform.position + transform.forward * hit.distance;
                        particleTrail.transform.position = intersect - transform.forward * 0.1f;
                        intersect = board.transform.InverseTransformPoint (intersect);
                        if (Mathf.Abs (intersect.x - x) > 0.5f + overlap ||
                                Mathf.Abs (intersect.z - z) > 0.5f + overlap) {
                                x = Mathf.RoundToInt (intersect.x);
                                z = Mathf.RoundToInt (intersect.z);
                        }
                } else {
                        x = -1;
                        z = -1;
                }
                board.Hover (x, z);

                if (FaceInstance != null) {
                        FaceInstance.transform.position = transform.position;
                        FaceInstance.transform.rotation = transform.rotation;
                }
        }
}
