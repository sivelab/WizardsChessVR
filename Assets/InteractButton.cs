using UnityEngine;
using System.Collections;

public class InteractButton : MonoBehaviour
{
        public Board board;
        public string action;
        private Color startColor;
    
        public GameObject enable;
        public GameObject disable;

        void Start ()
        {
                startColor = renderer.material.color;
        }

        public void Hover ()
        {
                renderer.material.color = Color.white;
        }

        public void Unhover ()
        {
                renderer.material.color = startColor;
        }

        public void Action ()
        {
                if (enable != null) {
                        enable.SetActive (true);
                }
                if (disable != null) {
                        disable.SetActive (false);
                }
                if (Network.isServer) {
                        board.ClientAction (action, Network.player);
                } else {
                        board.networkView.RPC ("ClientAction", RPCMode.Server, action);
                }
        }
}
