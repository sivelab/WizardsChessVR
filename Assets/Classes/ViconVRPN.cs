using UnityEngine;
using System.Collections;

public static class ViconVRPN
{
	public static Vector3 vrpnTrackerPos (string host, string obj, int channel)
	{
		Vector3 pos = VRPN.vrpnTrackerPos (obj + "@" + host, channel);
		return new Vector3 (pos.x, pos.z, pos.y);
	}
	
	public static Quaternion vrpnTrackerQuat (string host, string obj, int channel)
	{
		Quaternion quat = VRPN.vrpnTrackerQuat (obj + "@" + host, channel);
		return new Quaternion (quat.x, quat.z, quat.y, -quat.w);
	}
}
