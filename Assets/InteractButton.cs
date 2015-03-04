using UnityEngine;
using System.Collections;

public class InteractButton : MonoBehaviour
{
        public Color startColor;
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

        }
}
