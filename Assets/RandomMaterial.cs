using UnityEngine;
using System.Collections;

public class RandomMaterial : MonoBehaviour
{
        public Material[] choices;

        private static int cur = 0;

        void Start ()
        {
                renderer.material = choices [cur % choices.Length];
                cur += 1;
        }
}
