using UnityEngine;
using System.Collections;

public class ServerHandler : MonoBehaviour {
	void Start () {
		if (Application.isEditor){
			Network.InitializeServer(32, 25000, false);
		} else {
			NetworkConnectionError error = Network.Connect(new []{"localhost","192.168.100.105", "131.212.41.254"}, 25000);
			if (error != NetworkConnectionError.NoError){
				Debug.LogError(error);
			}
		}
	}
	

	void Update () {
	
	}
}
