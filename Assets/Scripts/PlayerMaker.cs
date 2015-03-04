using UnityEngine;
using System.Collections;

public class PlayerMaker : MonoBehaviour
{
        public GameObject Oculus;
        public GameObject Server;
        public Board board;
        void Start ()
        {
                if (Application.isEditor) {
                        GameObject.Instantiate (Server);
                } else {
                        GameObject.Instantiate (Oculus);
                }
        }

}
