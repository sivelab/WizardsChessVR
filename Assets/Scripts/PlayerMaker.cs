using UnityEngine;
using System.Collections;

public class PlayerMaker : MonoBehaviour
{
        public GameObject Oculus;
        public GameObject Server;

        void Start ()
        {
                if (Application.isEditor) {
                        GameObject.Instantiate (Server);
                } else {
                        GameObject.Instantiate (Oculus);
                }
        }

}
