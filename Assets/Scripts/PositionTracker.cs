using UnityEngine;
using System.Collections;
public class PositionTracker : MonoBehaviour {
	public string address;
	public int channel;
		
	void Update () {

		transform.localPosition = VRPN.vrpnTrackerPos(address, channel);
		Debug.Log(transform.localPosition);
	}
}
