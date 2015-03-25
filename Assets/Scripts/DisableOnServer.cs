using UnityEngine;
using System.Collections;

public class DisableOnServer : MonoBehaviour
{

        void OnServerInitialized ()
        {
                gameObject.SetActive (false);
        }
}
